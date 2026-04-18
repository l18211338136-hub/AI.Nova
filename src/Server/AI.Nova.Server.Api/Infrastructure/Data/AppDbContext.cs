using System.Linq.Expressions;
using AI.Nova.Server.Api.Features.AreaCodes;
using AI.Nova.Server.Api.Features.Attachments;
using AI.Nova.Server.Api.Features.Categories;
using AI.Nova.Server.Api.Features.Identity.Models;
using AI.Nova.Server.Api.Features.Products;
using AI.Nova.Server.Api.Features.PushNotification;
using AI.Nova.Server.Api.Features.Todo;
using AI.Nova.Server.Api.Infrastructure.Data.Audit;
using AI.Nova.Server.Api.Infrastructure.Data.Configurations;
using AI.Nova.Server.Api.Infrastructure.Services.Contracts;
using Hangfire.EntityFrameworkCore;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace AI.Nova.Server.Api.Infrastructure.Data;

public partial class AppDbContext(DbContextOptions<AppDbContext> options, ICurrentUserService currentUserService)
    : IdentityDbContext<User, Role, Guid, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>(options), IDataProtectionKeyContext
{
    public DbSet<UserSession> UserSessions { get; set; } = default!;

    public DbSet<TodoItem> TodoItems { get; set; } = default!;
    public DbSet<Category> Categories { get; set; } = default!;
    public DbSet<Product> Products { get; set; } = default!;
    public DbSet<PushNotificationSubscription> PushNotificationSubscriptions { get; set; } = default!;

    public DbSet<WebAuthnCredential> WebAuthnCredential { get; set; } = default!;

    public DbSet<SystemPrompt> SystemPrompts { get; set; } = default!;

    public DbSet<Attachment> Attachments { get; set; } = default!;
    public DbSet<AreaCode> AreaCodes { get; set; } = default!;

    public DbSet<DataProtectionKey> DataProtectionKeys { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

            if (IsEmbeddingEnabled)
            {
                modelBuilder.HasPostgresExtension("vector");
            }

        modelBuilder.OnHangfireModelCreating("jobs");

        modelBuilder.HasSequence<int>("ProductShortId")
            .StartsAt(10_051) // There are 50 products added by ProductConfiguration.cs
            .IncrementsBy(1);


        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        ConfigureIdentityTableNames(modelBuilder);

        ConfigureConcurrencyToken(modelBuilder);

        ConfigureRowVersion(modelBuilder);

        ApplySoftDeleteQueryFilters(modelBuilder);
    }

    private void ApplySoftDeleteQueryFilters(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
            {
                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(ConvertFilterExpression<ISoftDelete>(e => e.IsDeleted != true, entityType.ClrType));
            }
        }
    }

    private static LambdaExpression ConvertFilterExpression<TInterface>(Expression<Func<TInterface, bool>> filterExpression, Type entityType)
    {
        var parameter = Expression.Parameter(entityType);
        var body = ReplacingExpressionVisitor.Replace(filterExpression.Parameters[0], parameter, filterExpression.Body);
        return Expression.Lambda(body, parameter);
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        try
        {
            OnSavingChanges();

#pragma warning disable NonAsyncEFCoreMethodsUsageAnalyzer
            return base.SaveChanges(acceptAllChangesOnSuccess);
#pragma warning restore NonAsyncEFCoreMethodsUsageAnalyzer
        }
        catch (DbUpdateConcurrencyException exception)
        {
            throw new ConflictException(nameof(AppStrings.UpdateConcurrencyException), exception);
        }
    }

    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
    {
        try
        {
            OnSavingChanges();

            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
        catch (DbUpdateConcurrencyException exception)
        {
            throw new ConflictException(nameof(AppStrings.UpdateConcurrencyException), exception);
        }
    }

    private void OnSavingChanges()
    {
        ChangeTracker.DetectChanges();

        var userId = currentUserService.GetCurrentUserId();
        var now = DateTimeOffset.UtcNow;

        foreach (var entry in ChangeTracker.Entries<IAuditableEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedOn = now;
                entry.Entity.CreatedBy = userId;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.ModifiedOn = now;
                entry.Entity.ModifiedBy = userId;
            }
        }

        foreach (var entry in ChangeTracker.Entries<ISoftDelete>())
        {
            if (entry.State == EntityState.Deleted)
            {
                entry.State = EntityState.Unchanged;
                entry.Entity.IsDeleted = true;
                entry.Property(nameof(ISoftDelete.IsDeleted)).IsModified = true;

                entry.Entity.DeletedOn = now;
                entry.Property(nameof(ISoftDelete.DeletedOn)).IsModified = true;

                entry.Entity.DeletedBy = userId;
                entry.Property(nameof(ISoftDelete.DeletedBy)).IsModified = true;

                // Also update ModifiedOn for soft delete
                if (entry.Entity is IAuditableEntity auditable)
                {
                    auditable.ModifiedOn = now;
                    entry.Property(nameof(IAuditableEntity.ModifiedOn)).IsModified = true;

                    auditable.ModifiedBy = userId;
                    entry.Property(nameof(IAuditableEntity.ModifiedBy)).IsModified = true;
                }
            }
        }

        foreach (var entityEntry in ChangeTracker.Entries().Where(e => e.State is EntityState.Modified or EntityState.Deleted))
        {
            // https://github.com/dotnet/efcore/issues/35443
            if (entityEntry.Properties.Any(p => p.Metadata.Name == "Version") && entityEntry.CurrentValues["Version"] is long currentVersion)
                entityEntry.OriginalValues["Version"] = currentVersion;
        }
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {

            configurationBuilder.Conventions.Add(_ => new PostgreSQLPrimaryKeySequentialGuidDefaultValueConvention());
            // PostgreSQL does not support DateTimeOffset with offset other than Utc.
            configurationBuilder.Properties<DateTimeOffset>().HaveConversion<PostgresDateTimeOffsetConverter>();
            configurationBuilder.Properties<DateTimeOffset?>().HaveConversion<NullablePostgresDateTimeOffsetConverter>();


        configurationBuilder.Properties<decimal>().HavePrecision(18, 3);
        configurationBuilder.Properties<decimal?>().HavePrecision(18, 3);

        base.ConfigureConventions(configurationBuilder);
    }

    private void ConfigureIdentityTableNames(ModelBuilder builder)
    {
        builder.Entity<User>()
            .ToTable("Users");

        builder.Entity<Role>()
            .ToTable("Roles");

        builder.Entity<UserRole>()
            .ToTable("UserRoles");

        builder.Entity<RoleClaim>()
            .ToTable("RoleClaims");

        builder.Entity<UserClaim>()
            .ToTable("UserClaims");

        builder.Entity<UserLogin>()
            .ToTable("UserLogins");

        builder.Entity<UserToken>()
            .ToTable("UserTokens");
    }

    private void ConfigureConcurrencyToken(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {

            foreach (var property in entityType.GetProperties()
                .Where(p => p.Name is "Version" && p.PropertyInfo?.PropertyType == typeof(long)))
            {
                var builder = new PropertyBuilder(property);
                builder.IsConcurrencyToken();
            }
        }
    }

    private void ConfigureRowVersion(ModelBuilder modelBuilder)
    {

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {

            foreach (var property in entityType.GetProperties()
                .Where(p => p.Name is "Version" && p.PropertyInfo?.PropertyType == typeof(long)))
            {
                var builder = new PropertyBuilder(property);

                builder.IsRowVersion();

                builder.HasConversion<uint>();
            }
        }
    }

    // In order to enable embedding, the `pgvector` extension must be installed in your PostgreSQL.
    // The following command runs the postgreSQL container with the `pgvector` extension:
    // docker run -d --name postgres -e POSTGRES_USER=postgres -e POSTGRES_PASSWORD=postgres -e POSTGRES_DB=AI.NovaDb -p 5432:5432 -v pgdata:/var/lib/postgresql --restart unless-stopped pgvector/pgvector:pg18
    public static readonly bool IsEmbeddingEnabled = true;
}
