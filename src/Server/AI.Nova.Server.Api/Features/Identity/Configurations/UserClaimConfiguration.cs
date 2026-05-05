using AI.Nova.Server.Api.Features.Identity.Models;

namespace AI.Nova.Server.Api.Features.Identity.Configurations;

public partial class UserClaimConfiguration : IEntityTypeConfiguration<UserClaim>
{
    public void Configure(EntityTypeBuilder<UserClaim> builder)
    {
        builder.Property(uc => uc.UserId).HasComment("用户外键");

        builder.Property(uc => uc.ClaimType)
            .HasMaxLength(256) 
            .HasComment("声明类型");

        builder.Property(uc => uc.ClaimValue)
            .HasMaxLength(1024)
            .HasComment("声明值");

        builder.HasIndex(userClaim => new { userClaim.UserId, userClaim.ClaimType, userClaim.ClaimValue });
    }
}
