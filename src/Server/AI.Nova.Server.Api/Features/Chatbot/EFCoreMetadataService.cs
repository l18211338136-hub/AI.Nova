﻿﻿﻿using System.Text;
using AI.Nova.Server.Api.Features.Knowledge;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AI.Nova.Server.Api.Features.Chatbot;

public partial class EFCoreMetadataService
{
    [AutoInject] private AppDbContext dbContext = default!;
    [AutoInject] private ILogger<EFCoreMetadataService> logger = default!;
    [AutoInject] private KnowledgeEmbeddingService knowledgeEmbeddingService = default!;
    [AutoInject] private IChatClient chatClient = default!;

    private const string SYSTEM_DOCUMENTATION_KB_NAME = "系统文档";
    private const string DATABASE_SCHEMA_DOC_NAME = "数据库架构";

    public async Task<DatabaseSchemaInfo> GetDatabaseSchemaAsync(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("开始获取数据库架构信息");

        var model = dbContext.GetService<IDesignTimeModel>().Model;
        var entityTypes = model.GetEntityTypes()
            .Where(e =>
            {
                var tableName = e.GetTableName();
                return tableName != null &&
                       !tableName.StartsWith("Hangfire", StringComparison.OrdinalIgnoreCase) &&
                       !tableName.StartsWith("Knowledge", StringComparison.OrdinalIgnoreCase);
            })
            .OrderBy(e => e.Name)
            .ToList();

        var schemaInfo = new DatabaseSchemaInfo
        {
            Tables = entityTypes.Select(e => new TableSchema
            {
                TableName = e.GetTableName() ?? e.Name,
                EntityName = e.ClrType.Name,
                Comment = e.GetComment(),
                Columns = e.GetProperties().Select(p => new ColumnSchema
                {
                    ColumnName = p.Name,
                    PropertyName = p.Name,
                    ColumnType = p.GetColumnType() ?? p.ClrType.Name,
                    ClrType = p.ClrType.Name,
                    MaxLength = p.GetMaxLength(),
                    IsNullable = p.IsNullable,
                    IsPrimaryKey = p.IsPrimaryKey(),
                    Comment = p.GetComment(),
                    InferredTableName = InferRelatedTableName(p, entityTypes)
                }).ToList()
            }).ToList()
        };

        logger.LogInformation("获取到 {Count} 个表的架构信息", schemaInfo.Tables.Count);
        return schemaInfo;
    }

    public async Task<DatabaseSchemaInfo> GetRelevantSchemaAsync(
        string naturalLanguageQuery,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation("根据自然语言查询获取相关架构: {Query}", naturalLanguageQuery);

        var knowledgeBase = await dbContext.KnowledgeBases
            .FirstOrDefaultAsync(kb => kb.Name == SYSTEM_DOCUMENTATION_KB_NAME, cancellationToken);

        if (knowledgeBase == null)
        {
            logger.LogWarning("知识库 '{KnowledgeBaseName}' 不存在", SYSTEM_DOCUMENTATION_KB_NAME);
            return await GetDatabaseSchemaAsync(cancellationToken);
        }

        var relevantChunks = await knowledgeEmbeddingService.SearchChunks(
            knowledgeBase.Id,
            naturalLanguageQuery,
            vectorWeight: 0.85f,
            docName: DATABASE_SCHEMA_DOC_NAME,
            cancellationToken);

        if (!relevantChunks.Any())
        {
            logger.LogWarning("未找到相关的架构片段，返回完整架构");
            return await GetDatabaseSchemaAsync(cancellationToken);
        }

        var model = dbContext.GetService<IDesignTimeModel>().Model;
        var allEntityTypes = model.GetEntityTypes().ToList();
        var relevantTableScores = new Dictionary<string, double>();

        foreach (var chunk in relevantChunks)
        {
            var rawContent = chunk.RawContent ?? "";

            var tableMatch = System.Text.RegularExpressions.Regex.Match(
                rawContent,
                @"表名:\s*([^\s<]+)");

            if (tableMatch.Success)
            {
                var tableName = tableMatch.Groups[1].Value;
                var score = chunk.Score;

                if (!relevantTableScores.ContainsKey(tableName) || score > relevantTableScores[tableName])
                {
                    relevantTableScores[tableName] = score;
                }
            }
        }

        var candidateEntityTypes = allEntityTypes
            .Where(e =>
            {
                var tableName = e.GetTableName();
                return tableName != null &&
                       relevantTableScores.ContainsKey(tableName) &&
                       !tableName.StartsWith("Hangfire", StringComparison.OrdinalIgnoreCase) &&
                       !tableName.StartsWith("Knowledge", StringComparison.OrdinalIgnoreCase);
            })
            .OrderByDescending(e =>
            {
                var tableName = e.GetTableName();
                return relevantTableScores.TryGetValue(tableName, out var score) ? score : 0;
            })
            .ThenBy(e => e.Name)
            .ToList();

        if (!candidateEntityTypes.Any())
        {
            logger.LogWarning("未找到候选表，返回完整架构");
            return await GetDatabaseSchemaAsync(cancellationToken);
        }

        var selectedTableNames = await SelectRelevantTablesWithAI(
            naturalLanguageQuery,
            candidateEntityTypes,
            relevantTableScores,
            cancellationToken);

        if (!selectedTableNames.Any())
        {
            logger.LogWarning("AI 未选择任何表，使用候选表");
            selectedTableNames = candidateEntityTypes
                .Select(e => e.GetTableName() ?? e.Name)
                .Take(10)
                .ToList();
        }

        var relevantEntityTypes = allEntityTypes
            .Where(e =>
            {
                var tableName = e.GetTableName();
                return tableName != null &&
                       selectedTableNames.Contains(tableName);
            })
            .OrderBy(e => selectedTableNames.IndexOf(e.GetTableName() ?? e.Name))
            .ToList();

        var schemaInfo = new DatabaseSchemaInfo
        {
            Tables = relevantEntityTypes.Select(e => new TableSchema
            {
                Score = relevantTableScores.TryGetValue(e.GetTableName() ?? e.Name, out var score) ? score : 0,
                TableName = e.GetTableName() ?? e.Name,
                EntityName = e.ClrType.Name,
                Comment = e.GetComment(),
                Columns = e.GetProperties().Select(p => new ColumnSchema
                {
                    ColumnName = p.Name,
                    PropertyName = p.Name,
                    ColumnType = p.GetColumnType() ?? p.ClrType.Name,
                    ClrType = p.ClrType.Name,
                    MaxLength = p.GetMaxLength(),
                    IsNullable = p.IsNullable,
                    IsPrimaryKey = p.IsPrimaryKey(),
                    Comment = p.GetComment(),
                    InferredTableName = InferRelatedTableName(p, allEntityTypes)
                }).ToList()
            }).ToList()
        };

        logger.LogInformation("获取到 {Count} 个相关表的架构信息", schemaInfo.Tables.Count);
        return schemaInfo;
    }

    private string? InferRelatedTableName(IProperty property, IReadOnlyList<IEntityType> entityTypes)
    {
        if (property.IsPrimaryKey() || !property.Name.EndsWith("Id", StringComparison.OrdinalIgnoreCase))
            return null;

        var potentialTable = property.Name[..^2];
        var hasMatchingEntity = entityTypes.Any(e =>
            e.ClrType.Name == potentialTable ||
            e.GetTableName() == potentialTable);

        return hasMatchingEntity ? potentialTable : null;
    }

    public string FormatSchemaForAI(DatabaseSchemaInfo schemaInfo)
    {
        var sb = new StringBuilder();
        sb.AppendLine("## 数据库架构信息\n");

        foreach (var table in schemaInfo.Tables)
        {
            sb.AppendLine($"### 表名: {table.TableName}");
            sb.AppendLine($"实体类: {table.EntityName}");

            if (!string.IsNullOrEmpty(table.Comment))
            {
                sb.AppendLine($"说明: {table.Comment}");
            }

            sb.AppendLine("\n**列信息:**");
            sb.AppendLine("| 列名 | 类型 | 长度 | 可空 | 主键 | 关联表 | 注释 |");
            sb.AppendLine("|------|------|------|------|------|--------|------|");

            foreach (var column in table.Columns)
            {
                var maxLength = column.MaxLength?.ToString() ?? "-";
                var isNullable = column.IsNullable ? "是" : "否";
                var isPrimaryKey = column.IsPrimaryKey ? "是" : "-";
                var inferredTable = column.InferredTableName ?? "-";
                var comment = (column.Comment ?? "-").Replace("\n", " ");

                sb.AppendLine($"| {column.ColumnName} | {column.ColumnType} | {maxLength} | {isNullable} | {isPrimaryKey} | {inferredTable} | {comment} |");
            }

            sb.AppendLine();
        }

        return sb.ToString();
    }

    private async Task<List<string>> SelectRelevantTablesWithAI(string naturalLanguageQuery, IReadOnlyList<IEntityType> candidateEntityTypes, Dictionary<string, double> relevantTableScores, CancellationToken cancellationToken)
    {
        try
        {
            var tableList = candidateEntityTypes
                .Select(e =>
                {
                    var tableName = e.GetTableName() ?? e.Name;
                    var score = relevantTableScores.TryGetValue(tableName, out var s) ? s : 0;
                    var comment = e.GetComment() ?? "";
                    return $"{tableName} | score={score:F3} | {comment}";
                })
                .ToList();

            var tableListText = string.Join("\n", tableList);

            var systemPrompt = """
你是数据库专家。

任务：
从候选表中选择最相关的表。

=====================
规则（必须遵守）
=====================

1. 只返回表名
2. 每行一个表名
3. 按相关性排序
4. 只能从候选列表中选择
5. 不允许输出任何解释
6. 不允许编造表名

=====================
输出示例
=====================

Orders
Users
Products
""";

            var userPrompt = $"""
用户查询:
{naturalLanguageQuery}

候选表:
{tableListText}
""";

            var response = await chatClient.GetResponseAsync(
                messages:
                [
                    new ChatMessage(ChatRole.System, systemPrompt),
                    new ChatMessage(ChatRole.User, userPrompt)
                ],
                options: new ChatOptions
                {
                    Temperature = 0.1f, // 🔥降低随机性
                    MaxOutputTokens = 512
                },
                cancellationToken: cancellationToken);

            var text = response.Text?.Trim();

            if (string.IsNullOrWhiteSpace(text))
                return [];

            var candidateSet = candidateEntityTypes
                .Select(e => e.GetTableName() ?? e.Name)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var selectedTables = text
                .Split('\n', StringSplitOptions.RemoveEmptyEntries)
                .Select(t => t.Trim())
                .Where(t => candidateSet.Contains(t)) // 🔥防 hallucination
                .Distinct()
                .ToList();

            logger.LogInformation("AI 选择了 {Count} 个相关表: {Tables}",
                selectedTables.Count,
                string.Join(", ", selectedTables));

            return selectedTables;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "AI 选择相关表时出错");
            return [];
        }
    }

    //    private async Task<List<string>> SelectRelevantTablesWithAI(
    //        string naturalLanguageQuery,
    //        IReadOnlyList<IEntityType> candidateEntityTypes,
    //        Dictionary<string, double> relevantTableScores,
    //        CancellationToken cancellationToken)
    //    {
    //        try
    //        {
    //            var tableList = candidateEntityTypes
    //                .Select(e =>
    //                {
    //                    var tableName = e.GetTableName() ?? e.Name;
    //                    var score = relevantTableScores.TryGetValue(tableName, out var s) ? s : 0;
    //                    var comment = e.GetComment() ?? "";
    //                    return $"- {tableName} (相似度: {score:F3}): {comment}";
    //                })
    //                .ToList();

    //            var tableListText = string.Join("\n", tableList);

    //            var systemPrompt = @"你是一个数据库架构专家。请根据用户的自然语言查询，从以下候选表中选择最相关的表。

    //请只返回表名，每行一个表名，按相关性从高到低排序。不要包含任何其他内容。";

    //            var userPrompt = $@"用户查询: {naturalLanguageQuery}

    //候选表列表:
    //{tableListText}";

    //            var agent = chatClient.AsAIAgent(
    //                instructions: systemPrompt,
    //                name: "TableSelectorAgent",
    //                description: "Selects relevant database tables based on natural language queries");

    //            var response = await agent.RunAsync(
    //                [
    //                    new ChatMessage(ChatRole.User, userPrompt)
    //                ],
    //                cancellationToken: cancellationToken);

    //            var selectedTables = response.Text
    //                .Split('\n', StringSplitOptions.RemoveEmptyEntries)
    //                .Select(line => line.Trim())
    //                .Where(line => !string.IsNullOrEmpty(line))
    //                .ToList();

    //            logger.LogInformation("AI 选择了 {Count} 个相关表: {Tables}", selectedTables.Count, string.Join(", ", selectedTables));

    //            return selectedTables;
    //        }
    //        catch (Exception ex)
    //        {
    //            logger.LogError(ex, "AI 选择相关表时出错");
    //            return [];
    //        }
    //    }
}

public class DatabaseSchemaInfo
{
    public List<TableSchema> Tables { get; set; } = [];
}

public class TableSchema
{
    public double Score { get; set; }
    public string TableName { get; set; } = string.Empty;
    public string EntityName { get; set; } = string.Empty;
    public string? Comment { get; set; }
    public List<ColumnSchema> Columns { get; set; } = [];
}

public class ColumnSchema
{
    public string ColumnName { get; set; } = string.Empty;
    public string PropertyName { get; set; } = string.Empty;
    public string ColumnType { get; set; } = string.Empty;
    public string ClrType { get; set; } = string.Empty;
    public int? MaxLength { get; set; }
    public bool IsNullable { get; set; }
    public bool IsPrimaryKey { get; set; }
    public string? Comment { get; set; }
    public string? InferredTableName { get; set; }
}
