using AI.Nova.Server.Api.Features.Identity.Models;
using AI.Nova.Server.Api.Infrastructure.Data.Seed;
using AI.Nova.Shared.Features.Identity.Dtos;
using Microsoft.EntityFrameworkCore;

namespace AI.Nova.Server.Api.Features.Identity.Services;

#pragma warning disable NonAsyncEFCoreMethodsUsageAnalyzer

public class IdentitySeeder : IDataSeeder
{
    public async Task SeedAsync(AppDbContext dbContext, CancellationToken cancellationToken)
    {
        if (await dbContext.Roles.AsNoTracking().AnyAsync(cancellationToken) is false)
        {
            var roles = new List<Role>
            {
               new()
               {
                   Id = Guid.Parse("8ff71671-a1d6-5f97-abb9-d87d7b47d6e7"),
                   Name = AppRoles.SuperAdmin,
                   NormalizedName = AppRoles.SuperAdmin.ToUpperInvariant(),
                   ConcurrencyStamp = Guid.NewGuid().ToString()
               },
               new()
               {
                   Id = Guid.Parse("9ff71672-a1d5-4f97-abb7-d87d6b47d5e8"),
                   Name = AppRoles.Demo,
                   NormalizedName = AppRoles.Demo.ToUpperInvariant(),
                   ConcurrencyStamp = Guid.NewGuid().ToString()
               }
            };

            await dbContext.Roles.AddRangeAsync(roles, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        var demoRoleId = Guid.Parse("9ff71672-a1d5-4f97-abb7-d87d6b47d5e8");
        var superAdminRoleId = Guid.Parse("8ff71671-a1d6-5f97-abb9-d87d7b47d6e7");

        if (await dbContext.Users.AsNoTracking().AnyAsync(cancellationToken) is false)
        {
            const string userName = "admin";
            const string email = "761516331@qq.com";

            var adminUser = new User
            {
                Id = Guid.Parse("8ff71671-a1d6-4f97-abb9-d87d7b47d6e7"),
                EmailConfirmed = true,
                LockoutEnabled = true,
                Gender = Gender.Other,
                BirthDate = new DateTimeOffset(new DateOnly(2023, 1, 1), default, default),
                FullName = $"AI.Nova {userName} account",
                UserName = userName,
                NormalizedUserName = userName.ToUpperInvariant(),
                Email = email,
                NormalizedEmail = email.ToUpperInvariant(),
                EmailTokenRequestedOn = new DateTimeOffset(new DateOnly(2000, 1, 1), default, default),
                PhoneNumber = "+3118211338136",
                PhoneNumberConfirmed = true,
                SecurityStamp = "959ff4a9-4b07-4cc1-8141-c5fc033daf83",
                ConcurrencyStamp = "315e1a26-5b3a-4544-8e91-2760cd28e231",
                PasswordHash = "AQAAAAIAAYagAAAAEP0v3wxkdWtMkHA3Pp5/JfS+42/Qto9G05p2mta6dncSK37hPxEHa3PGE4aqN30Aag==", // 123456
            };

            await dbContext.Users.AddAsync(adminUser, cancellationToken);
            await dbContext.UserRoles.AddAsync(new UserRole { RoleId = superAdminRoleId, UserId = adminUser.Id }, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        // Generate 10,000 more users if they don't exist
        var existingUserCount = await dbContext.Users.AsNoTracking().CountAsync(cancellationToken);
        if (existingUserCount < 10000)
        {
            string[] surnames = { "王", "李", "张", "刘", "陈", "杨", "赵", "黄", "周", "吴", "徐", "孙", "胡", "朱", "高", "林", "何", "郭", "马", "罗", "梁", "宋", "郑", "谢", "韩", "唐", "冯", "于", "董", "萧", "程", "曹", "袁", "邓", "许", "傅", "沈", "曾", "彭", "吕" };
            string[] names = { "伟", "杰", "翔", "豪", "敏", "芳", "燕", "娟", "丽", "军", "强", "芳", "英", "华", "飞", "萍", "亮", "军", "强", "兵", "涛", "志", "勇", "丹", "艳", "平", "晨", "宇", "琪", "涵", "馨", "佳", "睿", "子", "轩", "浩", "博", "雅", "梦", "怡", "洁", "慧", "婷", "莉", "博", "悦", "彤", "瑞", "欣", "宇" };

            var batchSize = 1000;
            var usersToCreate = 11000 - existingUserCount;
            var count = 0;

            var passwordHash = "AQAAAAIAAYagAAAAEP0v3wxkdWtMkHA3Pp5/JfS+42/Qto9G05p2mta6dncSK37hPxEHa3PGE4aqN30Aag=="; // 123456

            for (int i = 0; i < usersToCreate; i++)
            {
                var surname = surnames[i % surnames.Length];
                var name1 = names[(i / surnames.Length) % names.Length];
                var name2 = names[(i / (surnames.Length * names.Length)) % names.Length];
                
                var fullName = i % 2 == 0 ? $"{surname}{name1}" : $"{surname}{name1}{name2}";
                var userName = $"user{existingUserCount + i}";
                var email = $"{userName}@example.com";

                var user = new User
                {
                    Id = Guid.NewGuid(),
                    EmailConfirmed = true,
                    LockoutEnabled = true,
                    Gender = (Gender)(i % 3),
                    BirthDate = DateTimeOffset.UtcNow.AddYears(-20 - (i % 30)),
                    FullName = fullName,
                    UserName = userName,
                    NormalizedUserName = userName.ToUpperInvariant(),
                    Email = email,
                    NormalizedEmail = email.ToUpperInvariant(),
                    PhoneNumber = $"+86138{i:D8}",
                    PhoneNumberConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    PasswordHash = passwordHash
                };

                await dbContext.Users.AddAsync(user, cancellationToken);
                await dbContext.UserRoles.AddAsync(new UserRole { RoleId = demoRoleId, UserId = user.Id }, cancellationToken);

                count++;
                if (count >= batchSize)
                {
                    await dbContext.SaveChangesAsync(cancellationToken);
                    count = 0;
                }
            }

            if (count > 0)
            {
                await dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        if (await dbContext.RoleClaims.AsNoTracking().AnyAsync(cancellationToken) is false)
        {
            var id = 1;

            // Unlimited privileged sessions for super admins
            var adminRoleClaim = new RoleClaim
            {
                Id = id++,
                ClaimType = AppClaimTypes.MAX_PRIVILEGED_SESSIONS,
                ClaimValue = "-1",
                RoleId = superAdminRoleId
            };

            await dbContext.RoleClaims.AddAsync(adminRoleClaim, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            // Assign non admin features to demo role
            foreach (var feature in AppFeatures.GetAll()
                .Where(f => f.Group != typeof(AppFeatures.System)
                         && f.Group != typeof(AppFeatures.Management)))
            {
                var roleClaimValue = new RoleClaim
                {
                    Id = id++,
                    ClaimType = AppClaimTypes.FEATURES,
                    ClaimValue = feature.Value,
                    RoleId = demoRoleId
                };

                await dbContext.RoleClaims.AddAsync(roleClaimValue, cancellationToken);
            }
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
