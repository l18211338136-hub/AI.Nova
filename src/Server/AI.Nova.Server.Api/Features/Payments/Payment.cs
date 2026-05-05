using AI.Nova.Server.Api.Features.Identity.Models;
using AI.Nova.Server.Api.Features.Orders;
using AI.Nova.Server.Api.Infrastructure.Data.Audit;

namespace AI.Nova.Server.Api.Features.Payments;

/// <summary>
/// 支付流水表：记录订单的所有支付活动，包括支付渠道信息、交易单号、支付状态及确认时间。
/// </summary>
[Table("Payments")]
[Comment("支付流水表")]
public partial class Payment : AuditEntity
{
    /// <summary>
    /// 支付记录唯一标识。
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Comment("主键")]
    public Guid Id { get; set; }

    /// <summary>
    /// 所属的交易订单对象。
    /// </summary>
    [ForeignKey(nameof(OrderId))]
    [Comment("关联的所属订单交易对象")]
    public Order? Order { get; set; }

    /// <summary>
    /// 外键：订单 ID。
    /// </summary>
    [Comment("订单外键")]
    public Guid? OrderId { get; set; }

    /// <summary>
    /// 执行支付的用户。
    /// </summary>
    [ForeignKey(nameof(UserId))]
    [Comment("执行该支付流水记录的用户对象")]
    public User? User { get; set; }

    /// <summary>
    /// 外键：用户 ID。
    /// </summary>
    [Comment("用户外键")]
    public Guid? UserId { get; set; }

    /// <summary>
    /// 本次流水涉及的支付金额。
    /// </summary>
    [Precision(18, 3)]
    [Comment("实付金额")]
    public decimal? Amount { get; set; }

    /// <summary>
    /// 采用的支付方式渠道（Alipay, WeChatPay, UnionPay等）。
    /// </summary>
    [MaxLength(32)]
    [Comment("支付方式 (如：Alipay, WeChatPay)")]
    public string? PaymentMethod { get; set; }

    /// <summary>
    /// 第三方支付平台返回的原始交易参考号（流水号）。
    /// </summary>
    [MaxLength(128)]
    [Comment("交易单号")]
    public string? TransactionId { get; set; }

    /// <summary>
    /// 支付记录的当前处理状态（0:待支付, 1:成功, 2:失败, 3:已退款）。
    /// </summary>
    [Comment("支付状态（0:待支付, 1:成功, 2:失败, 3:已退款）")]
    public short? Status { get; set; }

    /// <summary>
    /// 支付成功被确认的具体时刻。
    /// </summary>
    [Comment("支付时间")]
    public DateTimeOffset? PaidOn { get; set; }
}
