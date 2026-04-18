using AI.Nova.Server.Api.Infrastructure.Data.Seed;

namespace AI.Nova.Server.Api.Features.Categories;

public class CategorySeeder : IDataSeeder
{
    public async Task SeedAsync(AppDbContext dbContext, CancellationToken cancellationToken)
    {
        if (await dbContext.Categories.AnyAsync(cancellationToken))
        {
            return;
        }

        var defaultVersion = 1;

        List<Category> categories = new List<Category>()
        {
            new() { Id = Guid.Parse("31d78bd0-0b4f-4e87-b02f-8f66d4ab2845"), Name = "福特", Color = "#FFCD56", Version = defaultVersion },
            new() { Id = Guid.Parse("582b8c19-0709-4dae-b7a6-fa0e704dad3c"), Name = "日产", Color = "#FF6384", Version = defaultVersion },
            new() { Id = Guid.Parse("6fae78f3-b067-40fb-a2d5-9c8dd5eb2e08"), Name = "奔驰", Color = "#4BC0C0", Version = defaultVersion },
            new() { Id = Guid.Parse("ecf0496f-f1e3-4d92-8fe4-0d7fa2b4ffa4"), Name = "宝马", Color = "#FF9124", Version = defaultVersion },
            new() { Id = Guid.Parse("747f6d66-7524-40ca-8494-f65e85b5ee5d"), Name = "特斯拉", Color = "#2B88D8", Version = defaultVersion }
        };

        await dbContext.Categories.AddRangeAsync(categories);
        await dbContext.SaveChangesAsync();
    }
}
