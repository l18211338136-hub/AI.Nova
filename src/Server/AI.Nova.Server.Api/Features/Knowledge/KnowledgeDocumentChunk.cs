using System.ComponentModel.DataAnnotations.Schema;
using AI.Nova.Server.Api.Infrastructure.Data.Audit;
using Pgvector;

namespace AI.Nova.Server.Api.Features.Knowledge;

public class KnowledgeDocumentChunk : AuditEntity, IAuditableEntity, ISoftDelete
{
    public Guid Id { get; set; }
    
    public string? Content { get; set; }
    
    public int? TokenCount { get; set; }
    
    public int? Index { get; set; }
    
    [NotMapped]
    public double Score { get; set; }

    public Guid? DocumentId { get; set; }

    public KnowledgeDocument? Document { get; set; } = default!;

    public Vector? Embedding { get; set; }
}
