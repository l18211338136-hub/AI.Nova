using Riok.Mapperly.Abstractions;
using AI.Nova.Shared.Features.Knowledge;

namespace AI.Nova.Server.Api.Features.Knowledge;

[Mapper]
public static partial class KnowledgeMapper
{
    public static partial IQueryable<KnowledgeBaseDto> Project(this IQueryable<KnowledgeBase> q);
    public static partial IQueryable<KnowledgeDocumentDto> Project(this IQueryable<KnowledgeDocument> q);
    public static partial IQueryable<KnowledgeDocumentChunkDto> Project(this IQueryable<KnowledgeDocumentChunk> q);
    
    public static partial KnowledgeBaseDto MapToDto(this KnowledgeBase kb);
    public static partial KnowledgeDocumentDto MapToDto(this KnowledgeDocument doc);
    [MapProperty(nameof(KnowledgeDocumentChunk.Document.Title), nameof(KnowledgeDocumentChunkDto.DocumentTitle))]
    public static partial KnowledgeDocumentChunkDto MapToDto(this KnowledgeDocumentChunk chunk);
    
    public static partial void Map(this KnowledgeBaseDto source, KnowledgeBase target);
    public static partial void Map(this KnowledgeDocumentDto source, KnowledgeDocument chunk);
    public static partial void Map(this KnowledgeDocumentChunkDto source, KnowledgeDocumentChunk target);
    
    [MapperIgnoreTarget(nameof(KnowledgeBase.Id))]
    public static partial void Patch(this KnowledgeBaseDto source, KnowledgeBase target);
    
    [MapperIgnoreTarget(nameof(KnowledgeDocument.Id))]
    public static partial void Patch(this KnowledgeDocumentDto source, KnowledgeDocument target);

    [MapperIgnoreTarget(nameof(KnowledgeDocumentChunk.Id))]
    public static partial void Patch(this KnowledgeDocumentChunkDto source, KnowledgeDocumentChunk target);
}
