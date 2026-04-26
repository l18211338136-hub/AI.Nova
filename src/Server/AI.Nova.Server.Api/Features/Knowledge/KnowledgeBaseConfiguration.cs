using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AI.Nova.Server.Api.Features.Knowledge;

public class KnowledgeBaseConfiguration : IEntityTypeConfiguration<KnowledgeBase>
{
    public void Configure(EntityTypeBuilder<KnowledgeBase> builder)
    {
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Name).IsRequired().HasMaxLength(256);
        builder.Property(e => e.Description).HasMaxLength(2000);

        builder.HasMany(e => e.Documents)
               .WithOne(d => d.KnowledgeBase)
               .HasForeignKey(d => d.KnowledgeBaseId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
