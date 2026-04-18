
namespace AI.Nova.Server.Api.Features.Products;

public partial class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasIndex(p => p.Name).IsUnique();
        builder.HasIndex(p => p.ShortId).IsUnique();

        builder.Property(p => p.ShortId).UseSequence("ProductShortId");
        if (AppDbContext.IsEmbeddingEnabled)
        {
            builder.Property(p => p.Embedding).HasColumnType("vector(768)"); // Dimensions depend on the model used, here assuming 384 because of LocalTextEmbeddingGenerationService.
        }
        else
        {
            builder.Ignore(p => p.Embedding);
        }
    }
}

