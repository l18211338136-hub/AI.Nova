using AI.Nova.Server.Api.Features.Products;
using AI.Nova.Server.Api.Infrastructure.Data.Audit;

namespace AI.Nova.Server.Api.Features.Orders;

/// <summary>
/// 订单明细表：记录订单中的具体商品项目，包含下单时的名称、单价及图片快照，用于交易追溯。
/// </summary>
[Table("OrderItems")]
[Comment("订单明细表：记录订单中每一项商品的详细快照及购买数量。")]
public partial class OrderItem : AuditEntity
{
    /// <summary>
    /// 订单项唯一标识。
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Comment("订单项记录唯一标识")]
    public Guid Id { get; set; }

    /// <summary>
    /// 所属的主订单对象。
    /// </summary>
    [ForeignKey(nameof(OrderId))]
    [Comment("该明细所属的主订单对象")]
    public Order? Order { get; set; }

    /// <summary>
    /// 外键：主订单 ID。
    /// </summary>
    [Comment("关联的主订单 ID")]
    public Guid? OrderId { get; set; }

    /// <summary>
    /// 关联的当前商品实体（可能随时间被软删除或修改）。
    /// </summary>
    [ForeignKey(nameof(ProductId))]
    [Comment("关联的实际商品对象")]
    public Product? Product { get; set; }

    /// <summary>
    /// 外键：商品 ID。
    /// </summary>
    [Comment("关联的商品 ID")]
    public Guid? ProductId { get; set; }

    /// <summary>
    /// 下单时刻记录的商品名称快照，防止后期商品更名导致历史订单显示错误。
    /// </summary>
    [MaxLength(128)]
    [Comment("下单时刻存储的商品名称属性快照")]
    public string? ProductName { get; set; }

    /// <summary>
    /// 下单时刻的实际成交单价。
    /// </summary>
    [Precision(18, 3)]
    [Comment("下单时刻存储的成交单价快照")]
    public decimal? UnitPrice { get; set; }

    /// <summary>
    /// 用户购买该商品项的数量。
    /// </summary>
    [Comment("用户购买商品的选择数量")]
    public int? Quantity { get; set; }

    /// <summary>
    /// 该商品项的小计总金额（单价 * 数量）。
    /// </summary>
    [Precision(18, 3)]
    [Comment("该订单项对应的合计金额小计")]
    public decimal? SubTotal { get; set; }

    /// <summary>
    /// 下单时刻商品主图的替代文本快照。
    /// </summary>
    [Comment("下单时刻存储的图片替代文本快照")]
    public string? PrimaryImageAltText { get; set; }
}
