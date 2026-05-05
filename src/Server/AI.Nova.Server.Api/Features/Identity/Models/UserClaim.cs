using AI.Nova.Server.Api.Infrastructure.Data.Audit;

namespace AI.Nova.Server.Api.Features.Identity.Models;

[Table("UserClaims")]
[Comment("用户声明表")]
public class UserClaim : IdentityUserClaim<Guid>, IAuditableEntity, ISoftDelete
{
    public User? User { get; set; }

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

    [Comment("删除时间")]
    public DateTimeOffset? DeletedOn { get; set; }

    [Comment("删除者")]
    public Guid? DeletedBy { get; set; }
}
