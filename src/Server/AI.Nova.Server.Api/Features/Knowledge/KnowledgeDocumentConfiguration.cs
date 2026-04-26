using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AI.Nova.Server.Api.Features.Knowledge;

public class KnowledgeDocumentConfiguration : IEntityTypeConfiguration<KnowledgeDocument>
{
    public void Configure(EntityTypeBuilder<KnowledgeDocument> builder)
    {
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Title).IsRequired().HasMaxLength(512);

        builder.HasMany(e => e.Chunks)
               .WithOne(c => c.Document)
               .HasForeignKey(c => c.DocumentId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
