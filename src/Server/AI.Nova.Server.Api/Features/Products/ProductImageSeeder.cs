using AI.Nova.Server.Api.Features.Products;
using AI.Nova.Server.Api.Infrastructure.Data.Seed;
using Microsoft.EntityFrameworkCore;

namespace AI.Nova.Server.Api.Features.Products;

#pragma warning disable NonAsyncEFCoreMethodsUsageAnalyzer

public class ProductImageSeeder : IDataSeeder
{
    public async Task SeedAsync(AppDbContext dbContext, CancellationToken cancellationToken)
    {
        if (await dbContext.ProductImages.AsNoTracking().AnyAsync(cancellationToken))
        {
            return;
        }

        var products = await dbContext.Products.AsNoTracking().ToListAsync(cancellationToken);

        foreach (var product in products)
        {
            await dbContext.ProductImages.AddRangeAsync(new List<ProductImage>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    ProductId = product.Id,
                    ImageUrl = $"https://picsum.photos/seed/{product.Id}/800/600",
                    AltText = $"{product.Name} - 主图",
                    IsPrimary = true,
                    SortOrder = 1
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    ProductId = product.Id,
                    ImageUrl = $"https://picsum.photos/seed/{product.Id}_2/800/600",
                    AltText = $"{product.Name} - 细节图 1",
                    IsPrimary = false,
                    SortOrder = 2
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    ProductId = product.Id,
                    ImageUrl = $"https://picsum.photos/seed/{product.Id}_3/800/600",
                    AltText = $"{product.Name} - 细节图 2",
                    IsPrimary = false,
                    SortOrder = 3
                }
            }, cancellationToken);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
