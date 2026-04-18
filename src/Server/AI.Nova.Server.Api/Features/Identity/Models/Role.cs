using AI.Nova.Server.Api.Infrastructure.Data.Audit;

namespace AI.Nova.Server.Api.Features.Identity.Models;

[Table("Roles")]
[Comment("角色表：存储系统角色的基础定义、数据权限范围及层级关系，作为用户授权与功能隔离的核心依据。")]
public partial class Role : IdentityRole<Guid>, IAuditableEntity, ISoftDelete
{
    public List<UserRole> Users { get; set; } = [];
    public List<RoleClaim> Claims { get; set; } = [];

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

