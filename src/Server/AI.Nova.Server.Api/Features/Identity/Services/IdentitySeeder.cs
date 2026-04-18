using AI.Nova.Server.Api.Features.Identity.Models;
using AI.Nova.Server.Api.Infrastructure.Data.Seed;
using AI.Nova.Shared.Features.Identity.Dtos;

namespace AI.Nova.Server.Api.Features.Identity.Services;

public class IdentitySeeder : IDataSeeder
{
    public async Task SeedAsync(AppDbContext dbContext, CancellationToken cancellationToken)
    {
        if (await dbContext.Roles.AnyAsync(cancellationToken) is false)
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

        if (await dbContext.Users.AnyAsync(cancellationToken) is false)
        {
            const string userName = "admin";
            const string email = "761516331@qq.com";

            var user = new User
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

            await dbContext.Users.AddAsync(user, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        if (await dbContext.UserRoles.AnyAsync(cancellationToken) is false)
        {
            var superAdminRoleId = Guid.Parse("8ff71671-a1d6-5f97-abb9-d87d7b47d6e7");
            var defaultTestUserId = Guid.Parse("8ff71671-a1d6-4f97-abb9-d87d7b47d6e7");

            var userRole = new UserRole { RoleId = superAdminRoleId, UserId = defaultTestUserId };
            await dbContext.UserRoles.AddAsync(userRole, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        if (await dbContext.RoleClaims.AnyAsync(cancellationToken) is false)
        {
            var id = 1;

            // Unlimited privileged sessions for super admins
            var superAdminRoleId = Guid.Parse("8ff71671-a1d6-5f97-abb9-d87d7b47d6e7");
            var superAdminRoleClaim = new RoleClaim
            {
                Id = id++,
                ClaimType = AppClaimTypes.MAX_PRIVILEGED_SESSIONS,
                ClaimValue = "-1",
                RoleId = superAdminRoleId
            };

            await dbContext.RoleClaims.AddAsync(superAdminRoleClaim, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            // Assign non admin features to demo role
            var demoRoleId = Guid.Parse("9ff71672-a1d5-4f97-abb7-d87d6b47d5e8");
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
                await dbContext.SaveChangesAsync(cancellationToken);
            }
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
