using AI.Nova.Server.Api.Features.Addresses;
using AI.Nova.Server.Api.Features.Identity.Models;
using AI.Nova.Server.Api.Features.Payments;
using AI.Nova.Server.Api.Features.Products;
using AI.Nova.Server.Api.Infrastructure.Data.Audit;

namespace AI.Nova.Server.Api.Features.Orders;

/// <summary>
/// 订单主表：记录交易流转的核心单据，记录总金额、订单状态、收货地址以及配送详情。
/// </summary>
[Table("Orders")]
[Comment("订单主表：电商交易的核心记录，维护订单生命周期状态及金额明细。")]
public partial class Order : AuditEntity
{
    /// <summary>
    /// 订单记录唯一标识。
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Comment("订单记录唯一标识")]
    public Guid Id { get; set; }

    /// <summary>
    /// 面向业务的唯一订单编号（如：20240418XXXX）。
    /// </summary>
    [MaxLength(32)]
    [Comment("全局唯一业务订单编号")]
    public string? OrderNo { get; set; }

    /// <summary>
    /// 下单的用户主体。
    /// </summary>
    [ForeignKey(nameof(UserId))]
    [Comment("关联的下单用户对象")]
    public User? User { get; set; }

    /// <summary>
    /// 外键：用户 ID。
    /// </summary>
    [Comment("关联的下单用户 ID")]
    public Guid? UserId { get; set; }

    /// <summary>
    /// 订单内所有商品按原价计算的总额。
    /// </summary>
    [Precision(18, 3)]
    [Comment("商品原始销售总价")]
    public decimal? TotalAmount { get; set; }

    /// <summary>
    /// 各类营销活动及礼券产生的抵扣金额。
    /// </summary>
    [Precision(18, 3)]
    [Comment("优惠抵扣的总金额")]
    public decimal? DiscountAmount { get; set; }

    /// <summary>
    /// 配送商品所需的运费支出。
    /// </summary>
    [Precision(18, 3)]
    [Comment("订单物流配送费用")]
    public decimal? ShippingFee { get; set; }

    /// <summary>
    /// 买家最终实际需要结算支付的金额。
    /// </summary>
    [Precision(18, 3)]
    [Comment("实付应结的总金额 (含运费并扣除优惠)")]
    public decimal? PayableAmount { get; set; }

    /// <summary>
    /// 订单实时流转状态（0:待付款, 1:已付款, 2:已发货, 3:已完成, 4:已取消, 5:退款中）。
    /// </summary>
    [Comment("0:待付款, 1:已付款, 2:已发货, 3:已完成, 4:已取消, 5:退款中")]
    public short? Status { get; set; }

    /// <summary>
    /// 订单指定的收货位置信息。
    /// </summary>
    [ForeignKey(nameof(AddressId))]
    [Comment("该订单指定的收货地址对象")]
    public Address? Address { get; set; }

    /// <summary>
    /// 外键：收货地址 ID。
    /// </summary>
    [Comment("关联的配送地址 ID")]
    public Guid? AddressId { get; set; }

    /// <summary>
    /// 买家在提交订单时备注的特殊说明要求。
    /// </summary>
    [MaxLength(512)]
    [Comment("用户提供的订单留言或特殊备注说明")]
    public string? Remark { get; set; }

    /// <summary>
    /// 系统接收到第三方支付回执的确认时间。
    /// </summary>
    [Comment("该订单由于支付成功而被确认的时间戳")]
    public DateTimeOffset? PaidOn { get; set; }

    // 导航属性
    /// <summary>
    /// 订单关联的商品明细项集合。
    /// </summary>
    public IList<OrderItem> OrderItems { get; set; } = [];

    /// <summary>
    /// 对此订单内商品发表的项目评价。
    /// </summary>
    public IList<ProductReview> ProductReviews { get; set; } = [];

    /// <summary>
    /// 该订单关联的支付流水记录集合。
    /// </summary>
    public IList<Payment> Payments { get; set; } = [];
}
