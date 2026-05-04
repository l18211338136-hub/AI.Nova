using System.Text;
using AI.Nova.Server.Api.Infrastructure.Data.Seed;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AI.Nova.Server.Api.Features.Knowledge;

public class KnowledgeBaseSeeder(KnowledgeEmbeddingService embeddingService) : IDataSeeder
{
    private const string KnowledgeBaseName = "系统文档";

    public async Task SeedAsync(AppDbContext dbContext, CancellationToken cancellationToken)
    {
        var kb = await dbContext.KnowledgeBases
            .FirstOrDefaultAsync(k => k.Name == KnowledgeBaseName, cancellationToken);

        if (kb == null)
        {
            kb = new KnowledgeBase
            {
                Id = Guid.CreateVersion7(),
                Name = KnowledgeBaseName,
                Description = "包含项目的元数据信息、数据库架构、表关系和常用查询示例，用于 Text-to-SQL 报表生成。"
            };
            await dbContext.KnowledgeBases.AddAsync(kb, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        var model = dbContext.GetService<IDesignTimeModel>().Model;
        var entityTypes = model.GetEntityTypes()
            .Where(e =>
            {
                var tableName = e.GetTableName();
                return tableName != null &&
                       !tableName.StartsWith("Hangfire", StringComparison.OrdinalIgnoreCase) &&
                       !tableName.StartsWith("Knowledge", StringComparison.OrdinalIgnoreCase) &&
                       !tableName.StartsWith("__", StringComparison.OrdinalIgnoreCase);
            })
            .OrderBy(e => e.Name)
            .ToList();

        await SeedDatabaseSchemaDocument(dbContext, kb, entityTypes, model, cancellationToken);
        await SeedTableRelationshipsDocument(dbContext, kb, entityTypes, model, cancellationToken);
        await SeedCommonQueriesDocument(dbContext, kb, entityTypes, cancellationToken);
        await SeedDataTypesDocument(dbContext, kb, entityTypes, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task SeedDatabaseSchemaDocument(
        AppDbContext dbContext,
        KnowledgeBase kb,
        IReadOnlyList<IEntityType> entityTypes,
        IModel model,
        CancellationToken cancellationToken)
    {
        const string documentTitle = "数据库架构";

        var doc = await dbContext.KnowledgeDocuments
            .FirstOrDefaultAsync(d => d.Title == documentTitle && d.KnowledgeBaseId == kb.Id, cancellationToken);

        if (doc == null)
        {
            doc = new KnowledgeDocument
            {
                Id = Guid.CreateVersion7(),
                Title = documentTitle,
                KnowledgeBaseId = kb.Id,
                ContentType = "text/markdown"
            };
            await dbContext.KnowledgeDocuments.AddAsync(doc, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        else
        {
            var existingChunks = await dbContext.KnowledgeDocumentChunks
                .Where(c => c.DocumentId == doc.Id)
                .ToListAsync(cancellationToken);
            dbContext.KnowledgeDocumentChunks.RemoveRange(existingChunks);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        var chunks = new List<KnowledgeDocumentChunk>();
        var index = 0;

        foreach (var entity in entityTypes)
        {
            var tableName = entity.GetTableName() ?? entity.Name;
            var tableComment = entity.GetComment();

            var chunkContent = BuildTableSchemaChunk(entity, tableName, tableComment, entityTypes);

            var chunk = new KnowledgeDocumentChunk
            {
                Id = Guid.CreateVersion7(),
                DocumentId = doc.Id,
                RawContent = chunkContent,
                Content = BuildTableSearchContent(tableName, tableComment, entity),
                Index = index++,
                TokenCount = chunkContent.Length / 4
            };

            await embeddingService.Embed(chunk, cancellationToken);
            chunks.Add(chunk);
        }

        if (chunks.Any())
        {
            await dbContext.KnowledgeDocumentChunks.AddRangeAsync(chunks, cancellationToken);
        }
    }

    private async Task SeedTableRelationshipsDocument(
        AppDbContext dbContext,
        KnowledgeBase kb,
        IReadOnlyList<IEntityType> entityTypes,
        IModel model,
        CancellationToken cancellationToken)
    {
        const string documentTitle = "表关系";

        var doc = await dbContext.KnowledgeDocuments
            .FirstOrDefaultAsync(d => d.Title == documentTitle && d.KnowledgeBaseId == kb.Id, cancellationToken);

        if (doc == null)
        {
            doc = new KnowledgeDocument
            {
                Id = Guid.CreateVersion7(),
                Title = documentTitle,
                KnowledgeBaseId = kb.Id,
                ContentType = "text/markdown"
            };
            await dbContext.KnowledgeDocuments.AddAsync(doc, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        else
        {
            var existingChunks = await dbContext.KnowledgeDocumentChunks
                .Where(c => c.DocumentId == doc.Id)
                .ToListAsync(cancellationToken);
            dbContext.KnowledgeDocumentChunks.RemoveRange(existingChunks);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        var chunks = new List<KnowledgeDocumentChunk>();
        var index = 0;

        foreach (var entity in entityTypes)
        {
            var tableName = entity.GetTableName() ?? entity.Name;
            var relationships = BuildRelationshipsChunk(entity, tableName, entityTypes);

            if (string.IsNullOrWhiteSpace(relationships))
                continue;

            var chunk = new KnowledgeDocumentChunk
            {
                Id = Guid.CreateVersion7(),
                DocumentId = doc.Id,
                RawContent = relationships,
                Content = BuildRelationshipSearchContent(tableName, entity, entityTypes),
                Index = index++,
                TokenCount = relationships.Length / 4
            };

            await embeddingService.Embed(chunk, cancellationToken);
            chunks.Add(chunk);
        }

        if (chunks.Any())
        {
            await dbContext.KnowledgeDocumentChunks.AddRangeAsync(chunks, cancellationToken);
        }
    }

    private async Task SeedCommonQueriesDocument(
        AppDbContext dbContext,
        KnowledgeBase kb,
        IReadOnlyList<IEntityType> entityTypes,
        CancellationToken cancellationToken)
    {
        const string documentTitle = "常用查询示例";

        var doc = await dbContext.KnowledgeDocuments
            .FirstOrDefaultAsync(d => d.Title == documentTitle && d.KnowledgeBaseId == kb.Id, cancellationToken);

        if (doc == null)
        {
            doc = new KnowledgeDocument
            {
                Id = Guid.CreateVersion7(),
                Title = documentTitle,
                KnowledgeBaseId = kb.Id,
                ContentType = "text/markdown"
            };
            await dbContext.KnowledgeDocuments.AddAsync(doc, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        else
        {
            var existingChunks = await dbContext.KnowledgeDocumentChunks
                .Where(c => c.DocumentId == doc.Id)
                .ToListAsync(cancellationToken);
            dbContext.KnowledgeDocumentChunks.RemoveRange(existingChunks);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        var chunks = new List<KnowledgeDocumentChunk>();
        var index = 0;

        var commonQueries = GenerateCommonQueries(entityTypes);

        foreach (var query in commonQueries)
        {
            var chunk = new KnowledgeDocumentChunk
            {
                Id = Guid.CreateVersion7(),
                DocumentId = doc.Id,
                RawContent = query.Sql,
                Content = BuildQuerySearchContent(query),
                Index = index++,
                TokenCount = (query.Sql.Length + query.Description.Length) / 4
            };

            await embeddingService.Embed(chunk, cancellationToken);
            chunks.Add(chunk);
        }

        if (chunks.Any())
        {
            await dbContext.KnowledgeDocumentChunks.AddRangeAsync(chunks, cancellationToken);
        }
    }

    private async Task SeedDataTypesDocument(
        AppDbContext dbContext,
        KnowledgeBase kb,
        IReadOnlyList<IEntityType> entityTypes,
        CancellationToken cancellationToken)
    {
        const string documentTitle = "数据类型参考";

        var doc = await dbContext.KnowledgeDocuments
            .FirstOrDefaultAsync(d => d.Title == documentTitle && d.KnowledgeBaseId == kb.Id, cancellationToken);

        if (doc == null)
        {
            doc = new KnowledgeDocument
            {
                Id = Guid.CreateVersion7(),
                Title = documentTitle,
                KnowledgeBaseId = kb.Id,
                ContentType = "text/markdown"
            };
            await dbContext.KnowledgeDocuments.AddAsync(doc, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        else
        {
            var existingChunks = await dbContext.KnowledgeDocumentChunks
                .Where(c => c.DocumentId == doc.Id)
                .ToListAsync(cancellationToken);
            dbContext.KnowledgeDocumentChunks.RemoveRange(existingChunks);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        var chunks = new List<KnowledgeDocumentChunk>();
        var index = 0;

        var dataTypeInfo = BuildDataTypesChunk(entityTypes);

        foreach (var typeInfo in dataTypeInfo)
        {
            var chunk = new KnowledgeDocumentChunk
            {
                Id = Guid.CreateVersion7(),
                DocumentId = doc.Id,
                RawContent = typeInfo.Content,
                Content = BuildDataTypeSearchContent(typeInfo),
                Index = index++,
                TokenCount = typeInfo.Content.Length / 4
            };

            await embeddingService.Embed(chunk, cancellationToken);
            chunks.Add(chunk);
        }

        if (chunks.Any())
        {
            await dbContext.KnowledgeDocumentChunks.AddRangeAsync(chunks, cancellationToken);
        }
    }

    private string BuildTableSchemaChunk(IEntityType entity, string tableName, string? tableComment, IReadOnlyList<IEntityType> allEntityTypes)
    {
        var sb = new StringBuilder();

        sb.AppendLine("## 表信息");
        sb.AppendLine($"表名: {tableName}");
        sb.AppendLine($"实体类: {entity.ClrType.Name}");

        if (!string.IsNullOrEmpty(tableComment))
        {
            sb.AppendLine($"说明: {tableComment}");
        }

        sb.AppendLine();

        sb.AppendLine("## 列信息");
        sb.AppendLine("| 列名 | 数据库类型 | CLR 类型 | 长度 | 可空 | 主键 | 外键 | 关联表 | 注释 |");
        sb.AppendLine("|------|----------|---------|------|------|------|------|--------|------|");

        foreach (var property in entity.GetProperties())
        {
            var name = property.Name;
            var columnType = property.GetColumnType() ?? property.ClrType.Name;
            var clrType = property.ClrType.Name;
            var maxLength = property.GetMaxLength()?.ToString() ?? "-";
            var isNullable = property.IsNullable ? "是" : "否";
            var isPk = property.IsPrimaryKey() ? "是" : "-";

            var foreignKeyInfo = GetForeignKeyInfo(property, allEntityTypes);
            var isFk = foreignKeyInfo.IsForeignKey ? "是" : "-";
            var relatedTable = foreignKeyInfo.RelatedTable ?? "-";

            var comment = (property.GetComment() ?? "-").Replace("\n", " ");

            sb.AppendLine($"| {name} | {columnType} | {clrType} | {maxLength} | {isNullable} | {isPk} | {isFk} | {relatedTable} | {comment} |");
        }

        sb.AppendLine();

        sb.AppendLine("## 索引信息");
        var indexes = entity.GetIndexes();
        if (indexes.Any())
        {
            sb.AppendLine("| 索引名 | 列 | 是否唯一 |");
            sb.AppendLine("|--------|-----|---------|");

            foreach (var index in indexes)
            {
                var columns = string.Join(", ", index.Properties.Select(p => p.Name));
                var isUnique = index.IsUnique ? "是" : "否";
                sb.AppendLine($"| {index.Name} | {columns} | {isUnique} |");
            }
        }
        else
        {
            sb.AppendLine("无索引");
        }

        return sb.ToString();
    }

    private string BuildTableSearchContent(string tableName, string? tableComment, IEntityType entity)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"表名: {tableName}");

        if (!string.IsNullOrEmpty(tableComment))
        {
            sb.AppendLine($"说明: {tableComment}");
        }

        sb.AppendLine($"实体类: {entity.ClrType.Name}");

        var mainColumns = entity.GetProperties()
            .Where(p => !p.IsShadowProperty())
            .Take(5)
            .Select(p => p.Name);

        sb.AppendLine($"主要列: {string.Join(", ", mainColumns)}");

        var foreignKeys = entity.GetProperties()
            .Where(p => p.Name.EndsWith("Id", StringComparison.OrdinalIgnoreCase) && !p.IsPrimaryKey())
            .Select(p => p.Name[..^2]);

        if (foreignKeys.Any())
        {
            sb.AppendLine($"关联表: {string.Join(", ", foreignKeys)}");
        }

        return sb.ToString();
    }

    private string BuildRelationshipsChunk(IEntityType entity, string tableName, IReadOnlyList<IEntityType> allEntityTypes)
    {
        var sb = new StringBuilder();

        sb.AppendLine($"## 表: {tableName}");
        sb.AppendLine();

        var foreignKeys = entity.GetForeignKeys();

        if (foreignKeys.Any())
        {
            sb.AppendLine("### 外键关系 (引用其他表):");
            sb.AppendLine("| 外键列 | 引用表 | 引用列 | 级联删除 |");
            sb.AppendLine("|--------|--------|--------|---------|");

            foreach (var fk in foreignKeys)
            {
                var fkColumn = fk.Properties.FirstOrDefault()?.Name ?? "-";
                var principalTable = fk.PrincipalEntityType.GetTableName() ?? fk.PrincipalEntityType.Name;
                var pkColumn = fk.PrincipalKey.Properties.FirstOrDefault()?.Name ?? "-";
                var deleteBehavior = fk.DeleteBehavior.ToString();

                sb.AppendLine($"| {fkColumn} | {principalTable} | {pkColumn} | {deleteBehavior} |");
            }
        }
        else
        {
            sb.AppendLine("### 外键关系: 无");
        }

        sb.AppendLine();

        var referencedBy = allEntityTypes
            .Where(e => e.GetForeignKeys().Any(fk => fk.PrincipalEntityType == entity))
            .ToList();

        if (referencedBy.Any())
        {
            sb.AppendLine("### 被引用关系 (其他表引用此表):");
            sb.AppendLine("| 引用表 | 外键列 |");
            sb.AppendLine("|--------|--------|");

            foreach (var referencingEntity in referencedBy)
            {
                var referencingTableName = referencingEntity.GetTableName() ?? referencingEntity.Name;
                var fk = referencingEntity.GetForeignKeys()
                    .FirstOrDefault(fk => fk.PrincipalEntityType == entity);
                var fkColumn = fk?.Properties.FirstOrDefault()?.Name ?? "-";

                sb.AppendLine($"| {referencingTableName} | {fkColumn} |");
            }
        }
        else
        {
            sb.AppendLine("### 被引用关系: 无");
        }

        return sb.ToString();
    }

    private string BuildRelationshipSearchContent(string tableName, IEntityType entity, IReadOnlyList<IEntityType> allEntityTypes)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"表名: {tableName}");

        var foreignKeys = entity.GetForeignKeys();
        if (foreignKeys.Any())
        {
            var referencedTables = foreignKeys
                .Select(fk => fk.PrincipalEntityType.GetTableName() ?? fk.PrincipalEntityType.Name)
                .Distinct();
            sb.AppendLine($"引用表: {string.Join(", ", referencedTables)}");
        }

        var referencedBy = allEntityTypes
            .Where(e => e.GetForeignKeys().Any(fk => fk.PrincipalEntityType == entity))
            .Select(e => e.GetTableName() ?? e.Name);

        if (referencedBy.Any())
        {
            sb.AppendLine($"被引用表: {string.Join(", ", referencedBy)}");
        }

        return sb.ToString();
    }

    private List<CommonQuery> GenerateCommonQueries(IReadOnlyList<IEntityType> entityTypes)
    {
        var queries = new List<CommonQuery>();

        foreach (var entity in entityTypes)
        {
            var tableName = entity.GetTableName() ?? entity.Name;
            var entityName = entity.ClrType.Name;

            var properties = entity.GetProperties()
                .Where(p => !p.IsShadowProperty())
                .ToList();

            var mainColumns = properties.Take(3).Select(p => p.Name);
            var numericColumns = properties
                .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(int) || p.ClrType == typeof(double))
                .Select(p => p.Name);
            var nameColumns = properties
                .Where(p => p.Name.Contains("Name", StringComparison.OrdinalIgnoreCase))
                .Select(p => p.Name);
            var dateColumns = properties
                .Where(p => p.ClrType == typeof(DateTime) || p.ClrType == typeof(DateTimeOffset))
                .Select(p => p.Name);

            if (mainColumns.Any())
            {
                queries.Add(new CommonQuery
                {
                    Description = $"查询 {tableName} 表的所有数据",
                    Sql = $"SELECT TOP 100 {string.Join(", ", mainColumns)} FROM {tableName}",
                    Keywords = [tableName, "查询", "所有", "全部"]
                });
            }

            if (numericColumns.Any())
            {
                var numericColumn = numericColumns.First();
                queries.Add(new CommonQuery
                {
                    Description = $"按 {numericColumn} 排序查询 {tableName}",
                    Sql = $"SELECT TOP 100 {string.Join(", ", mainColumns)} FROM {tableName} ORDER BY {numericColumn} DESC",
                    Keywords = [tableName, "排序", numericColumn, "最大", "最小"]
                });
            }

            if (nameColumns.Any())
            {
                var nameColumn = nameColumns.First();
                queries.Add(new CommonQuery
                {
                    Description = $"按 {nameColumn} 搜索 {tableName}",
                    Sql = $"SELECT TOP 100 {string.Join(", ", mainColumns)} FROM {tableName} WHERE {nameColumn} LIKE @searchPattern ORDER BY {nameColumn}",
                    Keywords = [tableName, "搜索", nameColumn, "查找"]
                });
            }

            if (dateColumns.Any())
            {
                var dateColumn = dateColumns.First();
                queries.Add(new CommonQuery
                {
                    Description = $"按 {dateColumn} 范围查询 {tableName}",
                    Sql = $"SELECT TOP 100 {string.Join(", ", mainColumns)} FROM {tableName} WHERE {dateColumn} >= @startDate AND {dateColumn} <= @endDate ORDER BY {dateColumn} DESC",
                    Keywords = [tableName, "日期", "范围", "时间", "期间"]
                });
            }

            var foreignKeys = entity.GetForeignKeys();
            foreach (var fk in foreignKeys)
            {
                var fkColumn = fk.Properties.FirstOrDefault()?.Name;
                var principalTable = fk.PrincipalEntityType.GetTableName() ?? fk.PrincipalEntityType.Name;

                if (!string.IsNullOrEmpty(fkColumn) && !string.IsNullOrEmpty(principalTable))
                {
                    queries.Add(new CommonQuery
                    {
                        Description = $"关联查询 {tableName} 和 {principalTable}",
                        Sql = $"SELECT TOP 100 t1.*, t2.* FROM {tableName} t1 JOIN {principalTable} t2 ON t1.{fkColumn} = t2.Id",
                        Keywords = [tableName, principalTable, "关联", "连接", "JOIN"]
                    });
                }
            }
        }

        return queries;
    }

    private string BuildQuerySearchContent(CommonQuery query)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"查询描述: {query.Description}");
        sb.AppendLine($"SQL: {query.Sql}");
        sb.AppendLine($"关键词: {string.Join(", ", query.Keywords)}");
        return sb.ToString();
    }

    private List<DataTypeInfo> BuildDataTypesChunk(IReadOnlyList<IEntityType> entityTypes)
    {
        var typeInfos = new List<DataTypeInfo>();
        var seenTypes = new HashSet<string>();

        foreach (var entity in entityTypes)
        {
            foreach (var property in entity.GetProperties())
            {
                var columnType = property.GetColumnType() ?? property.ClrType.Name;
                var clrType = property.ClrType.Name;

                if (!seenTypes.Add($"{columnType}|{clrType}"))
                    continue;

                var sb = new StringBuilder();
                sb.AppendLine($"## 数据类型: {columnType}");
                sb.AppendLine($"CLR 类型: {clrType}");
                sb.AppendLine();

                var examples = GetDataTypeExamples(clrType);
                sb.AppendLine("### 示例数据:");
                foreach (var example in examples)
                {
                    sb.AppendLine($"- {example}");
                }

                sb.AppendLine();

                var usage = GetDataTypeUsage(clrType);
                sb.AppendLine("### 常见用途:");
                sb.AppendLine(usage);

                sb.AppendLine();

                var notes = GetDataTypeNotes(clrType);
                sb.AppendLine("### 注意事项:");
                sb.AppendLine(notes);

                typeInfos.Add(new DataTypeInfo
                {
                    ColumnType = columnType,
                    ClrType = clrType,
                    Content = sb.ToString(),
                    Keywords = new[] { columnType, clrType }.Concat(examples.Take(2)).ToList(),
                });
            }
        }

        return typeInfos;
    }

    private string BuildDataTypeSearchContent(DataTypeInfo typeInfo)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"数据库类型: {typeInfo.ColumnType}");
        sb.AppendLine($"CLR 类型: {typeInfo.ClrType}");
        sb.AppendLine($"关键词: {string.Join(", ", typeInfo.Keywords)}");
        return sb.ToString();
    }

    private List<string> GetDataTypeExamples(string clrType)
    {
        return clrType.ToLower() switch
        {
            "string" => ["'示例文本'", "'Hello World'", "'产品名称'"],
            "int32" or "int64" => ["123", "1000", "-456"],
            "decimal" or "double" => ["123.45", "0.99", "-1000.50"],
            "datetime" or "datetimeoffset" => ["'2024-01-15'", "'2024-01-15 10:30:00'", "'2024-01-15T10:30:00Z'"],
            "boolean" or "bool" => ["true", "false"],
            "guid" => ["'00000000-0000-0000-0000-000000000000'", "Guid.CreateVersion7()"],
            _ => ["示例值"]
        };
    }

    private string GetDataTypeUsage(string clrType)
    {
        return clrType.ToLower() switch
        {
            "string" => "用于存储文本数据，如名称、描述、地址等",
            "int32" or "int64" => "用于存储整数，如数量、ID、计数等",
            "decimal" or "double" => "用于存储小数，如价格、金额、百分比等",
            "datetime" or "datetimeoffset" => "用于存储日期时间，如创建时间、更新时间、生日等",
            "boolean" or "bool" => "用于存储布尔值，如是否激活、是否删除等",
            "guid" => "用于存储唯一标识符，如主键ID、关联ID等",
            _ => "通用数据类型"
        };
    }

    private string GetDataTypeNotes(string clrType)
    {
        return clrType.ToLower() switch
        {
            "string" => "注意最大长度限制，使用 LIKE 进行模糊查询",
            "decimal" or "double" => "注意精度和舍入问题，使用 CAST 转换类型",
            "datetime" or "datetimeoffset" => "注意时区问题，使用 PostgreSQL 日期函数",
            "guid" => "使用 NEWID() 或 Guid.CreateVersion7() 生成新值",
            _ => "根据实际使用场景选择合适的数据类型"
        };
    }

    private (bool IsForeignKey, string? RelatedTable) GetForeignKeyInfo(IProperty property, IReadOnlyList<IEntityType> allEntityTypes)
    {
        if (property.IsPrimaryKey() || !property.Name.EndsWith("Id", StringComparison.OrdinalIgnoreCase))
            return (false, null);

        var potentialTable = property.Name[..^2];
        var hasMatchingEntity = allEntityTypes.Any(e =>
            e.ClrType.Name == potentialTable ||
            e.GetTableName() == potentialTable);

        return (hasMatchingEntity, hasMatchingEntity ? potentialTable : null);
    }

    private class CommonQuery
    {
        public string Description { get; set; } = string.Empty;
        public string Sql { get; set; } = string.Empty;
        public List<string> Keywords { get; set; } = [];
    }

    private class DataTypeInfo
    {
        public string ColumnType { get; set; } = string.Empty;
        public string ClrType { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public List<string> Keywords { get; set; } = [];
    }
}
