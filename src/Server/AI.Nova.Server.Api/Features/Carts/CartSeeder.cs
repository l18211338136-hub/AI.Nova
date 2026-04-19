using AI.Nova.Server.Api.Infrastructure.Data.Seed;

namespace AI.Nova.Server.Api.Features.Carts;

#pragma warning disable NonAsyncEFCoreMethodsUsageAnalyzer

public class CartSeeder : IDataSeeder
{
    public async Task SeedAsync(AppDbContext dbContext, CancellationToken cancellationToken)
    {
        if (await dbContext.CartItems.AsNoTracking().AnyAsync(cancellationToken))
        {
            return;
        }

        var users = await dbContext.Users.AsNoTracking().Take(100).ToListAsync(cancellationToken);
        var products = await dbContext.Products.AsNoTracking().Take(10).ToListAsync(cancellationToken);

        if (users.Count == 0 || products.Count == 0) return;

        foreach (var user in users)
        {
            // Each of the first 100 users gets some items in their cart
            var randomProducts = products.OrderBy(x => Guid.NewGuid()).Take(2).ToList();
            foreach (var product in randomProducts)
            {
                await dbContext.CartItems.AddAsync(new CartItem
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    ProductId = product.Id,
                    Quantity = (new Random().Next(1, 4)),
                    Selected = true,
                    CreatedOn = DateTimeOffset.UtcNow,
                    CreatedBy = Guid.Parse("8ff71671-a1d6-4f97-abb9-d87d7b47d6e7"),
                }, cancellationToken);
            }
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
