using AI.Nova.Server.Api.Infrastructure.Data.Audit;

namespace AI.Nova.Server.Api.Features.Products;

/// <summary>
/// 商品图片关联表：存储商品的多媒体展示资源，支持多图展示、主图标记及排序展示。
/// </summary>
[Table("ProductImages")]
[Comment("商品图片表")]
public partial class ProductImage : AuditEntity
{
    /// <summary>
    /// 图片记录唯一标识。
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Comment("主键")]
    public Guid Id { get; set; }

    /// <summary>
    /// 导航属性：关联的商品实体。
    /// </summary>
    [ForeignKey(nameof(ProductId))]
    [Comment("关联的商品对象")]
    public Product? Product { get; set; }

    /// <summary>
    /// 外键：所属商品 ID。
    /// </summary>
    [Comment("商品主键")]
    public Guid? ProductId { get; set; }

    /// <summary>
    /// 图片的存储访问路径（CDN 或静态资源地址）。
    /// </summary>
    [MaxLength(512)]
    [Comment("图片路径")]
    public string? ImageUrl { get; set; }

    /// <summary>
    /// 图片的替代文本，用于 SEO 优化及无障碍访问需求。
    /// </summary>
    [MaxLength(128)]
    [Comment("Alt Text")]
    public string? AltText { get; set; }

    /// <summary>
    /// 展示顺序权重，数值越小展示优先级越高。
    /// </summary>
    [Comment("排序")]
    public int? SortOrder { get; set; }

    /// <summary>
    /// 标记此图片是否为商品详情展示的封面主图。
    /// </summary>
    [Comment("是否封面主图")]
    public bool? IsPrimary { get; set; }
}
