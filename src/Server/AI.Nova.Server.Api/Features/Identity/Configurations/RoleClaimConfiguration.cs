using AI.Nova.Server.Api.Features.Identity.Models;

namespace AI.Nova.Server.Api.Features.Identity.Configurations;

public partial class RoleClaimConfiguration : IEntityTypeConfiguration<RoleClaim>
{
    public void Configure(EntityTypeBuilder<RoleClaim> builder)
    {
        builder.Property(x => x.ClaimType)
           .HasMaxLength(255) // 建议设置最大长度
           .HasComment("声明类型");

        builder.Property(x => x.ClaimValue)
            .HasMaxLength(255)
            .HasComment("声明值");

        builder.Property(x => x.RoleId)
            .HasComment("角色外键");

        builder.HasIndex(roleClaim => new { roleClaim.RoleId, roleClaim.ClaimType, roleClaim.ClaimValue });
    }
}
