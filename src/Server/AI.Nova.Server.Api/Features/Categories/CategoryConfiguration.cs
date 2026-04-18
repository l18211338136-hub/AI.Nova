using AngleSharp.Dom;

namespace AI.Nova.Server.Api.Features.Categories;

public partial class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");
        builder.HasIndex(p => p.Name).IsUnique();
    }
}

