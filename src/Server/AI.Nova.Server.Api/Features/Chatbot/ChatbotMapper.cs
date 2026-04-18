using Riok.Mapperly.Abstractions;
using AI.Nova.Shared.Features.Chatbot;

namespace AI.Nova.Server.Api.Features.Chatbot;

/// <summary>
/// More info at Server/Mappers/README.md
/// </summary>
[Mapper]
public static partial class ChatbotMapper
{
    public static partial IQueryable<SystemPromptDto> Project(this IQueryable<SystemPrompt> query);

    public static partial SystemPromptDto Map(this SystemPrompt source);
    public static partial SystemPrompt Map(this SystemPromptDto source);
    public static partial void Patch(this SystemPromptDto source, SystemPrompt destination);
}
