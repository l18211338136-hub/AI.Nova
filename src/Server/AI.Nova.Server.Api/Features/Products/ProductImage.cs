using AI.Nova.Server.Api.Infrastructure.Data.Audit;

namespace AI.Nova.Server.Api.Features.Products;

/// <summary>
/// 商品图片关联表：存储商品的多媒体展示资源，支持多图展示、主图标记及排序展示。
/// </summary>
[Table("ProductImages")]
[Comment("商品图片关联表：存储商品的多媒体展示资源，支持多图展示、主图标记及排序展示。")]
public partial class ProductImage : AuditEntity
{
    /// <summary>
    /// 图片记录唯一标识。
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Comment("图片记录唯一标识")]
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
    [Comment("关联的商品 ID")]
    public Guid? ProductId { get; set; }

    /// <summary>
    /// 图片的存储访问路径（CDN 或静态资源地址）。
    /// </summary>
    [MaxLength(512)]
    [Comment("图片素材的存储 URL 路径")]
    public string? ImageUrl { get; set; }

    /// <summary>
    /// 图片的替代文本，用于 SEO 优化及无障碍访问需求。
    /// </summary>
    [MaxLength(128)]
    [Comment("图片替代文本 (Alt Text)，用于 SEO 和无障碍显示")]
    public string? AltText { get; set; }

    /// <summary>
    /// 展示顺序权重，数值越小展示优先级越高。
    /// </summary>
    [Comment("图片在展示列表中的排序权重 (升序排列)")]
    public int? SortOrder { get; set; }

    /// <summary>
    /// 标记此图片是否为商品详情展示的封面主图。
    /// </summary>
    [Comment("是否将此图片设为商品封面主图")]
    public bool? IsPrimary { get; set; }
}
