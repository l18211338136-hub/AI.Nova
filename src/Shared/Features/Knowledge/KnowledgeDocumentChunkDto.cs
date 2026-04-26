using System.ComponentModel.DataAnnotations;

namespace AI.Nova.Shared.Features.Knowledge;

[DtoResourceType(typeof(AppStrings))]
public partial class KnowledgeDocumentChunkDto
{
    public Guid Id { get; set; }

    [Required(ErrorMessageResourceType = typeof(AppStrings), ErrorMessageResourceName = nameof(AppStrings.RequiredAttribute_ValidationError))]
    public string? Content { get; set; }

    public int TokenCount { get; set; }

    public int Index { get; set; }
    
    public double Score { get; set; }

    public Guid DocumentId { get; set; }
    
    public string? DocumentTitle { get; set; }

    public long Version { get; set; }
}
