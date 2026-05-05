using AI.Nova.Server.Api.Features.Identity.Models;

namespace AI.Nova.Server.Api.Features.Identity.Configurations;

public partial class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.Property(ur => ur.UserId)
                    .HasComment("用户外键");

        builder.Property(ur => ur.RoleId)
            .HasComment("角色外键");

        builder.HasIndex(userRole => new { userRole.RoleId, userRole.UserId }).IsUnique();
    }
}
