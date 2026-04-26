using AI.Nova.Server.Api.Infrastructure.Data;
using AI.Nova.Shared.Features.Knowledge;
using Microsoft.AspNetCore.Mvc;

namespace AI.Nova.Server.Api.Features.Knowledge;

[ApiVersion(1)]
[ApiController, Route("api/v{v:apiVersion}/[controller]/[action]")]
[Authorize(Policy = AppFeatures.Management.ManageKnowledge)]
public partial class KnowledgeController : AppControllerBase, IKnowledgeController
{
    [AutoInject] private KnowledgeEmbeddingService knowledgeEmbeddingService = default!;

    [HttpGet]
    public async Task<PagedResponse<KnowledgeBaseDto>> GetKnowledgeBases(ODataQueryOptions<KnowledgeBaseDto> odataQuery, CancellationToken cancellationToken)
    {
        var query = (IQueryable<KnowledgeBaseDto>)odataQuery.ApplyTo(DbContext.KnowledgeBases.Project(), ignoreQueryOptions: AllowedQueryOptions.Top | AllowedQueryOptions.Skip);

        var totalCount = await query.LongCountAsync(cancellationToken);

        query = query.SkipIf(odataQuery.Skip is not null, odataQuery.Skip?.Value)
                     .TakeIf(odataQuery.Top is not null, odataQuery.Top?.Value);

        return new PagedResponse<KnowledgeBaseDto>(await query.ToArrayAsync(cancellationToken), totalCount);
    }

    [HttpGet]
    public async Task<PagedResponse<KnowledgeDocumentDto>> GetDocuments(ODataQueryOptions<KnowledgeDocumentDto> odataQuery, CancellationToken cancellationToken)
    {
        var query = (IQueryable<KnowledgeDocumentDto>)odataQuery.ApplyTo(DbContext.KnowledgeDocuments.Project(), ignoreQueryOptions: AllowedQueryOptions.Top | AllowedQueryOptions.Skip);

        var totalCount = await query.LongCountAsync(cancellationToken);

        query = query.SkipIf(odataQuery.Skip is not null, odataQuery.Skip?.Value)
                     .TakeIf(odataQuery.Top is not null, odataQuery.Top?.Value);

        return new PagedResponse<KnowledgeDocumentDto>(await query.ToArrayAsync(cancellationToken), totalCount);
    }

    [HttpGet]
    public async Task<PagedResponse<KnowledgeDocumentChunkDto>> GetChunks(ODataQueryOptions<KnowledgeDocumentChunkDto> odataQuery, CancellationToken cancellationToken)
    {
        var query = (IQueryable<KnowledgeDocumentChunkDto>)odataQuery.ApplyTo(DbContext.KnowledgeDocumentChunks.Project(), ignoreQueryOptions: AllowedQueryOptions.Top | AllowedQueryOptions.Skip);

        var totalCount = await query.LongCountAsync(cancellationToken);

        query = query.SkipIf(odataQuery.Skip is not null, odataQuery.Skip?.Value)
                     .TakeIf(odataQuery.Top is not null, odataQuery.Top?.Value);

        return new PagedResponse<KnowledgeDocumentChunkDto>(await query.ToArrayAsync(cancellationToken), totalCount);
    }

    [HttpPost]
    public async Task<KnowledgeBaseDto> CreateBase(KnowledgeBaseDto dto, CancellationToken cancellationToken)
    {
        var kb = new KnowledgeBase();
        dto.Map(kb);
        await DbContext.KnowledgeBases.AddAsync(kb, cancellationToken);
        await DbContext.SaveChangesAsync(cancellationToken);
        return kb.MapToDto();
    }

    [HttpPut]
    public async Task<KnowledgeBaseDto> UpdateBase(KnowledgeBaseDto dto, CancellationToken cancellationToken)
    {
        var kb = await DbContext.KnowledgeBases.FirstOrDefaultAsync(x => x.Id == dto.Id, cancellationToken);
        if (kb is null) throw new ResourceNotFoundException();
        dto.Patch(kb);
        await DbContext.SaveChangesAsync(cancellationToken);
        return kb.MapToDto();
    }

    [HttpDelete("{id}")]
    public async Task DeleteBase(Guid id, CancellationToken cancellationToken)
    {
        var kb = await DbContext.KnowledgeBases.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (kb is null) throw new ResourceNotFoundException();
        DbContext.KnowledgeBases.Remove(kb);
        await DbContext.SaveChangesAsync(cancellationToken);
    }

    [HttpPost]
    public async Task<KnowledgeDocumentDto> CreateDocument(KnowledgeDocumentDto dto, CancellationToken cancellationToken)
    {
        var doc = new KnowledgeDocument();
        dto.Map(doc);
        await DbContext.KnowledgeDocuments.AddAsync(doc, cancellationToken);
        await DbContext.SaveChangesAsync(cancellationToken);
        return doc.MapToDto();
    }

    [HttpPut]
    public async Task<KnowledgeDocumentDto> UpdateDocument(KnowledgeDocumentDto dto, CancellationToken cancellationToken)
    {
        var doc = await DbContext.KnowledgeDocuments.FirstOrDefaultAsync(x => x.Id == dto.Id, cancellationToken);
        if (doc is null) throw new ResourceNotFoundException();
        dto.Patch(doc);
        await DbContext.SaveChangesAsync(cancellationToken);
        return doc.MapToDto();
    }

    [HttpDelete("{id}")]
    public async Task DeleteDocument(Guid id, CancellationToken cancellationToken)
    {
        var doc = await DbContext.KnowledgeDocuments.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (doc is null) throw new ResourceNotFoundException();
        DbContext.KnowledgeDocuments.Remove(doc);
        await DbContext.SaveChangesAsync(cancellationToken);
    }

    [HttpPost("{knowledgeBaseId}")]
    public async Task UploadDocument(Guid knowledgeBaseId, CancellationToken cancellationToken)
    {
        if (Request.Form.Files.Any() is false)
            throw new BadRequestException("No files uploaded.");

        foreach (var file in Request.Form.Files)
        {
            var docId = Guid.NewGuid();
            
            // Read full content for saving as original
            using var binaryStream = file.OpenReadStream();
            using var ms = new MemoryStream();
            await binaryStream.CopyToAsync(ms, cancellationToken);
            var fileContent = ms.ToArray();

            var doc = new KnowledgeDocument
            {
                Id = docId,
                KnowledgeBaseId = knowledgeBaseId,
                Title = file.FileName,
                Content = fileContent,
                ContentType = file.ContentType
            };
            await DbContext.KnowledgeDocuments.AddAsync(doc, cancellationToken);

            // Read text for chunking
            ms.Position = 0;
            using var reader = new StreamReader(ms, System.Text.Encoding.UTF8);
            var textContent = await reader.ReadToEndAsync();

            if (string.IsNullOrWhiteSpace(textContent)) continue;

            // Smart chunking logic inspired by Semantic Kernel / LangChain
            var chunks = new List<KnowledgeDocumentChunk>();
            var rawChunks = SmartSplit(textContent, file.FileName);
            
            for (int i = 0; i < rawChunks.Count; i++)
            {
                var text = rawChunks[i];
                chunks.Add(new KnowledgeDocumentChunk
                {
                    Id = Guid.NewGuid(),
                    DocumentId = docId,
                    Content = text,
                    TokenCount = (int)Math.Ceiling(text.Length / 4.0),
                    Index = i
                });
            }

            if (chunks.Any())
            {
                await knowledgeEmbeddingService.Embed(chunks, cancellationToken);
                await DbContext.KnowledgeDocumentChunks.AddRangeAsync(chunks, cancellationToken);
            }
        }

        await DbContext.SaveChangesAsync(cancellationToken);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> DownloadDocument(Guid id, CancellationToken cancellationToken)
    {
        var doc = await DbContext.KnowledgeDocuments.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (doc is null || doc.Content is null) throw new ResourceNotFoundException();

        return File(doc.Content, doc.ContentType ?? "application/octet-stream", doc.Title);
    }

    [HttpGet("{knowledgeBaseId}/{searchQuery}/{vectorWeight}")]
    public async Task<PagedResponse<KnowledgeDocumentChunkDto>> SearchChunks(Guid knowledgeBaseId, string searchQuery, double vectorWeight, [FromQuery] string? docName, CancellationToken cancellationToken)
    {
        var chunks = await knowledgeEmbeddingService.SearchChunks(knowledgeBaseId, searchQuery, (float)vectorWeight, docName, cancellationToken);
        
        var mappedChunks = chunks.Select(c => c.MapToDto()).ToArray();

        return new PagedResponse<KnowledgeDocumentChunkDto>(mappedChunks, mappedChunks.Length);
    }

    private List<string> SmartSplit(string content, string fileName)
    {
        var isMarkdown = fileName.EndsWith(".md", StringComparison.OrdinalIgnoreCase) || 
                         fileName.EndsWith(".markdown", StringComparison.OrdinalIgnoreCase);

        var list = new List<string>();
        if (!isMarkdown)
        {
            // Simple sliding window for non-markdown files
            var chunkSize = 1000;
            var overlap = 200;
            for (int i = 0; i < content.Length; i += (chunkSize - overlap))
            {
                var length = Math.Min(chunkSize, content.Length - i);
                list.Add(content.Substring(i, length));
                if (i + length >= content.Length) break;
            }
            return list;
        }

        // Markdown Smart Split
        // 1. Split by headers (H1-H4) or common separators
        var separators = new[] { "\n# ", "\n## ", "\n### ", "\n#### ", "\n---", "\n<!-- chunk-split -->" };
        var rawPieces = content.Split(separators, StringSplitOptions.RemoveEmptyEntries);
        
        var currentChunk = new System.Text.StringBuilder();
        var maxChunkSize = 1500; // Prefer larger chunks for contextual richness in semantic search

        foreach (var piece in rawPieces)
        {
            if (currentChunk.Length + piece.Length > maxChunkSize && currentChunk.Length > 0)
            {
                list.Add(currentChunk.ToString().Trim());
                currentChunk.Clear();
            }
            currentChunk.AppendLine(piece);
        }
        
        if (currentChunk.Length > 0)
        {
            list.Add(currentChunk.ToString().Trim());
        }

        return list;
    }
}
