using System.Data;

namespace AI.Nova.Server.Api.Features.Chatbot;

public partial class TextToSqlService
{
    [AutoInject] private AppDbContext dbContext = default!;
    [AutoInject] private EFCoreMetadataService metadataService = default!;
    [AutoInject] private ILogger<TextToSqlService> logger = default!;
    [AutoInject] private CoderClient coderClient = default!;
    [AutoInject] private ReportGeneratorService reportGeneratorService = default!;
    [AutoInject] private ServerApiSettings appSettings = default!;
    [AutoInject] private IWebHostEnvironment webHostEnvironment = default!;

    public async Task<TextToSqlReportResult> GenerateReportAsync(
        string naturalLanguageQuery,
        string? reportTitle = null,
        string? reportDescription = null,
        CancellationToken cancellationToken = default)
    {
        var result = new TextToSqlReportResult();

        try
        {
            logger.LogInformation("开始生成报表: {Query}", naturalLanguageQuery);

            result.ReportTitle = reportTitle ?? "数据查询报表";
            result.ReportDescription = reportDescription ?? $"基于查询: {naturalLanguageQuery}";
            result.OriginalQuery = naturalLanguageQuery;
            result.GeneratedAt = DateTimeOffset.UtcNow;

            result.Step1_RetrievedSchema = await GetRelevantSchemaAsync(naturalLanguageQuery, cancellationToken);
            result.Step2_GeneratedSqlQueries = await GenerateMultipleSqlQueriesAsync(naturalLanguageQuery, result.Step1_RetrievedSchema, cancellationToken);
            result.Step3_ExecutionResults = await ExecuteMultipleSqlQueriesAsync(result.Step2_GeneratedSqlQueries, cancellationToken);

            result.Step4_HtmlReport = await reportGeneratorService.GenerateHtmlReportAsync(
                result.ReportTitle,
                result.ReportDescription,
                result.OriginalQuery,
                result.Step3_ExecutionResults,
                result.Step2_GeneratedSqlQueries,
                result.Step1_RetrievedSchema,
                cancellationToken);

            result.ReportFilePath = await SaveReportToFileAsync(
                result.ReportTitle,
                result.Step4_HtmlReport,
                cancellationToken);

            result.Success = true;
            result.ErrorMessage = null;

            logger.LogInformation("报表生成成功，生成了 {Count} 条 SQL 查询", result.Step2_GeneratedSqlQueries.Count);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "报表生成失败");
            result.Success = false;
            result.ErrorMessage = ex.Message;
        }

        return result;
    }

    private async Task<DatabaseSchemaInfo> GetRelevantSchemaAsync(
        string naturalLanguageQuery,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("步骤 1: 获取相关数据库架构信息");

        var schemaInfo = await metadataService.GetRelevantSchemaAsync(naturalLanguageQuery, cancellationToken);

        if (!schemaInfo.Tables.Any())
        {
            logger.LogWarning("未找到相关的数据库架构信息");
            throw new InvalidOperationException("未找到相关的数据库架构信息。请尝试更具体的查询。");
        }

        logger.LogInformation("获取到 {Count} 个相关表的架构信息", schemaInfo.Tables.Count);
        return schemaInfo;
    }

    private async Task<List<SqlQueryInfo>> GenerateMultipleSqlQueriesAsync(string naturalLanguageQuery, DatabaseSchemaInfo schemaInfo, CancellationToken cancellationToken)
    {
        logger.LogInformation("步骤 2: 生成多条 SQL 查询");

        var schemaText = metadataService.FormatSchemaForAI(schemaInfo);

        var systemPrompt = $$"""
    你是一个专业的 SQL 查询生成专家，专门针对 PostgreSQL 数据库和 EF Core ORM。你的任务是根据用户的自然语言查询，生成多条相关的 SQL 查询，为报表提供丰富的数据。

    ## PostgreSQL 语法规则（必须严格遵守）:

    1. 表名和列名必须使用双引号
    2. 所有表名前必须加上 schema 名称（"public"）
    3. 字符串使用单引号
    4. 使用 LIMIT 而不是 TOP
    5. 只允许 SELECT

    ## 生成规则:
    - 必须生成 3-5 条 SQL
    - 必须覆盖：主查询、汇总、趋势、排名、统计
    - 必须严格基于 schema

    ## ⚠️ 强制规则（最高优先级）：

    1. 只能使用提供的 schema 中的表和列
    2. 必须使用 "public"."TableName"
    3. 所有列必须使用双引号
    4. 禁止省略 schema
    5. 禁止使用未提供的表（如 TestTables 除非 schema 明确存在）
    6. 禁止“猜测关系”

    如果无法确定关系，必须使用已知表结构

    ## 可用表结构:
    {{schemaText}}

    
    ## 输出格式要求（严格）

    必须返回 JSON 数组，每个元素结构如下：

    {
      "queryType": string,
      "description": string,
      "sql": string,
      "priority": number
    }

    规则：
    - 只返回 JSON
    - 不允许解释
    - 不允许示例数据
    - 不允许 markdown
    """;

        var userPrompt = $"""
    ## 用户查询:
    {naturalLanguageQuery}

    请生成 3-5 条 PostgreSQL SQL 查询
    """;

        var chatOptions = new ChatOptions
        {
            Temperature = 0.2f, // 降低随机性，避免 JSON 乱
            MaxOutputTokens = 8192,
            ResponseFormat = ChatResponseFormat.Text
        };

        var response = await coderClient.Client.GetResponseAsync(
            messages: new[]
            {
                new ChatMessage(ChatRole.System, systemPrompt),
                new ChatMessage(ChatRole.User, userPrompt)
            },
            options: chatOptions,
            cancellationToken: cancellationToken);

      var content = response.Text?.Trim();

        if (string.IsNullOrWhiteSpace(content))
            throw new InvalidOperationException("AI 未返回内容");

        logger.LogInformation("AI 原始返回: {Content}", content);

        // 🔥 关键：处理 ```json 包裹
        content = ExtractJson(content);

        List<SqlQueryInfo>? result;
        try
        {
            result = JsonSerializer.Deserialize<List<SqlQueryInfo>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "JSON 解析失败: {Content}", content);
            throw new InvalidOperationException("SQL JSON 解析失败");
        }

        if (result == null || result.Count == 0)
            throw new InvalidOperationException("未生成有效 SQL");

        logger.LogInformation("生成了 {Count} 条 SQL 查询", result.Count);

        foreach (var query in result)
        {
            logger.LogInformation("  - {QueryType}: {Description}", query.QueryType, query.Description);
        }

        return result;
    }

    private async Task<List<SqlExecutionResult>> ExecuteMultipleSqlQueriesAsync(
        List<SqlQueryInfo> sqlQueries,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("步骤 3: 执行 {Count} 条 SQL 查询", sqlQueries.Count);

        var results = new List<SqlExecutionResult>();

        foreach (var sqlQueryInfo in sqlQueries.OrderBy(q => q.Priority))
        {
            var result = await ExecuteSqlQueryAsync(sqlQueryInfo, cancellationToken);
            results.Add(result);
        }

        var successCount = results.Count(r => r.Success);
        logger.LogInformation("SQL 查询执行完成，成功 {SuccessCount}/{TotalCount}", successCount, results.Count);

        return results;
    }

    private async Task<SqlExecutionResult> ExecuteSqlQueryAsync(
        SqlQueryInfo sqlQueryInfo,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("执行 SQL 查询: {QueryType} - {Description} - {Sql}", sqlQueryInfo.QueryType, sqlQueryInfo.Description, sqlQueryInfo.Sql);

        var result = new SqlExecutionResult
        {
            QueryType = sqlQueryInfo.QueryType,
            Description = sqlQueryInfo.Description,
            Sql = sqlQueryInfo.Sql,
            Priority = sqlQueryInfo.Priority
        };

        try
        {
            var connection = dbContext.Database.GetDbConnection();

            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync(cancellationToken);
            }

            using var command = connection.CreateCommand();
            command.CommandText = sqlQueryInfo.Sql;
            command.CommandTimeout = 30;

            using var reader = await command.ExecuteReaderAsync(cancellationToken);

            var dataTable = new DataTable();
            dataTable.Load(reader);

            result.RowCount = dataTable.Rows.Count;
            result.ColumnCount = dataTable.Columns.Count;
            result.Columns = dataTable.Columns.Cast<DataColumn>()
                .Select(c => c.ColumnName)
                .ToList();

            result.Data = dataTable.Rows.Cast<DataRow>()
                .Select(row => dataTable.Columns.Cast<DataColumn>()
                    .ToDictionary(col => col.ColumnName, col => row[col]?.ToString() ?? "NULL"))
                .ToList();

            result.Success = true;
            result.ErrorMessage = null;

            logger.LogInformation("SQL 查询执行成功，返回 {RowCount} 行数据", result.RowCount);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "SQL 查询执行失败: {QueryType}", sqlQueryInfo.QueryType);
            result.Success = false;
            result.ErrorMessage = ex.Message;
            result.RowCount = 0;
            result.ColumnCount = 0;
            result.Columns = [];
            result.Data = [];
        }

        return result;
    }

    private async Task<string> SaveReportToFileAsync(string reportTitle, string htmlReport, CancellationToken cancellationToken)
    {
        var reportsDir = Path.Combine(webHostEnvironment.WebRootPath, appSettings.ReportsDir);

        if (!Directory.Exists(reportsDir))
        {
            Directory.CreateDirectory(reportsDir);
        }

        var fileName = $"{Guid.NewGuid():N}.html";
        var filePath = Path.Combine(reportsDir, fileName);

        await File.WriteAllTextAsync(filePath, htmlReport, cancellationToken);

        var relativePath = $"{appSettings.ReportsDir}{fileName}";
        return relativePath;
    }

    private string ExtractJson(string text)
    {
        if (text.StartsWith("```"))
        {
            var start = text.IndexOf('[');
            var end = text.LastIndexOf(']');
            if (start >= 0 && end > start)
            {
                return text.Substring(start, end - start + 1);
            }
        }

        return text;
    }
}

public class TextToSqlReportResult
{
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }

    public string ReportTitle { get; set; } = string.Empty;
    public string ReportDescription { get; set; } = string.Empty;
    public string OriginalQuery { get; set; } = string.Empty;
    public DateTimeOffset GeneratedAt { get; set; }

    public DatabaseSchemaInfo Step1_RetrievedSchema { get; set; } = new();
    public List<SqlQueryInfo> Step2_GeneratedSqlQueries { get; set; } = [];
    public List<SqlExecutionResult> Step3_ExecutionResults { get; set; } = [];
    public string Step4_HtmlReport { get; set; } = string.Empty;
    public string ReportFilePath { get; set; } = string.Empty;
}

public class SqlQueryInfo
{
    public string QueryType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Sql { get; set; } = string.Empty;
    public int Priority { get; set; }
}

public class SqlExecutionResult
{
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
    public string QueryType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Sql { get; set; } = string.Empty;
    public int Priority { get; set; }
    public int RowCount { get; set; }
    public int ColumnCount { get; set; }
    public List<string> Columns { get; set; } = [];
    public List<Dictionary<string, string>> Data { get; set; } = [];
}
