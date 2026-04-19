using AI.Nova.Server.Api.Features.Addresses;
using AI.Nova.Server.Api.Features.AreaCodes;
using AI.Nova.Server.Api.Infrastructure.Data.Seed;
using Microsoft.EntityFrameworkCore;

namespace AI.Nova.Server.Api.Features.Addresses;

#pragma warning disable NonAsyncEFCoreMethodsUsageAnalyzer

public class AddressSeeder : IDataSeeder
{
    public async Task SeedAsync(AppDbContext dbContext, CancellationToken cancellationToken)
    {
        if (await dbContext.Addresses.AnyAsync(cancellationToken))
        {
            return;
        }

        var users = await dbContext.Users.AsNoTracking().OrderBy(u => u.Id).ToListAsync(cancellationToken);
        if (users.Count == 0) return;

        // Since the AreaCodes table is large, we fetch only what we need.
        // We'll pick a sample of Level 5 (Village/Community) entries.
        var level5Entries = await dbContext.AreaCodes
            .AsNoTracking()
            .Where(a => a.Level == 5)
            .ToListAsync(cancellationToken);

        if (level5Entries.Count == 0)
        {
            // Fallback to Level 3 if Level 5 is not available
            level5Entries = await dbContext.AreaCodes
                .AsNoTracking()
                .Where(a => a.Level == 3)
                .ToListAsync(cancellationToken);
        }

        // Collect all related parent codes to fetch them efficiently
        var parentCodes = level5Entries.Select(a => a.Pcode).Where(p => p.HasValue).Cast<long>().Distinct().ToList();
        var allParents = new Dictionary<long, AreaCode>();

        while (parentCodes.Count > 0)
        {
            var currentBatch = await dbContext.AreaCodes
                .AsNoTracking()
                .Where(a => parentCodes.Contains(a.Code))
                .ToListAsync(cancellationToken);
            
            foreach (var parent in currentBatch)
            {
                allParents[parent.Code] = parent;
            }

            parentCodes = currentBatch.Select(a => a.Pcode).Where(p => p.HasValue && !allParents.ContainsKey(p.Value)).Cast<long>().Distinct().ToList();
        }

        var batchSize = 1000;
        var count = 0;

        for (int i = 0; i < users.Count; i++)
        {
            var current = level5Entries[i % level5Entries.Count];
            var chain = new List<AreaCode> { current };
            
            var p = current.Pcode;
            while (p.HasValue && allParents.TryGetValue(p.Value, out var parent))
            {
                chain.Add(parent);
                p = parent.Pcode;
            }

            // chain index: 0=Level5, 1=Level4, 2=Level3, 3=Level2, 4=Level1
            var province = chain.FirstOrDefault(a => a.Level == 1)?.Name ?? "";
            var city = chain.FirstOrDefault(a => a.Level == 2)?.Name ?? "";
            var district = chain.FirstOrDefault(a => a.Level == 3)?.Name ?? "";
            var town = chain.FirstOrDefault(a => a.Level == 4)?.Name ?? "";
            var village = chain.FirstOrDefault(a => a.Level == 5)?.Name ?? "";

            var address = new Address
            {
                Id = Guid.NewGuid(),
                UserId = users[i].Id,
                RecipientName = users[i].FullName,
                PhoneNumber = users[i].PhoneNumber,
                Province = province,
                City = city,
                District = district,
                StreetAddress = $"{town}{village}{i % 1000 + 1}号{i % 10 + 1}单元",
                PostalCode = $"{current.Code}".Substring(0, 6),
                IsDefault = true
            };

            await dbContext.Addresses.AddAsync(address, cancellationToken);

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
}
