using AI.Nova.Server.Api.Infrastructure.Data.Audit;

namespace AI.Nova.Server.Api.Features.Identity.Models;


[Table("UserLogins")]
[Comment("用户外部登录表：存储用户关联的第三方登录提供商信息（如 Google, Microsoft, Facebook 等）。")]
public class UserLogin : IdentityUserLogin<Guid>, IAuditableEntity, ISoftDelete
{
    public User? User { get; set; }

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
