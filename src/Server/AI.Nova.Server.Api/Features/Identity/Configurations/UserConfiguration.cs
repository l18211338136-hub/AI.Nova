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

        const string userName = "test";
        const string email = "761516331@qq.com";

        builder.HasData([new User
        {
            Id = Guid.Parse("8ff71671-a1d6-4f97-abb9-d87d7b47d6e7"),
            EmailConfirmed = true,
            LockoutEnabled = true,
            Gender = Gender.Other,
            BirthDate = new DateTimeOffset(new DateOnly(2023, 1, 1), default, default),
            FullName = "AI.Nova test account",
            UserName = userName,
            NormalizedUserName = userName.ToUpperInvariant(),
            Email = email,
            NormalizedEmail = email.ToUpperInvariant(),
            EmailTokenRequestedOn = new DateTimeOffset(new DateOnly(2023, 1, 1), default, default),
            PhoneNumber = "+31684207362",
            PhoneNumberConfirmed = true,
            SecurityStamp = "959ff4a9-4b07-4cc1-8141-c5fc033daf83",
            ConcurrencyStamp = "315e1a26-5b3a-4544-8e91-2760cd28e231",
            PasswordHash = "AQAAAAIAAYagAAAAEP0v3wxkdWtMkHA3Pp5/JfS+42/Qto9G05p2mta6dncSK37hPxEHa3PGE4aqN30Aag==", // 123456
        }]);

        builder
            .HasIndex(b => b.Email)
            .HasFilter($"'{nameof(User.Email)}' IS NOT NULL")
            .IsUnique();

        builder
            .HasIndex(b => b.PhoneNumber)
            .HasFilter($"'{nameof(User.PhoneNumber)}' IS NOT NULL")
            .IsUnique();
    }
}
