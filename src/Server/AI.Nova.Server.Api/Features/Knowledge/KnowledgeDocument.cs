using AI.Nova.Server.Api.Infrastructure.Data.Audit;

namespace AI.Nova.Server.Api.Features.Knowledge;

public class KnowledgeDocument : AuditEntity, IAuditableEntity, ISoftDelete
{
    public Guid Id { get; set; }
    
    public string? Title { get; set; }

    public Guid? KnowledgeBaseId { get; set; }

    public KnowledgeBase? KnowledgeBase { get; set; } = default!;

    public byte[]? Content { get; set; }
    public string? ContentType { get; set; }

    public ICollection<KnowledgeDocumentChunk> Chunks { get; set; } = [];
}
