using AI.Nova.Server.Api.Infrastructure.Data.Audit;

namespace AI.Nova.Server.Api.Features.Knowledge;

public class KnowledgeBase : AuditEntity, IAuditableEntity, ISoftDelete
{
    public Guid Id { get; set; }
    
    public string? Name { get; set; }
    
    public string? Description { get; set; }

    public ICollection<KnowledgeDocument> Documents { get; set; } = [];
}
