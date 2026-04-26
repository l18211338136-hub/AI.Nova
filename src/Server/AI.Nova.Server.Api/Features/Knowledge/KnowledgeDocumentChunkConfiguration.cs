using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pgvector.EntityFrameworkCore;
using AI.Nova.Server.Api.Infrastructure.Data;

namespace AI.Nova.Server.Api.Features.Knowledge;

public class KnowledgeDocumentChunkConfiguration : IEntityTypeConfiguration<KnowledgeDocumentChunk>
{
    public void Configure(EntityTypeBuilder<KnowledgeDocumentChunk> builder)
    {
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Content).IsRequired();

        if (AppDbContext.IsEmbeddingEnabled)
        {
            builder.Property(e => e.Embedding).HasColumnType("vector(768)");
        }
        else
        {
            builder.Ignore(p => p.Embedding);
        }
    }
}
