using AI.Nova.Server.Api.Features.Todo;
using AI.Nova.Server.Api.Infrastructure.Data.Audit;
using AI.Nova.Shared.Features.Identity.Dtos;

namespace AI.Nova.Server.Api.Features.Identity.Models;

[Table("Users")]
[Comment("用户核心表：存储系统用户的账户信息、个人资料及安全凭证。")]
public partial class User : IdentityUser<Guid>, IAuditableEntity, ISoftDelete
{
    [PersonalData]
    [Comment("用户的全名")]
    public string? FullName { get; set; }

    public string? DisplayName => FullName ?? DisplayUserName;
    public string? DisplayUserName => FullName ?? Email ?? PhoneNumber ?? UserName;

    [PersonalData]
    [Comment("用户性别")]
    public Gender? Gender { get; set; }

    [PersonalData]
    [Comment("用户出生日期")]
    public DateTimeOffset? BirthDate { get; set; }

    /// <summary>
    /// The date and time of the last token request. Ensures only the latest generated token is valid and can only be used once.
    /// </summary>
    [Comment("邮箱验证/修改令牌的最后请求时间，用于安全校验")]
    public DateTimeOffset? EmailTokenRequestedOn { get; set; }
    [Comment("手机号验证令牌的最后请求时间")]
    public DateTimeOffset? PhoneNumberTokenRequestedOn { get; set; }
    [Comment("重置密码令牌的最后请求时间")]
    public DateTimeOffset? ResetPasswordTokenRequestedOn { get; set; }
    [Comment("双因素认证 (2FA) 令牌的最后请求时间")]
    public DateTimeOffset? TwoFactorTokenRequestedOn { get; set; }
    [Comment("一次性密码 (OTP) 的最后请求时间")]
    public DateTimeOffset? OtpRequestedOn { get; set; }

    /// <summary>
    /// <inheritdoc cref="AuthPolicies.ELEVATED_ACCESS" />
    /// </summary>
    [Comment("高权限访问令牌 (Elevated Access) 的最后请求时间")]
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
    [Comment("是否拥有头像标记")]
    public bool? HasProfilePicture { get; set; }

    [Comment("记录创建时间")]
    public DateTimeOffset? CreatedOn { get; set; }

    [Comment("记录创建者ID")]
    public Guid? CreatedBy { get; set; }

    [Comment("记录最后修改时间")]
    public DateTimeOffset? ModifiedOn { get; set; }

    [Comment("记录最后修改者ID")]
    public Guid? ModifiedBy { get; set; }

    [Comment("软删除标记：true表示已删除")]
    public bool? IsDeleted { get; set; }

    [Comment("记录删除时间")]
    public DateTimeOffset? DeletedOn { get; set; }

    [Comment("记录删除者ID")]
    public Guid? DeletedBy { get; set; }
}
