using AI.Nova.Server.Api.Features.Products;
using AI.Nova.Server.Api.Infrastructure.Data.Audit;

namespace AI.Nova.Server.Api.Features.Categories;

[Table("Categories")]
[Comment("商品分类表：用于管理商品的类别和标签")]
public partial class Category : AuditEntity
{
    [Key]
    [Comment("主键ID：分类的唯一标识")]
    public Guid Id { get; set; }

    [MaxLength(64)]
    [Comment("分类名称：商品的类别名称，最大长度64字符")]
    public string? Name { get; set; }

    [Comment("颜色代码：用于前端展示分类标签的颜色，如 #FF5733")]
    public string? Color { get; set; }

    [Comment("版本号：用于乐观并发控制，每次更新自动递增")]
    public long Version { get; set; }

    public IList<Product> Products { get; set; } = [];
}
