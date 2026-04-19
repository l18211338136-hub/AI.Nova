using AI.Nova.Server.Api.Features.Products;
using AI.Nova.Server.Api.Infrastructure.Data.Seed;
using Microsoft.EntityFrameworkCore;

namespace AI.Nova.Server.Api.Features.Products;

#pragma warning disable NonAsyncEFCoreMethodsUsageAnalyzer

public class InventorySeeder : IDataSeeder
{
    public async Task SeedAsync(AppDbContext dbContext, CancellationToken cancellationToken)
    {
        if (await dbContext.Inventories.AsNoTracking().AnyAsync(cancellationToken))
        {
            return;
        }

        var products = await dbContext.Products.AsNoTracking().ToListAsync(cancellationToken);
        
        foreach (var product in products)
        {
            await dbContext.Inventories.AddAsync(new Inventory
            {
                Id = Guid.NewGuid(),
                ProductId = product.Id,
                StockQuantity = 1000,
                ReservedQuantity = 0,
                LowStockThreshold = 10
            }, cancellationToken);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
