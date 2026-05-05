using AI.Nova.Server.Api.Features.Identity.Models;
using AI.Nova.Server.Api.Features.Products;
using AI.Nova.Server.Api.Infrastructure.Data.Audit;

namespace AI.Nova.Server.Api.Features.Carts;

/// <summary>
/// 购物车明细表：管理用户加购商品的待结算记录，支持多终端同步及结算勾选状态记录。
/// </summary>
[Table("CartItems")]
[Comment("购物车")]
public partial class CartItem : AuditEntity
{
    /// <summary>
    /// 购物车项唯一标识。
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Comment("主键")]
    public Guid Id { get; set; }

    /// <summary>
    /// 拥有该购物篮的用户对象。
    /// </summary>
    [ForeignKey(nameof(UserId))]
    [Comment("用户")]
    public User? User { get; set; }

    /// <summary>
    /// 外键：用户 ID。
    /// </summary>
    [Comment("用户外键")]
    public Guid? UserId { get; set; }

    /// <summary>
    /// 加购的具体商品对象。
    /// </summary>
    [ForeignKey(nameof(ProductId))]
    [Comment("商品外键")]
    public Product? Product { get; set; }

    /// <summary>
    /// 外键：商品 ID。
    /// </summary>
    [Comment("商品外键")]
    public Guid? ProductId { get; set; }

    /// <summary>
    /// 预加购的商品件数。
    /// </summary>
    [Comment("数量")]
    public int? Quantity { get; set; }

    /// <summary>
    /// 该项目在购物车内是否已被用户选定参与本次结算。
    /// </summary>
    [Comment("是否选中")]
    public bool? Selected { get; set; }
}
