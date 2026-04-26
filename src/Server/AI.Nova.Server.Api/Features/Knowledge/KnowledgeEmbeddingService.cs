using Pgvector.EntityFrameworkCore;
using AI.Nova.Server.Api.Infrastructure.Data;

namespace AI.Nova.Server.Api.Features.Knowledge;

public partial class KnowledgeEmbeddingService
{
    [AutoInject] private IHostEnvironment env = default!;
    [AutoInject] private AppDbContext dbContext = default!;
    [AutoInject] private IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator = default!;

    public async Task Embed(KnowledgeDocumentChunk chunk, CancellationToken cancellationToken)
    {
        if (AppDbContext.IsEmbeddingEnabled is false && env.IsDevelopment())
            return;

        if (string.IsNullOrWhiteSpace(chunk.Content)) return;

        var embeddedResponse = await embeddingGenerator.GenerateAsync(chunk.Content, cancellationToken: cancellationToken);
        chunk.Embedding = new Pgvector.Vector(embeddedResponse.Vector);
    }

    public async Task Embed(IEnumerable<KnowledgeDocumentChunk> chunks, CancellationToken cancellationToken)
    {
        if (AppDbContext.IsEmbeddingEnabled is false && env.IsDevelopment())
            return;

        var chunksToEmbed = chunks.Where(c => !string.IsNullOrWhiteSpace(c.Content)).ToList();
        if (!chunksToEmbed.Any()) return;

        var texts = chunksToEmbed.Select(c => c.Content!).ToArray();
        var embeddingsResponse = await embeddingGenerator.GenerateAsync(texts, cancellationToken: cancellationToken);

        for (int i = 0; i < chunksToEmbed.Count; i++)
        {
            chunksToEmbed[i].Embedding = new Pgvector.Vector(embeddingsResponse[i].Vector);
        }
    }

    public async Task<List<KnowledgeDocumentChunk>> SearchChunks(Guid knowledgeBaseId, string searchQuery, float vectorWeight, string? docName, CancellationToken cancellationToken)
    {
        var keywordWeight = 1.0f - vectorWeight;

        // 1. 并行准备：生成向量的同时，准备数据库的关键字查询 (提升响应速度)
        var vectorTask = embeddingGenerator.GenerateAsync(searchQuery, cancellationToken: cancellationToken);
        
        // 2. 双路召回 - 路径 A：关键词召回 (先走数据库索引)
        var keywordCandidatesTask = dbContext.KnowledgeDocumentChunks
            .Include(c => c.Document)
            .AsNoTracking()
            .Where(c => c.Document!.KnowledgeBaseId == knowledgeBaseId && EF.Functions.Like(c.Content!, $"%{searchQuery}%"))
            .WhereIf(string.IsNullOrWhiteSpace(docName) is false, c => c.Document!.Title == docName)
            .OrderByDescending(c => c.Id)
            .Take(50)
            .ToListAsync(cancellationToken);

        // 处理生成的向量
        var embeddedSearchQuery = await vectorTask;
        var queryVector = new Pgvector.Vector(embeddedSearchQuery.Vector);

        // 2. 双路召回 - 路径 B：向量语义召回
        var vectorCandidatesTask = dbContext.KnowledgeDocumentChunks
            .Include(c => c.Document)
            .AsNoTracking()
            .Where(c => c.Document!.KnowledgeBaseId == knowledgeBaseId && c.Embedding != null)
            .WhereIf(string.IsNullOrWhiteSpace(docName) is false, c => c.Document!.Title == docName)
            .OrderBy(c => c.Embedding!.CosineDistance(queryVector))
            .Take(50)
            .ToListAsync(cancellationToken);

        // 等待所有召回完成
        await Task.WhenAll(keywordCandidatesTask, vectorCandidatesTask);

        // 3. 结果合并与去重 (融合两路候选人)
        var candidatesDict = keywordCandidatesTask.Result.ToDictionary(c => c.Id);
        foreach (var vCandidate in vectorCandidatesTask.Result)
        {
            candidatesDict.TryAdd(vCandidate.Id, vCandidate);
        }

        var candidates = candidatesDict.Values.ToList();

        // 4. 重排序 (进行加权复合评分)
        var results = candidates.Select(c =>
        {
            var vectorSimilarity = (c.Embedding != null) 
                ? (1.0 - CalculateCosineDistance(c.Embedding, queryVector)) 
                : 0.0;
            
            var keywordScore = ComputeKeywordScore(c.Content ?? "", searchQuery);

            c.Score = (vectorSimilarity * vectorWeight) + (keywordScore * keywordWeight);
            
            return c;
        })
        .OrderByDescending(c => c.Score)
        .Take(50) // 返回前 50 条精排结果
        .ToList();

        return results;
    }

    private double CalculateCosineDistance(Pgvector.Vector v1, Pgvector.Vector v2)
    {
        var arr1 = v1.ToArray();
        var arr2 = v2.ToArray();
        
        if (arr1.Length != arr2.Length) return 1.0;

        float dotProduct = 0;
        float mag1 = 0;
        float mag2 = 0;

        for (int i = 0; i < arr1.Length; i++)
        {
            dotProduct += arr1[i] * arr2[i];
            mag1 += arr1[i] * arr1[i];
            mag2 += arr2[i] * arr2[i];
        }

        if (mag1 == 0 || mag2 == 0) return 1.0;

        return 1.0 - (dotProduct / (Math.Sqrt(mag1) * Math.Sqrt(mag2)));
    }

    private double ComputeKeywordScore(string content, string query)
    {
        if (string.IsNullOrWhiteSpace(query) || string.IsNullOrWhiteSpace(content)) return 0;

        // 1. 预处理：转小写、分词并去重复
        var separators = new[] { ' ', ',', '.', '!', '?', ';', ':', '，', '。', '！', '？', '；', '：' };
        var queryTokens = query.ToLower().Split(separators, StringSplitOptions.RemoveEmptyEntries).Distinct().ToList();
        var contentLower = content.ToLower();

        if (!queryTokens.Any()) return 0;

        double matchedTokens = 0;
        double frequencyScore = 0;

        foreach (var token in queryTokens)
        {
            int count = 0;
            int index = contentLower.IndexOf(token);
            while (index != -1)
            {
                count++;
                index = contentLower.IndexOf(token, index + token.Length);
            }

            if (count > 0)
            {
                matchedTokens++;
                // 词频贡献采用非线性增长，防止单个词出现次数过多导致评分失真
                frequencyScore += Math.Log10(count + 9); 
            }
        }

        // 2. 基础分：关键词覆盖率 (占 70% 权重)
        var coverageScore = matchedTokens / queryTokens.Count;

        // 3. 词频分：关键词在文中出现的频率密度 (占 30% 权重)
        var avgFrequencyScore = Math.Min(frequencyScore / queryTokens.Count, 1.0);

        var finalScore = (coverageScore * 0.7) + (avgFrequencyScore * 0.3);

        // 4. 短语奖励：如果内容完整包含查询短语，额外奖励分数
        if (contentLower.Contains(query.ToLower().Trim()))
        {
            finalScore = Math.Min(finalScore + 0.15, 1.0);
        }

        return Math.Round(finalScore, 4);
    }
}
