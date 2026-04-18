using AI.Nova.Server.Api.Features.Identity.Models;
using AI.Nova.Server.Api.Features.Products;
using AI.Nova.Server.Api.Infrastructure.Data.Audit;

namespace AI.Nova.Server.Api.Features.Carts;

/// <summary>
/// 购物车明细表：管理用户加购商品的待结算记录，支持多终端同步及结算勾选状态记录。
/// </summary>
[Table("CartItems")]
[Comment("购物车明细表：记录用户添加到结算清单的商品、数量及勾选状态。")]
public partial class CartItem : AuditEntity
{
    /// <summary>
    /// 购物车项唯一标识。
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Comment("购物车记录唯一标识")]
    public Guid Id { get; set; }

    /// <summary>
    /// 拥有该购物篮的用户对象。
    /// </summary>
    [ForeignKey(nameof(UserId))]
    [Comment("该购物车记录所属的用户对象")]
    public User? User { get; set; }

    /// <summary>
    /// 外键：用户 ID。
    /// </summary>
    [Comment("关联的用户 ID")]
    public Guid? UserId { get; set; }

    /// <summary>
    /// 加购的具体商品对象。
    /// </summary>
    [ForeignKey(nameof(ProductId))]
    [Comment("被添加到购物车的商品实体")]
    public Product? Product { get; set; }

    /// <summary>
    /// 外键：商品 ID。
    /// </summary>
    [Comment("关联的商品 ID")]
    public Guid? ProductId { get; set; }

    /// <summary>
    /// 预加购的商品件数。
    /// </summary>
    [Comment("用户预备购买的商品件数")]
    public int? Quantity { get; set; }

    /// <summary>
    /// 该项目在购物车内是否已被用户选定参与本次结算。
    /// </summary>
    [Comment("标记该商品当前是否在结算清单中处于勾选/激活状态")]
    public bool? Selected { get; set; }
}
