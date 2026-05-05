using AI.Nova.Server.Api.Features.Products;
using AI.Nova.Server.Api.Infrastructure.Data.Audit;

namespace AI.Nova.Server.Api.Features.Orders;

/// <summary>
/// 订单明细表：记录订单中的具体商品项目，包含下单时的名称、单价及图片快照，用于交易追溯。
/// </summary>
[Table("OrderItems")]
[Comment("订单子表")]
public partial class OrderItem : AuditEntity
{
    /// <summary>
    /// 订单项唯一标识。
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Comment("主键")]
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
    [Comment("订单外键")]
    public Guid? OrderId { get; set; }

    /// <summary>
    /// 关联的当前商品实体（可能随时间被软删除或修改）。
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
    /// 下单时刻记录的商品名称快照，防止后期商品更名导致历史订单显示错误。
    /// </summary>
    [MaxLength(128)]
    [Comment("商品名称")]
    public string? ProductName { get; set; }

    /// <summary>
    /// 下单时刻的实际成交单价。
    /// </summary>
    [Precision(18, 3)]
    [Comment("单价")]
    public decimal? UnitPrice { get; set; }

    /// <summary>
    /// 用户购买该商品项的数量。
    /// </summary>
    [Comment("数量")]
    public int? Quantity { get; set; }

    /// <summary>
    /// 该商品项的小计总金额（单价 * 数量）。
    /// </summary>
    [Precision(18, 3)]
    [Comment("总金额")]
    public decimal? SubTotal { get; set; }

    /// <summary>
    /// 下单时刻商品主图的替代文本快照。
    /// </summary>
    [Comment("图片地址")]
    public string? PrimaryImageAltText { get; set; }
}
