using AI.Nova.Server.Api.Features.Identity.Models;
using AI.Nova.Server.Api.Features.Orders;
using AI.Nova.Server.Api.Infrastructure.Data.Audit;

namespace AI.Nova.Server.Api.Features.Payments;

/// <summary>
/// 支付流水表：记录订单的所有支付活动，包括支付渠道信息、交易单号、支付状态及确认时间。
/// </summary>
[Table("Payments")]
[Comment("支付流水表：记录用户对订单进行的每一笔支付尝试及其最终状态。")]
public partial class Payment : AuditEntity
{
    /// <summary>
    /// 支付记录唯一标识。
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Comment("支付记录唯一标识")]
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
    [Comment("关联的所属订单 ID")]
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
    [Comment("执行该支付流水记录的用户 ID")]
    public Guid? UserId { get; set; }

    /// <summary>
    /// 本次流水涉及的支付金额。
    /// </summary>
    [Precision(18, 3)]
    [Comment("本次支付流水的实付金额")]
    public decimal? Amount { get; set; }

    /// <summary>
    /// 采用的支付方式渠道（Alipay, WeChatPay, UnionPay等）。
    /// </summary>
    [MaxLength(32)]
    [Comment("支付方式/渠道名称 (如：Alipay, WeChatPay)")]
    public string? PaymentMethod { get; set; }

    /// <summary>
    /// 第三方支付平台返回的原始交易参考号（流水号）。
    /// </summary>
    [MaxLength(128)]
    [Comment("三方支付机构返回的全局唯一交易参考单号")]
    public string? TransactionId { get; set; }

    /// <summary>
    /// 支付记录的当前处理状态（0:待支付, 1:成功, 2:失败, 3:已退款）。
    /// </summary>
    [Comment("该笔支付流水当前的处理状态枚举")]
    public short? Status { get; set; }

    /// <summary>
    /// 支付成功被确认的具体时刻。
    /// </summary>
    [Comment("支付状态被标记为成功的时间点")]
    public DateTimeOffset? PaidOn { get; set; }
}
