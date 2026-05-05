using AI.Nova.Server.Api.Features.Identity.Models;

namespace AI.Nova.Server.Api.Features.Identity.Configurations;

public class UserTokenConfiguration : IEntityTypeConfiguration<UserToken>
{
    public void Configure(EntityTypeBuilder<UserToken> builder)
    {
        builder.HasKey(ut => new { ut.UserId, ut.LoginProvider, ut.Name });

        builder.Property(ut => ut.UserId)
            .HasComment("用户外键");

        builder.Property(ut => ut.LoginProvider)
            .HasComment("提供商（主键）：例如 'AspNetCore.Identity' 或 'Google'");

        builder.Property(ut => ut.Name)
            .HasComment("名称（主键）：例如 'SecurityStamp' 或 'AccessToken'");

        builder.Property(ut => ut.Value)
            .HasComment("令牌值");
    }
}
