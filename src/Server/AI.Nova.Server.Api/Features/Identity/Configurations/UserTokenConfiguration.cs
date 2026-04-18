using AI.Nova.Server.Api.Features.Identity.Models;

namespace AI.Nova.Server.Api.Features.Identity.Configurations;

public class UserTokenConfiguration : IEntityTypeConfiguration<UserToken>
{
    public void Configure(EntityTypeBuilder<UserToken> builder)
    {
        builder.HasKey(ut => new { ut.UserId, ut.LoginProvider, ut.Name });

        builder.Property(ut => ut.UserId)
            .HasComment("用户ID（主键的一部分）：关联到 Users 表");

        builder.Property(ut => ut.LoginProvider)
            .HasComment("令牌提供商名称（主键的一部分）：例如 'AspNetCore.Identity' 或 'Google'");

        builder.Property(ut => ut.Name)
            .HasComment("令牌名称（主键的一部分）：例如 'SecurityStamp' 或 'AccessToken'");

        builder.Property(ut => ut.Value)
            .HasComment("令牌的具体值（敏感数据，通常经过哈希处理或加密）");
    }
}
