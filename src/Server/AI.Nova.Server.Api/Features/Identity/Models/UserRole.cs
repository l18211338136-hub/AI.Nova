using AI.Nova.Server.Api.Infrastructure.Data.Audit;

namespace AI.Nova.Server.Api.Features.Identity.Models;

[Table("UserRoles")]
[Comment("用户角色关联表：用于实现用户与角色的多对多关系。")]
public class UserRole : IdentityUserRole<Guid>, IAuditableEntity, ISoftDelete
{
    public User? User { get; set; }

    public Role? Role { get; set; }

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
