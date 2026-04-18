using AI.Nova.Server.Api.Infrastructure.Data.Audit;

namespace AI.Nova.Server.Api.Features.Identity.Models;

[Table("RoleClaims")]
[Comment("角色声明表：存储角色关联的声明数据（如权限标识、自定义属性等）。")]
public class RoleClaim : IdentityRoleClaim<Guid>, IAuditableEntity, ISoftDelete
{
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
