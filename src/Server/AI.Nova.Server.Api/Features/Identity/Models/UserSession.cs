using AI.Nova.Server.Api.Features.PushNotification;
using AI.Nova.Server.Api.Infrastructure.Data.Audit;

namespace AI.Nova.Server.Api.Features.Identity.Models;

/// <summary>
/// 用户会话实体。
/// 用于跟踪用户的活动会话，包括设备信息、位置、安全状态和通知设置。
/// </summary>
[Table("UserSessions")]
[Comment("用户会话表：记录用户的登录会话信息，用于设备管理、安全审计和推送通知。")]
public partial class UserSession : AuditEntity
{
    /// <summary>
    /// 会话的唯一标识符。
    /// </summary>
    [Comment("会话的唯一标识符 (主键)")]
    public Guid Id { get; set; }

    /// <summary>
    /// 会话发起时的 IP 地址。
    /// </summary>
    [Comment("用户会话的 IP 地址")]
    public string? IP { get; set; }

    /// <summary>
    /// 基于 IP 解析的物理地址（城市、国家等）。
    /// </summary>
    [Comment("基于 IP 地址解析的地理位置信息")]
    public string? Address { get; set; }

    /// <summary>
    /// 指示该会话是否拥有特权访问权限。
    /// <inheritdoc cref="AuthPolicies.PRIVILEGED_ACCESS"/>
    /// </summary>
    [Comment("特权访问标记：指示该会话是否拥有高权限（如管理员操作）")]
    public bool? Privileged { get; set; }

    /// <summary>
    /// 会话开始的时间（Unix 时间戳，秒）。
    /// </summary>
    [Comment("会话开始时间 (Unix 时间戳，单位：秒)")]
    public long? StartedOn { get; set; }

    /// <summary>
    /// 会话最后续期的时间（Unix 时间戳，秒）。
    /// </summary>
    [Comment("会话最后续期时间 (Unix 时间戳，单位：秒)")]
    public long? RenewedOn { get; set; }

    /// <summary>
    /// 关联用户的唯一标识符。
    /// </summary>
    [Comment("关联用户的 ID (外键)")]
    public Guid? UserId { get; set; }

    /// <summary>
    /// 导航属性：关联的用户实体。
    /// </summary>
    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }

    /// <summary>
    /// 导航属性：关联的推送通知订阅信息。
    /// </summary>
    [Comment("关联的推送通知订阅信息")]
    public PushNotificationSubscription? PushNotificationSubscription { get; set; }

    /// <summary>
    /// SignalR 连接 ID，用于实时通信。
    /// </summary>
    [Comment("SignalR 连接 ID，用于实时消息推送")]
    public string? SignalRConnectionId { get; set; }

    /// <summary>
    /// 该会话的通知状态（例如：已读、未读、已禁用）。
    /// </summary>
    [Comment("推送通知状态：0=未配置, 1=允许, 2=静音")]
    public UserSessionNotificationStatus? NotificationStatus { get; set; }

    /// <summary>
    /// 设备详细信息（User-Agent 或自定义 JSON）。
    /// </summary>
    [Comment("设备详细信息（浏览器、操作系统、设备型号等）")]
    public string? DeviceInfo { get; set; }

    /// <summary>
    /// 应用平台类型（Web, iOS, Android, Desktop）。
    /// </summary>
    [Comment("客户端应用平台类型（如：Web, iOS, Android, Windows）")]
    public AppPlatformType? PlatformType { get; set; }

    /// <summary>
    /// 用户在该会话中选择的语言文化（例如：zh-CN, en-US）。
    /// </summary>
    [Comment("用户在该会话中选择的语言文化代码 (如：zh-CN, en-US)")]
    public string? CultureName { get; set; }

    /// <summary>
    /// 客户端应用程序的版本号。
    /// </summary>
    [Comment("客户端应用程序的版本号")]
    public string? AppVersion { get; set; }
}
