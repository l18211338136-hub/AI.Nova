using AI.Nova.Shared.Features.Knowledge;
using AI.Nova.Shared.Infrastructure.Dtos;

namespace AI.Nova.Shared.Features.Knowledge;

[Route("api/v1/[controller]/[action]/"), AuthorizedApi]
public interface IKnowledgeController : IAppController
{
    [HttpGet]
    Task<PagedResponse<KnowledgeBaseDto>> GetKnowledgeBases(CancellationToken cancellationToken = default) => default!;

    [HttpGet]
    Task<PagedResponse<KnowledgeDocumentDto>> GetDocuments(CancellationToken cancellationToken = default) => default!;

    [HttpGet]
    Task<PagedResponse<KnowledgeDocumentChunkDto>> GetChunks(CancellationToken cancellationToken = default) => default!;

    [HttpPost]
    Task<KnowledgeBaseDto> CreateBase(KnowledgeBaseDto dto, CancellationToken cancellationToken = default) => default!;

    [HttpPut]
    Task<KnowledgeBaseDto> UpdateBase(KnowledgeBaseDto dto, CancellationToken cancellationToken = default) => default!;

    [HttpDelete("{id}")]
    Task DeleteBase(Guid id, CancellationToken cancellationToken = default) => default!;

    [HttpPost("{knowledgeBaseId}")]
    Task UploadDocument(Guid knowledgeBaseId, CancellationToken cancellationToken = default) => default!;

    [HttpPut]
    Task<KnowledgeDocumentDto> UpdateDocument(KnowledgeDocumentDto dto, CancellationToken cancellationToken = default) => default!;

    [HttpDelete("{id}")]
    Task DeleteDocument(Guid id, CancellationToken cancellationToken = default) => default!;

    [HttpGet("{id}")]
    Task DownloadDocument(Guid id, CancellationToken cancellationToken = default) => default!;

    [HttpGet("{knowledgeBaseId}/{searchQuery}/{vectorWeight}{?docName}")]
    Task<PagedResponse<KnowledgeDocumentChunkDto>> SearchChunks(Guid knowledgeBaseId, string searchQuery, double vectorWeight, string? docName = null, CancellationToken cancellationToken = default) => default!;
}
