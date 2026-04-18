using AI.Nova.Server.Api.Features.Identity.Models;
using AI.Nova.Server.Api.Features.Orders;
using AI.Nova.Server.Api.Infrastructure.Data.Audit;

namespace AI.Nova.Server.Api.Features.Products;

/// <summary>
/// 商品评价表：存储用户对已购商品的评价信息，包括评分、评论内容及匿名偏好，用于商品信誉体系构建。
/// </summary>
[Table("ProductReviews")]
[Comment("商品评价表：存储用户对已购商品的评价信息，用于商品信誉和口碑展示。")]
public partial class ProductReview : AuditEntity
{
    /// <summary>
    /// 评价记录唯一标识。
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Comment("评价记录唯一标识")]
    public Guid Id { get; set; }

    /// <summary>
    /// 关联的订单，评价通常基于已成交的订单产生。
    /// </summary>
    [ForeignKey(nameof(OrderId))]
    [Comment("产生该评价的相关交易订单")]
    public Order? Order { get; set; }

    /// <summary>
    /// 外键：订单 ID。
    /// </summary>
    [Comment("关联的订单 ID")]
    public Guid? OrderId { get; set; }

    /// <summary>
    /// 关联被评价的商品。
    /// </summary>
    [ForeignKey(nameof(ProductId))]
    [Comment("被评价的商品对象")]
    public Product? Product { get; set; }

    /// <summary>
    /// 外键：商品 ID。
    /// </summary>
    [Comment("被评价的商品 ID")]
    public Guid? ProductId { get; set; }

    /// <summary>
    /// 发表评价的用户。
    /// </summary>
    [ForeignKey(nameof(UserId))]
    [Comment("发表评论的用户对象")]
    public User? User { get; set; }

    /// <summary>
    /// 外键：用户 ID。
    /// </summary>
    [Comment("发表评论的用户 ID")]
    public Guid? UserId { get; set; }

    /// <summary>
    /// 用户给出的评分等级，通常为 1-5 星。
    /// </summary>
    [Comment("商品评分 (取值范围 1-5)")]
    public short? Rating { get; set; }

    /// <summary>
    /// 评价的详细文字描述（支持后期 SEO 检索及前端展示）。
    /// </summary>
    [MaxLength(1024)]
    [Comment("用户发表的评价正文内容")]
    public string? Comment { get; set; }

    /// <summary>
    /// 用户是否选择隐藏个人身份进行匿名评论。
    /// </summary>
    [Comment("是否启用匿名方式显示评价")]
    public bool? IsAnonymous { get; set; }
}
