using AI.Nova.Server.Api.Features.Identity.Models;
using AI.Nova.Shared.Features.Identity.Dtos;

namespace AI.Nova.Server.Api.Features.Identity.Configurations;

public partial class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.UserName)
           .HasComment("用户名");

        builder.Property(u => u.NormalizedUserName)
            .HasComment("标准化用户名 (用于索引和查找)");

        builder.Property(u => u.Email)
            .HasComment("电子邮件地址");

        builder.Property(u => u.NormalizedEmail)
            .HasComment("标准化电子邮件地址");

        builder.Property(u => u.EmailConfirmed)
            .HasComment("邮箱是否已确认");

        builder.Property(u => u.PasswordHash)
            .HasComment("密码的加盐哈希值");

        builder.Property(u => u.SecurityStamp)
            .HasComment("安全戳：当用户凭据变更（如改密、删登录）时更改，用于使旧令牌失效");

        builder.Property(u => u.ConcurrencyStamp)
            .HasComment("并发戳：用于乐观并发控制，每次持久化到数据库时更改");

        builder.Property(u => u.PhoneNumber)
            .HasComment("电话号码");

        builder.Property(u => u.PhoneNumberConfirmed)
            .HasComment("电话号码是否已确认");

        builder.Property(u => u.TwoFactorEnabled)
            .HasComment("是否启用双因素认证 (2FA)");

        builder.Property(u => u.LockoutEnd)
            .HasComment("锁定期结束时间 (UTC)。如果为过去时间或空，表示未锁定");

        builder.Property(u => u.LockoutEnabled)
            .HasComment("是否允许用户被锁定");

        builder.Property(u => u.AccessFailedCount)
            .HasComment("登录失败尝试次数");

        builder.HasMany(user => user.Roles)
            .WithOne(ur => ur.User)
            .HasForeignKey(ur => ur.UserId);

        builder.HasMany(user => user.Claims)
            .WithOne(ur => ur.User)
            .HasForeignKey(ur => ur.UserId);

        builder.HasMany(user => user.Tokens)
            .WithOne(ut => ut.User)
            .HasForeignKey(ut => ut.UserId);

        builder.HasMany(user => user.Logins)
            .WithOne(ul => ul.User)
            .HasForeignKey(ul => ul.UserId);
    }
}
