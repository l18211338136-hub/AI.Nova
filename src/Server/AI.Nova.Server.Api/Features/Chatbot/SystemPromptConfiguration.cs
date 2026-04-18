
using AI.Nova.Shared.Features.Chatbot;

namespace AI.Nova.Server.Api.Features.Chatbot;

public class SystemPromptConfiguration : IEntityTypeConfiguration<SystemPrompt>
{
    public void Configure(EntityTypeBuilder<SystemPrompt> builder)
    {
        builder.HasIndex(sp => sp.PromptKind)
            .IsUnique();
    }
}
