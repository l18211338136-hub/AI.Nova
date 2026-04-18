namespace AI.Nova.Server.Api.Infrastructure.Data.Seed;

public interface IDataSeeder
{
    Task SeedAsync(AppDbContext dbContext, CancellationToken cancellationToken);
}
