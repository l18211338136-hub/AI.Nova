using AI.Nova.Server.Api.Features.Identity.Models;

namespace AI.Nova.Server.Api.Features.Identity.Configurations;

public partial class UserClaimConfiguration : IEntityTypeConfiguration<UserClaim>
{
    public void Configure(EntityTypeBuilder<UserClaim> builder)
    {
        builder.Property(uc => uc.UserId).HasComment("用户表主键Id：关联Users表Id字段");

        builder.Property(uc => uc.ClaimType)
            .HasMaxLength(256) 
            .HasComment("声明的类型（例如：'Permission.Read', 'Department', 'FullName'）");

        builder.Property(uc => uc.ClaimValue)
            .HasMaxLength(1024)
            .HasComment("声明的具体值（例如：'Admin', 'HR', 'John Doe'）");

        builder.HasIndex(userClaim => new { userClaim.UserId, userClaim.ClaimType, userClaim.ClaimValue });
    }
}
