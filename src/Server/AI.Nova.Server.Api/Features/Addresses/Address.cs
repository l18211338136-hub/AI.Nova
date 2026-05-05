using AI.Nova.Server.Api.Features.Identity.Models;
using AI.Nova.Server.Api.Features.Orders;
using AI.Nova.Server.Api.Infrastructure.Data.Audit;

namespace AI.Nova.Server.Api.Features.Addresses;

/// <summary>
/// 用户收货地址表：存储用户的多级行政区划地址信息，支持标记默认地址及作为订单配送的依据。
/// </summary>
[Table("Addresses")]
[Comment("用户收货地址表")]
public partial class Address : AuditEntity
{
    /// <summary>
    /// 主键
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Comment("主键")]
    public Guid Id { get; set; }

    /// <summary>
    /// 该地址所属的用户账户。
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
    /// 负责接收货物的联系人姓名快照。
    /// </summary>
    [MaxLength(64)]
    [Comment("收货人姓名")]
    public string? RecipientName { get; set; }

    /// <summary>
    /// 用于物流联系的电话单号。
    /// </summary>
    [MaxLength(20)]
    [Comment("收货联系号码")]
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// 地址所属的一级行政区（省/直辖市/自治区）。
    /// </summary>
    [MaxLength(32)]
    [Comment("省")]
    public string? Province { get; set; }

    /// <summary>
    /// 地址所属的二级行政区（地级市/盟/自治州）。
    /// </summary>
    [MaxLength(32)]
    [Comment("市")]
    public string? City { get; set; }

    /// <summary>
    /// 地址所属的三级行政区（区/县/旗）。
    /// </summary>
    [MaxLength(32)]
    [Comment("区")]
    public string? District { get; set; }

    /// <summary>
    /// 四级以下详细路名、门牌号或地标描述。
    /// </summary>
    [MaxLength(256)]
    [Comment("详细地址")]
    public string? StreetAddress { get; set; }

    /// <summary>
    /// 所在区域的邮政编码。
    /// </summary>
    [MaxLength(10)]
    [Comment("邮政编码")]
    public string? PostalCode { get; set; }

    /// <summary>
    /// 标记该地址是否作为用户的常用默认收货地址。
    /// </summary>
    [Comment("默认地址")]
    public bool? IsDefault { get; set; }

    // 导航属性
    /// <summary>
    /// 曾经或当前以此地址作为收货目标的订单集合。
    /// </summary>
    public IList<Order> Orders { get; set; } = [];
}
