using System.ComponentModel.DataAnnotations;

namespace AI.Nova.Shared.Features.Knowledge;

[DtoResourceType(typeof(AppStrings))]
public partial class KnowledgeBaseDto
{
    public Guid Id { get; set; }

    [Required(ErrorMessageResourceType = typeof(AppStrings), ErrorMessageResourceName = nameof(AppStrings.RequiredAttribute_ValidationError))]
    public string? Name { get; set; }

    public string? Description { get; set; }

    public long Version { get; set; }
}
