using AI.Nova.Server.Api.Features.Identity.Models;

namespace AI.Nova.Server.Api.Features.Identity.Configurations;

public partial class RoleClaimConfiguration : IEntityTypeConfiguration<RoleClaim>
{
    public void Configure(EntityTypeBuilder<RoleClaim> builder)
    {
        builder.Property(x => x.ClaimType)
           .HasMaxLength(255) // 建议设置最大长度
           .HasComment("声明类型：定义权限的种类，例如 'MaxPrivilegedSessions' 或 'Features'");

        builder.Property(x => x.ClaimValue)
            .HasMaxLength(255)
            .HasComment("声明值：权限的具体数值或标识，例如 '-1' 代表无限制，或具体的功能代码");

        builder.Property(x => x.RoleId)
            .HasComment("关联的角色 ID：外键，指向该声明所属的角色");

        builder.HasIndex(roleClaim => new { roleClaim.RoleId, roleClaim.ClaimType, roleClaim.ClaimValue });
    }
}
