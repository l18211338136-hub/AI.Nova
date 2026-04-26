using System.Text;
using AI.Nova.Server.Api.Infrastructure.Data;
using AI.Nova.Server.Api.Infrastructure.Data.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AI.Nova.Server.Api.Features.Knowledge;

public class KnowledgeBaseSeeder : IDataSeeder
{
    private const string KnowledgeBaseName = "系统文档";
    private const string DocumentTitle = "数据库架构";

    public async Task SeedAsync(AppDbContext dbContext, CancellationToken cancellationToken)
    {
        var kb = await dbContext.KnowledgeBases
            .FirstOrDefaultAsync(k => k.Name == KnowledgeBaseName, cancellationToken);

        if (kb == null)
        {
            kb = new KnowledgeBase
            {
                Id = Guid.NewGuid(),
                Name = KnowledgeBaseName,
                Description = "包含项目的元数据信息、数据库架构和系统文档。"
            };
            await dbContext.KnowledgeBases.AddAsync(kb, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        var doc = await dbContext.KnowledgeDocuments
            .FirstOrDefaultAsync(d => d.Title == DocumentTitle && d.KnowledgeBaseId == kb.Id, cancellationToken);

        if (doc == null)
        {
            doc = new KnowledgeDocument
            {
                Id = Guid.NewGuid(),
                Title = DocumentTitle,
                KnowledgeBaseId = kb.Id,
                ContentType = "text/markdown"
            };
            await dbContext.KnowledgeDocuments.AddAsync(doc, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        else
        {
            // Clear existing chunks to refresh the data with the new format
            var existingChunks = await dbContext.KnowledgeDocumentChunks
                .Where(c => c.DocumentId == doc.Id)
                .ToListAsync(cancellationToken);
            dbContext.KnowledgeDocumentChunks.RemoveRange(existingChunks);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        var model = dbContext.GetService<IDesignTimeModel>().Model;
        var entityTypes = model.GetEntityTypes().OrderBy(e => e.Name);
        var index = 0;

        foreach (var entity in entityTypes)
        {
            var tableName = entity.GetTableName() ?? entity.Name;
            var tableComment = entity.GetComment();
            
            if (tableName.StartsWith("Hangfire", StringComparison.OrdinalIgnoreCase)||
                tableName.StartsWith("Knowledge", StringComparison.OrdinalIgnoreCase)) 
                continue;

            var sb = new StringBuilder();
            sb.AppendLine("<table border=\"1\" width=\"100%\" style=\"border-collapse: collapse;\">");
            sb.AppendLine("  <thead>");
            sb.AppendLine("    <tr style=\"background-color: #f8f9fa;\">");
            sb.AppendLine("      <th colspan=\"8\" align=\"left\" style=\"padding: 10px;\">");
            sb.AppendLine($"        <strong>数据表: {tableName}</strong><br/>");
            if (!string.IsNullOrEmpty(tableComment))
            {
                sb.AppendLine($"        <span style=\"font-weight: normal; color: #666;\">说明: {tableComment}</span>");
            }
            sb.AppendLine("      </th>");
            sb.AppendLine("    </tr>");
            sb.AppendLine("    <tr style=\"background-color: #f2f2f2;\">");
            sb.AppendLine("      <th align=\"left\">列名</th>");
            sb.AppendLine("      <th align=\"left\">类型</th>");
            sb.AppendLine("      <th align=\"left\">长度</th>");
            sb.AppendLine("      <th align=\"left\">必填</th>");
            sb.AppendLine("      <th align=\"left\">主键</th>");
            sb.AppendLine("      <th align=\"left\">关联表</th>");
            sb.AppendLine("      <th align=\"left\">(推断)</th>");
            sb.AppendLine("      <th align=\"left\">注释</th>");
            sb.AppendLine("    </tr>");
            sb.AppendLine("  </thead>");
            sb.AppendLine("  <tbody>");

            foreach (var property in entity.GetProperties())
            {
                var name = property.Name;
                var type = property.GetColumnType() ?? property.ClrType.Name;
                var maxLength = property.GetMaxLength()?.ToString() ?? "-";
                var isRequired = property.IsNullable ? "否" : "是";
                var isPk = property.IsPrimaryKey() ? "是" : "-";
                var comment = (property.GetComment() ?? "-").Replace("\n", " ");
                
                var inferredTableName = "-";
                var isInferred = "-";
                if (!property.IsPrimaryKey() && name.EndsWith("Id", StringComparison.OrdinalIgnoreCase))
                {
                    var potentialTable = name[..^2];
                    if (entityTypes.Any(e => e.ClrType.Name == potentialTable || e.GetTableName() == potentialTable))
                    {
                        inferredTableName = potentialTable;
                        isInferred = "是";
                    }
                }

                sb.AppendLine("    <tr>");
                sb.AppendLine($"      <td>{name}</td>");
                sb.AppendLine($"      <td>{type}</td>");
                sb.AppendLine($"      <td>{maxLength}</td>");
                sb.AppendLine($"      <td>{isRequired}</td>");
                sb.AppendLine($"      <td>{isPk}</td>");
                sb.AppendLine($"      <td>{inferredTableName}</td>");
                sb.AppendLine($"      <td>{isInferred}</td>");
                sb.AppendLine($"      <td>{comment}</td>");
                sb.AppendLine("    </tr>");
            }

            sb.AppendLine("  </tbody>");
            sb.AppendLine("</table>");

            var chunk = new KnowledgeDocumentChunk
            {
                Id = Guid.NewGuid(),
                DocumentId = doc.Id,
                Content = sb.ToString(),
                Index = index++,
                TokenCount = sb.Length / 4 
            };

            await dbContext.KnowledgeDocumentChunks.AddAsync(chunk, cancellationToken);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
