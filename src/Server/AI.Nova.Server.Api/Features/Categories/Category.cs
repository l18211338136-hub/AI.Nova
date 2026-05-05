using AI.Nova.Server.Api.Features.Products;
using AI.Nova.Server.Api.Infrastructure.Data.Audit;

namespace AI.Nova.Server.Api.Features.Categories;

[Table("Categories")]
[Comment("商品分类表")]
public partial class Category : AuditEntity
{
    [Key]
    [Comment("主键")]
    public Guid Id { get; set; }

    [MaxLength(64)]
    [Comment("名称")]
    public string? Name { get; set; }

    [Comment("颜色如 #FF5733")]
    public string? Color { get; set; }

    [Comment("版本号")]
    public long Version { get; set; }

    public IList<Product> Products { get; set; } = [];
}
