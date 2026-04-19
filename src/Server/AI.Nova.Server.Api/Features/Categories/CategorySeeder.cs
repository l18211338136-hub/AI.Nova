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
            new() { Id = Guid.Parse("747f6d66-7524-40ca-8494-f65e85b5ee5d"), Name = "特斯拉", Color = "#2B88D8", Version = defaultVersion },
            new() { Id = Guid.Parse("4dd0801a-8534-4a47-a461-8276f75355a2"), Name = "索尼", Color = "#363636", Version = defaultVersion },
            new() { Id = Guid.Parse("194e9f54-d8f9-467a-9a99-9b4e6097d75a"), Name = "微软", Color = "#F25022", Version = defaultVersion },
            new() { Id = Guid.Parse("2658a2d3-13ff-4e78-bc48-aa16d7a5e840"), Name = "苹果", Color = "#A2AAAD", Version = defaultVersion },
            new() { Id = Guid.Parse("5a7d6e1b-2c3d-4e5f-a6b7-c8d9e0f1a2b3"), Name = "耐克", Color = "#111111", Version = defaultVersion },
            new() { Id = Guid.Parse("b6d4e5f8-c2a1-4321-9a8b-7c6d5e4f3a2b"), Name = "阿迪达斯", Color = "#008DCE", Version = defaultVersion },
            new() { Id = Guid.Parse("c5e3d1b9-a2f4-4b5c-8d7e-6f1a0b2c3d4e"), Name = "天梭", Color = "#E31837", Version = defaultVersion },
            new() { Id = Guid.Parse("a4b6c8d0-e1f2-4321-b9a8-d7c6f5e43210"), Name = "戴尔", Color = "#007DB8", Version = defaultVersion },
            new() { Id = Guid.Parse("e0f2a4b6-c8d1-4321-9a8b-f7e5d3c1b0a9"), Name = "宏碁", Color = "#83B81A", Version = defaultVersion },
            new() { Id = Guid.Parse("f5e7d9c1-b3a5-4e7a-9c2d-1a4b6c8d0e2f"), Name = "奥克利", Color = "#662D91", Version = defaultVersion },
            new() { Id = Guid.Parse("d2c4b6a8-e0f1-4321-9a8b-c7e5d3f1a0b2"), Name = "乐高", Color = "#D11013", Version = defaultVersion },
            new() { Id = Guid.Parse("c1d2e3f4-a5b6-7890-4321-f0e9d8c7b6a5"), Name = "匡威", Color = "#000000", Version = defaultVersion },
            new() { Id = Guid.Parse("f0e9d8c7-b6a5-4321-8901-a2b3c4d5e6f7"), Name = "设计师家具", Color = "#8B4513", Version = defaultVersion },
            new() { Id = Guid.Parse("b1c2d3e4-f5a6-4321-8901-23456789abcd"), Name = "综合书籍", Color = "#607D8B", Version = defaultVersion },
            new() { Id = Guid.Parse("a1b2c3d4-e5f6-4a1c-1e5a-7890abcdef12"), Name = "精品钟表", Color = "#FFD700", Version = defaultVersion },
            new() { Id = Guid.Parse("5e6f7a8b-9c0d-4e1f-2a3b-4c5d6e7f8a9b"), Name = "体育健身", Color = "#4CAF50", Version = defaultVersion },
            new() { Id = Guid.Parse("7a8b9c0d-e1f2-4321-b9a8-d7c6f5e43210"), Name = "时尚服饰", Color = "#FF4081", Version = defaultVersion }
        };

        await dbContext.Categories.AddRangeAsync(categories);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
