using AI.Nova.Server.Api.Features.Identity.Models;
using AI.Nova.Server.Api.Infrastructure.Data.Audit;

namespace AI.Nova.Server.Api.Features.PushNotification;

/// <summary>
/// 推送通知订阅实体。
/// 存储用于向用户设备发送推送通知（Web Push, APNs, FCM）所需的凭证和配置信息。
/// </summary>
[Table("PushNotificationSubscriptions")]
[Comment("推送通知订阅表")]
public class PushNotificationSubscription : AuditEntity
{
    /// <summary>
    /// 订阅记录的唯一标识符。
    /// </summary>
    [Comment("主键")]
    public int Id { get; set; }

    /// <summary>
    /// 设备的唯一标识符（用于区分同一用户的不同设备）。
    /// </summary>
    [Comment("设备标识符")]
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
    [Comment("推送渠道")]
    public string? PushChannel { get; set; }

    /// <summary>
    /// Web Push 加密公钥 (P-256)。
    /// 用于应用服务器加密推送消息。
    /// </summary>
    [Comment("加密公钥")]
    public string? P256dh { get; set; }

    /// <summary>
    /// Web Push 认证密钥。
    /// 用于生成加密盐。
    /// </summary>
    [Comment("认证密钥")]
    public string? Auth { get; set; }

    /// <summary>
    /// 完整的推送服务 Endpoint URL（通常与 PushChannel 相同，取决于实现）。
    /// </summary>
    [Comment("推送地址")]
    public string? Endpoint { get; set; }

    /// <summary>
    /// 关联的用户会话 ID。
    /// </summary>
    [Comment("用户会话外键")]
    public Guid? UserSessionId { get; set; }

    /// <summary>
    /// 导航属性：关联的用户会话。
    /// </summary>
    [ForeignKey(nameof(UserSessionId))]
    public UserSession? UserSession { get; set; }

    /// <summary>
    /// 订阅标签，用于分类推送内容（例如：["news", "alerts"]）。
    /// </summary>
    [Comment("订阅标签数组[\"news\", \"alerts\"]")]
    public string[]? Tags { get; set; } = [];

    /// <summary>
    /// 订阅过期时间（Unix 时间戳，秒）。
    /// </summary>
    [Comment("订阅过期时间（秒）")]
    public long? ExpirationTime { get; set; }

    /// <summary>
    /// 最后续期时间（Unix 时间戳，秒）。
    /// </summary>
    [Comment("订阅续期时间 （秒）")]
    public long? RenewedOn { get; set; }
}
