using AI.Nova.Server.Api.Features.Identity.Models;

namespace AI.Nova.Server.Api.Features.Identity.Configurations;

public partial class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.Property(ur => ur.UserId)
                    .HasComment("用户ID（主键的一部分）：关联到 Users 表");

        builder.Property(ur => ur.RoleId)
            .HasComment("角色ID（主键的一部分）：关联到 Roles 表");

        builder.HasIndex(userRole => new { userRole.RoleId, userRole.UserId }).IsUnique();
    }
}
