using AI.Nova.Server.Api.Features.Todo;
using AI.Nova.Server.Api.Infrastructure.Data.Audit;
using AI.Nova.Shared.Features.Identity.Dtos;

namespace AI.Nova.Server.Api.Features.Identity.Models;

[Table("Users")]
[Comment("用户表")]
public partial class User : IdentityUser<Guid>, IAuditableEntity, ISoftDelete
{
    [PersonalData]
    [Comment("全名")]
    public string? FullName { get; set; }
    [Comment("显示名称")]
    public string? DisplayName => FullName ?? DisplayUserName;
    [Comment("显示账号")]
    public string? DisplayUserName => FullName ?? Email ?? PhoneNumber ?? UserName;

    [PersonalData]
    [Comment("性别")]
    public Gender? Gender { get; set; }

    [PersonalData]
    [Comment("出生日期")]
    public DateTimeOffset? BirthDate { get; set; }

    /// <summary>
    /// The date and time of the last token request. Ensures only the latest generated token is valid and can only be used once.
    /// </summary>
    [Comment("邮箱验证时间")]
    public DateTimeOffset? EmailTokenRequestedOn { get; set; }
    [Comment("手机号时间")]
    public DateTimeOffset? PhoneNumberTokenRequestedOn { get; set; }
    [Comment("重置密码时间")]
    public DateTimeOffset? ResetPasswordTokenRequestedOn { get; set; }
    [Comment("双因素认证 (2FA) 时间")]
    public DateTimeOffset? TwoFactorTokenRequestedOn { get; set; }
    [Comment("一次性密码 (OTP) 的请求时间")]
    public DateTimeOffset? OtpRequestedOn { get; set; }

    /// <summary>
    /// <inheritdoc cref="AuthPolicies.ELEVATED_ACCESS" />
    /// </summary>
    [Comment("Elevated Access请求时间")]
    public DateTimeOffset? ElevatedAccessTokenRequestedOn { get; set; }
    [Comment("用户登录会话集合")]
    public List<UserSession> Sessions { get; set; } = [];
    [Comment("用户关联的待办事项集合")]
    public List<TodoItem> TodoItems { get; set; } = [];
    [Comment("用户注册的 WebAuthn (FIDO2) 安全密钥集合")]
    public List<WebAuthnCredential> WebAuthnCredentials { get; set; } = [];
    [Comment("用户与角色的关联集合（多对多）")]
    public List<UserRole> Roles { get; set; } = [];
    [Comment("用户自定义声明集合（Claims）")]
    public List<UserClaim> Claims { get; set; } = [];
    [Comment("用户关联的外部登录提供商集合（如 Google, Microsoft）")]
    public List<UserLogin> Logins { get; set; } = [];
    [Comment("用户身份验证令牌集合")]
    public List<UserToken> Tokens { get; set; } = [];
    [Comment("是否有头像")]
    public bool? HasProfilePicture { get; set; }

    [Comment("创建时间")]
    public DateTimeOffset? CreatedOn { get; set; }

    [Comment("创建者")]
    public Guid? CreatedBy { get; set; }

    [Comment("修改时间")]
    public DateTimeOffset? ModifiedOn { get; set; }

    [Comment("修改者")]
    public Guid? ModifiedBy { get; set; }

    [Comment("软删除")]
    public bool? IsDeleted { get; set; }

    [Comment("除时间")]
    public DateTimeOffset? DeletedOn { get; set; }

    [Comment("删除者")]
    public Guid? DeletedBy { get; set; }
}
