using System.ComponentModel.DataAnnotations;

namespace AI.Nova.Shared.Features.Knowledge;

[DtoResourceType(typeof(AppStrings))]
public partial class KnowledgeDocumentDto
{
    public Guid Id { get; set; }

    [Required(ErrorMessageResourceType = typeof(AppStrings), ErrorMessageResourceName = nameof(AppStrings.RequiredAttribute_ValidationError))]
    public string? Title { get; set; }

    public Guid KnowledgeBaseId { get; set; }

    public long Version { get; set; }
}
