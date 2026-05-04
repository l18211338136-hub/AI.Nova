using System.Text;

namespace AI.Nova.Server.Api.Features.Chatbot;

public partial class ReportGeneratorService
{
    [AutoInject] private ILogger<ReportGeneratorService> logger = default!;
    [AutoInject] private IChatClient chatClient = default!;
    [AutoInject] private CoderClient coderClient = default!;

    public async Task<string> GenerateHtmlReportAsync(
    string reportTitle,
    string reportDescription,
    string originalQuery,
    List<SqlExecutionResult> executionResults,
    List<SqlQueryInfo> sqlQueries,
    DatabaseSchemaInfo schemaInfo,
    CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("开始生成 HTML 报表");

            // 1️⃣ AI 分析（你已有逻辑）
            var queryIntent = await AnalyzeQueryIntentWithAI(originalQuery, cancellationToken);
            var dataCharacteristics = await AnalyzeDataCharacteristicsWithAI(executionResults, cancellationToken);

            // 2️⃣ 构建上下文
            var reportContext = BuildReportContext(
                reportTitle,
                reportDescription,
                originalQuery,
                executionResults,
                sqlQueries,
                schemaInfo);

            // 3️⃣ System Prompt（关键：强约束）
            var systemPrompt = """
你是一个专业的 HTML 报表生成专家。

你的目标：
根据输入数据生成一个【完整、可运行、结构正确】的 HTML 报表。

=====================
❗ 强制规则（必须遵守）
=====================

1. 只允许输出 HTML，不允许任何解释、说明、markdown
2. 必须包含完整 HTML 结构：
   <!DOCTYPE html>
   <html>
   <head>
   <body>

3. 只能使用以下技术栈：
   - Tailwind CSS
   - ECharts（唯一图表库）

4. 禁止：
   - 使用多个图表库
   - 使用 Three.js / 复杂3D
   - 使用 Vue / React
   - 编造数据

5. 所有图表必须：
   - 使用真实数据（来自输入）
   - 可直接运行（不能伪代码）

6. 必须保证：
   - 页面打开即可运行
   - JS 无错误
   - CDN 正确

=====================
📊 报表生成策略
=====================

根据数据自动选择：

- 有时间字段 → 折线图
- 分类统计 → 柱状图 / 饼图
- 排名数据 → Top 列表
- 明细数据 → 表格

=====================
📦 必须包含 CDN
=====================

Tailwind:
<script src="https://cdn.tailwindcss.com"></script>

ECharts:
<script src="https://cdn.jsdelivr.net/npm/echarts@5.4.3/dist/echarts.min.js"></script>

=====================
📤 输出要求
=====================

只返回 HTML 文档本身，不要 ```html，不要解释
""";

            // 4️⃣ 用户 Prompt
            var userPrompt = $"""
## 报表信息

标题: {reportTitle}
描述: {reportDescription}
用户查询: {originalQuery}

## 查询意图
{queryIntent}

## 数据特点
{dataCharacteristics}

## SQL 查询
{string.Join("\n", sqlQueries.Select(q => $"{q.QueryType}: {q.Description}"))}

## 数据结果
{reportContext}

请生成 HTML 报表
""";

            // 5️⃣ 调用 LLM
            var response = await coderClient.Client.GetResponseAsync(
                messages: new[]
                {
                new ChatMessage(ChatRole.System, systemPrompt),
                new ChatMessage(ChatRole.User, userPrompt)
                },
                options: new ChatOptions
                {
                    Temperature = 0.3f,
                    MaxOutputTokens = 8192
                },
                cancellationToken: cancellationToken);

            var html = response.Text?.Trim();

            if (string.IsNullOrWhiteSpace(html))
                throw new InvalidOperationException("AI 未返回 HTML");

            logger.LogInformation("原始 HTML 长度: {Length}", html.Length);

            // 6️⃣ 清洗 HTML（🔥必须）
            html = CleanHtml(html);

            // 7️⃣ 基础校验（建议）
            //ValidateHtml(html);

            logger.LogInformation("HTML 报表生成成功");

            return html;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "生成 HTML 报表失败");
            throw;
        }
    }

    private async Task<string> AnalyzeQueryIntentWithAI(
    string query,
    CancellationToken cancellationToken)
    {
        var systemPrompt = """
你是一个查询意图分析专家。

你的任务：
分析用户查询的业务意图，用于后续生成数据报表。

=====================
输出要求（必须遵守）
=====================

1. 只输出分析结果，不要任何开场白
2. 不要出现“好的”、“以下是”等废话
3. 内容必须简洁、可读

=====================
分析内容（只保留核心）
=====================

## 查询类型
（分析类 / 统计类 / 趋势类 / 对比类 / 列表类 / 排名类）

## 用户意图
（一句话说明用户真正想要什么）

## 关键指标
- 指标1
- 指标2

## 数据特征
（是否涉及时间 / 分类 / 排名）

=====================
限制（非常重要）
=====================

❌ 不要输出：
- 动画建议
- 3D 可视化
- 前端实现
- 技术方案

只关注“数据和业务”
""";

        var userPrompt = $"""
用户查询:
{query}

请分析其查询意图
""";

        var response = await chatClient.GetResponseAsync(
            messages: new[]
            { 
                new ChatMessage(ChatRole.System, systemPrompt),
                new ChatMessage(ChatRole.User, userPrompt)
            },
            options: new ChatOptions
            {
                Temperature = 0.2f,
                MaxOutputTokens = 1024
            },
            cancellationToken: cancellationToken);

        var result = response.Text?.Trim();

        if (string.IsNullOrWhiteSpace(result))
            return "未识别到明确查询意图";

        return result;
    }
    private async Task<string> AnalyzeDataCharacteristicsWithAI(
    List<SqlExecutionResult> executionResults,
    CancellationToken cancellationToken)
    {
        var dataSummary = BuildDataSummaryForAI(executionResults);

        var systemPrompt = """
你是一个数据分析专家。

你的任务：
分析 SQL 查询结果的数据特征，用于后续报表生成。

=====================
输出要求（必须遵守）
=====================

1. 只输出分析结果
2. 不要解释，不要开场白
3. 内容必须简洁、稳定

=====================
分析内容（核心）
=====================

## 数据规模
- 查询数量: [数量]
- 总行数: [数量]
- 数据规模: [小/中/大]

## 字段类型
- 数值字段: [列名...]
- 分类字段: [列名...]
- 时间字段: [列名...]

## 数据结构
- 是否包含时间序列: [是/否]
- 是否包含分类维度: [是/否]
- 是否适合聚合: [是/否]

## 数据特点
- 是否存在明显分组: [是/否]
- 是否适合趋势分析: [是/否]
- 是否适合排名分析: [是/否]

=====================
限制（非常重要）
=====================

❌ 不要输出：
- 图表类型
- 动画
- 3D
- 前端实现

只分析“数据本身”
""";

        var userPrompt = $"""
数据摘要:
{dataSummary}

请分析数据特征
""";

        var response = await chatClient.GetResponseAsync(
            messages:
            [
                new ChatMessage(ChatRole.System, systemPrompt),
                new ChatMessage(ChatRole.User, userPrompt)
            ],
            options: new ChatOptions
            {
                Temperature = 0.2f,
                MaxOutputTokens = 1024
            },
            cancellationToken: cancellationToken);

        var result = response.Text?.Trim();

        if (string.IsNullOrWhiteSpace(result))
            return "未识别到数据特征";

        return result;
    }
    
    private string BuildDataSummaryForAI(List<SqlExecutionResult> executionResults)
    {
        var sb = new StringBuilder();

        sb.AppendLine("## 查询概览:");
        sb.AppendLine($"总查询数: {executionResults.Count}");
        sb.AppendLine();

        foreach (var result in executionResults)
        {
            sb.AppendLine($"### {result.QueryType}: {result.Description}");
            sb.AppendLine($"- 优先级: {result.Priority}");
            sb.AppendLine($"- 总行数: {result.RowCount}");
            sb.AppendLine($"- 总列数: {result.ColumnCount}");
            sb.AppendLine($"- 列名: {string.Join(", ", result.Columns)}");
            sb.AppendLine();

            sb.AppendLine("#### 列详情:");
            foreach (var column in result.Columns)
            {
                var values = result.Data
                    .Where(row => row.ContainsKey(column) && row[column] != "NULL")
                    .Select(row => row[column])
                    .ToList();

                if (!values.Any()) continue;

                sb.AppendLine($"##### {column}:");
                sb.AppendLine($"- 总数: {values.Count}");
                sb.AppendLine($"- NULL 数: {result.Data.Count - values.Count}");
                sb.AppendLine($"- 唯一值: {values.Distinct().Count()}");
                sb.AppendLine($"- 唯一值比例: {(double)values.Distinct().Count() / values.Count:P1}");

                if (values.Any(v => decimal.TryParse(v, out _)))
                {
                    var numericValues = values.Select(v => decimal.Parse(v)).ToList();
                    sb.AppendLine($"- 数据类型: 数值型");
                    sb.AppendLine($"- 最小值: {numericValues.Min()}");
                    sb.AppendLine($"- 最大值: {numericValues.Max()}");
                    sb.AppendLine($"- 平均值: {numericValues.Average():F2}");
                    sb.AppendLine($"- 中位数: {numericValues.OrderBy(x => x).ElementAt(numericValues.Count / 2):F2}");
                    sb.AppendLine($"- 适合 3D 可视化: 是（3D 柱状图、3D 散点图）");
                }
                else if (values.All(v => DateTime.TryParse(v, out _) || DateTimeOffset.TryParse(v, out _)))
                {
                    sb.AppendLine($"- 数据类型: 日期时间型");
                    var dates = values.Select(v => DateTime.Parse(v)).ToList();
                    sb.AppendLine($"- 最早时间: {dates.Min()}");
                    sb.AppendLine($"- 最晚时间: {dates.Max()}");
                    sb.AppendLine($"- 时间跨度: {(dates.Max() - dates.Min()).Days} 天");
                    sb.AppendLine($"- 适合 3D 可视化: 是（3D 折线图、3D 曲面图）");
                }
                else if (values.All(v => v.Equals("true", StringComparison.OrdinalIgnoreCase) || v.Equals("false", StringComparison.OrdinalIgnoreCase)))
                {
                    sb.AppendLine($"- 数据类型: 布尔型");
                    var trueCount = values.Count(v => v.Equals("true", StringComparison.OrdinalIgnoreCase));
                    sb.AppendLine($"- true 数量: {trueCount}");
                    sb.AppendLine($"- false 数量: {values.Count - trueCount}");
                    sb.AppendLine($"- true 比例: {(double)trueCount / values.Count:P1}");
                    sb.AppendLine($"- 适合 3D 可视化: 是（3D 饼图）");
                }
                else
                {
                    sb.AppendLine($"- 数据类型: 文本型");
                    var avgLength = values.Average(v => v.Length);
                    sb.AppendLine($"- 平均长度: {avgLength:F1}");
                    sb.AppendLine($"- 最长: {values.Max(v => v.Length)}");
                    sb.AppendLine($"- 最短: {values.Min(v => v.Length)}");
                    sb.AppendLine($"- 适合 3D 可视化: 否（适合文本标签云）");
                }

                sb.AppendLine();
            }

            sb.AppendLine("#### 数据样本 (前5行):");
            for (int i = 0; i < Math.Min(5, result.Data.Count); i++)
            {
                var row = result.Data[i];
                sb.AppendLine($"行 {i + 1}:");
                foreach (var column in result.Columns)
                {
                    var value = row.ContainsKey(column) ? row[column] : "NULL";
                    sb.AppendLine($"  {column}: {value}");
                }
                sb.AppendLine();
            }
        }

        return sb.ToString();
    }

    private string BuildReportContext(
        string reportTitle,
        string reportDescription,
        string originalQuery,
        List<SqlExecutionResult> executionResults,
        List<SqlQueryInfo> sqlQueries,
        DatabaseSchemaInfo schemaInfo)
    {
        var sb = new StringBuilder();

        sb.AppendLine("## SQL 查询列表:");
        foreach (var query in sqlQueries.OrderBy(q => q.Priority))
        {
            sb.AppendLine($"### {query.QueryType}: {query.Description}");
            sb.AppendLine($"- 优先级: {query.Priority}");
            sb.AppendLine($"- SQL: {query.Sql}");
            sb.AppendLine();
        }

        sb.AppendLine("## 完整数据:");
        foreach (var result in executionResults.OrderBy(r => r.Priority))
        {
            sb.AppendLine($"### {result.QueryType}: {result.Description}");
            sb.AppendLine($"- 总行数: {result.RowCount}");
            sb.AppendLine($"- 列名: {string.Join(", ", result.Columns)}");
            sb.AppendLine();

            sb.AppendLine("#### 数据样本 (前10行):");
            for (int i = 0; i < Math.Min(10, result.Data.Count); i++)
            {
                var row = result.Data[i];
                sb.AppendLine($"行 {i + 1}:");
                foreach (var column in result.Columns)
                {
                    var value = row.ContainsKey(column) ? row[column] : "NULL";
                    sb.AppendLine($"  {column}: {value}");
                }
                sb.AppendLine();
            }
        }

        sb.AppendLine("## 涉及的数据库表:");
        foreach (var table in schemaInfo.Tables)
        {
            sb.AppendLine($"- {table.TableName} ({table.EntityName})");
            if (!string.IsNullOrEmpty(table.Comment))
            {
                sb.AppendLine($"  说明: {table.Comment}");
            }
        }

        return sb.ToString();
    }

    private string CleanHtml(string html)
    {
        if (string.IsNullOrWhiteSpace(html))
            return string.Empty;

        html = html.Trim();

        // 去掉 ```html
        if (html.StartsWith("```"))
        {
            var start = html.IndexOf("<!DOCTYPE");
            if (start >= 0)
            {
                html = html.Substring(start);
            }

            html = html.Trim('`');
        }

        return html;
    }
}
