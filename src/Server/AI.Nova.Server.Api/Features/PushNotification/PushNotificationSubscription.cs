using AI.Nova.Server.Api.Features.Identity.Models;
using AI.Nova.Server.Api.Infrastructure.Data.Audit;

namespace AI.Nova.Server.Api.Features.PushNotification;

/// <summary>
/// 推送通知订阅实体。
/// 存储用于向用户设备发送推送通知（Web Push, APNs, FCM）所需的凭证和配置信息。
/// </summary>
[Table("PushNotificationSubscriptions")]
[Comment("推送通知订阅表：存储用户设备接收推送通知所需的凭证（如 Endpoint, P256dh, Auth）和订阅状态。")]
public class PushNotificationSubscription : AuditEntity
{
    /// <summary>
    /// 订阅记录的唯一标识符。
    /// </summary>
    [Comment("订阅记录的主键 ID")]
    public int Id { get; set; }

    /// <summary>
    /// 设备的唯一标识符（用于区分同一用户的不同设备）。
    /// </summary>
    [Comment("设备的唯一标识符 (Device ID)")]
    public string? DeviceId { get; set; }

    /// <summary>
    /// 推送平台类型。
    /// 允许值："apns" (Apple), "fcmV1" (Firebase), "browser" (Web Push)。
    /// </summary>
    [AllowedValues("apns", "fcmV1", "browser")]
    [Comment("推送平台类型：'apns' (Apple), 'fcmV1' (Firebase), 'browser' (Web Push)")]
    public string? Platform { get; set; }

    /// <summary>
    /// 推送渠道/订阅 URL。
    /// 对于 Web Push，这是浏览器的 Endpoint URL。
    /// </summary>
    [Comment("推送服务提供的订阅 URL (Endpoint) 或渠道标识")]
    public string? PushChannel { get; set; }

    /// <summary>
    /// Web Push 加密公钥 (P-256)。
    /// 用于应用服务器加密推送消息。
    /// </summary>
    [Comment("Web Push 用户代理公钥 (P-256dh)，用于消息加密")]
    public string? P256dh { get; set; }

    /// <summary>
    /// Web Push 认证密钥。
    /// 用于生成加密盐。
    /// </summary>
    [Comment("Web Push 认证密钥 (Auth Secret)，用于生成加密盐")]
    public string? Auth { get; set; }

    /// <summary>
    /// 完整的推送服务 Endpoint URL（通常与 PushChannel 相同，取决于实现）。
    /// </summary>
    [Comment("完整的推送消息发送 Endpoint URL")]
    public string? Endpoint { get; set; }

    /// <summary>
    /// 关联的用户会话 ID。
    /// </summary>
    [Comment("关联的用户会话 ID (外键)，用于追踪订阅来源设备")]
    public Guid? UserSessionId { get; set; }

    /// <summary>
    /// 导航属性：关联的用户会话。
    /// </summary>
    [ForeignKey(nameof(UserSessionId))]
    public UserSession? UserSession { get; set; }

    /// <summary>
    /// 订阅标签，用于分类推送内容（例如：["news", "alerts"]）。
    /// </summary>
    [Comment("订阅标签数组 (JSON)，用于按主题过滤推送消息")]
    public string[]? Tags { get; set; } = [];

    /// <summary>
    /// 订阅过期时间（Unix 时间戳，秒）。
    /// </summary>
    [Comment("订阅过期时间 (Unix 时间戳，单位：秒)")]
    public long? ExpirationTime { get; set; }

    /// <summary>
    /// 最后续期时间（Unix 时间戳，秒）。
    /// </summary>
    [Comment("订阅最后续期时间 (Unix 时间戳，单位：秒)")]
    public long? RenewedOn { get; set; }
}
