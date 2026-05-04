using JiebaNet.Segmenter;
using Pgvector.EntityFrameworkCore;

namespace AI.Nova.Server.Api.Features.Knowledge;

public partial class KnowledgeEmbeddingService
{
    [AutoInject] private IHostEnvironment env = default!;
    [AutoInject] private AppDbContext dbContext = default!;
    [AutoInject] private IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator = default!;
    [AutoInject] private ILogger<KnowledgeEmbeddingService> logger = default!;

    private bool _jiebaInitialized = false;
    private bool _jiebaInitializationAttempted = false;

    public async Task Embed(KnowledgeDocumentChunk chunk, CancellationToken cancellationToken)
    {
        if (AppDbContext.IsEmbeddingEnabled is false && env.IsDevelopment())
            return;

        if (string.IsNullOrWhiteSpace(chunk.Content)) return;

        try
        {
            var textToEmbed = PrepareTextForEmbedding(chunk.Content);

            var embeddedResponse = await embeddingGenerator.GenerateAsync(textToEmbed, cancellationToken: cancellationToken);

            if (embeddedResponse.Vector.Length != 768)
            {
                logger.LogWarning("Embedding vector length mismatch. Expected: 768, Actual: {Length}",
                    embeddedResponse.Vector.Length);
            }

            chunk.Embedding = new Pgvector.Vector(embeddedResponse.Vector);

            logger.LogInformation("Successfully embedded chunk {ChunkId}, content length: {Length}, embedded length: {EmbeddedLength}",
                chunk.Id, chunk.Content?.Length, textToEmbed.Length);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error embedding chunk {ChunkId}, content length: {Length}", chunk.Id, chunk.Content?.Length);

            if (ex is HttpRequestException || ex is TimeoutException)
            {
                logger.LogWarning("Network error occurred, will retry embedding chunk {ChunkId}", chunk.Id);
                await RetryEmbeddingAsync(chunk, cancellationToken);
            }
        }
    }

    public async Task Embed(IEnumerable<KnowledgeDocumentChunk> chunks, CancellationToken cancellationToken)
    {
        if (AppDbContext.IsEmbeddingEnabled is false && env.IsDevelopment())
            return;

        var chunksToEmbed = chunks.Where(c => !string.IsNullOrWhiteSpace(c.Content)).ToList();
        if (!chunksToEmbed.Any()) return;

        // 分批处理 (每批 5 条)
        var batchSize = 5;
        for (int i = 0; i < chunksToEmbed.Count; i += batchSize)
        {
            var batch = chunksToEmbed.Skip(i).Take(batchSize).ToList();
            var texts = batch.Select(c => c.Content!.Length > 2000 ? c.Content![..2000] : c.Content!).ToArray();

            try
            {
                var embeddingsResponse = await embeddingGenerator.GenerateAsync(texts, cancellationToken: cancellationToken);

                for (int j = 0; j < batch.Count; j++)
                {
                    batch[j].Embedding = new Pgvector.Vector(embeddingsResponse[j].Vector);
                }
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Error embedding batch starting at index {Index}. Falling back to individual embedding.", i);
                
                // 批处理失败时，尝试逐条处理该批次
                foreach (var chunk in batch)
                {
                    await Embed(chunk, cancellationToken);
                }
            }
        }
    }

    public async Task<List<KnowledgeDocumentChunk>> SearchChunks(Guid knowledgeBaseId, string searchQuery, float vectorWeight, string? docName, CancellationToken cancellationToken)
    {
        var keywordWeight = 1.0f - vectorWeight;

        // 使用 Jieba 分词器对查询进行分词
        var queryTokens = TokenizeQuery(searchQuery);
        var originalQuery = searchQuery;

        // 1. 并行准备：生成向量的同时，准备数据库的关键字查询
        Pgvector.Vector? queryVector = null;
        var vectorTask = Task.Run(async () =>
        {
            try
            {
                var result = await embeddingGenerator.GenerateAsync(searchQuery, cancellationToken: cancellationToken);
                queryVector = new Pgvector.Vector(result.Vector);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error generating embedding for search query: {Query}", searchQuery);
            }
        }, cancellationToken);
        
        // 2. 双路召回 - 路径 A：关键词召回（改进版）
        var keywordCandidatesTask = GetKeywordCandidates(knowledgeBaseId, queryTokens, originalQuery, docName, cancellationToken);

        await vectorTask;
        
        // 2. 双路召回 - 路径 B：向量语义召回 (仅当向量生成成功时)
        List<KnowledgeDocumentChunk> vectorCandidates = [];
        if (queryVector != null)
        {
            vectorCandidates = await dbContext.KnowledgeDocumentChunks
                .Include(c => c.Document)
                .AsNoTracking()
                .Where(c => c.Document!.KnowledgeBaseId == knowledgeBaseId && c.Embedding != null)
                .WhereIf(string.IsNullOrWhiteSpace(docName) is false, c => c.Document!.Title == docName)
                .OrderBy(c => c.Embedding!.CosineDistance(queryVector))
                .Take(50)
                .ToListAsync(cancellationToken);
        }

        // 等待所有召回完成
        var keywordCandidates = await keywordCandidatesTask;

        // 3. 结果合并与去重
        var candidatesDict = keywordCandidates.ToDictionary(c => c.Id);
        foreach (var vCandidate in vectorCandidates)
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
            
            var keywordScore = ComputeKeywordScore(c.Content ?? "", queryTokens, originalQuery);

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

    private List<string> TokenizeQuery(string query)
    {
        if (string.IsNullOrWhiteSpace(query)) return [];

        var tokens = new List<string>();
        
        // 尝试使用 Jieba 分词器
        if (TryInitializeJieba())
        {
            try
            {
                var jiebaTokens = GetJiebaSegmenter().Cut(query, cutAll: false);
                
                // 过滤掉停用词和单字（除非是数字或特殊字符）
                var stopWords = new HashSet<string> { "的", "了", "在", "是", "我", "有", "和", "就", "不", "人", "都", "一", "一个", "上", "也", "很", "到", "说", "要", "去", "你", "会", "着", "没有", "看", "好", "自己", "这" };
                
                foreach (var token in jiebaTokens)
                {
                    var trimmedToken = token.Trim();
                    
                    // 跳过空字符串和停用词
                    if (string.IsNullOrWhiteSpace(trimmedToken) || stopWords.Contains(trimmedToken))
                        continue;
                    
                    // 保留多字词、数字、英文单词
                    if (trimmedToken.Length > 1 || char.IsDigit(trimmedToken[0]) || char.IsLetter(trimmedToken[0]))
                    {
                        tokens.Add(trimmedToken);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Jieba 分词失败，回退到简单分词: {Query}", query);
            }
        }
        
        // 如果 Jieba 分词失败或没有结果，使用简单分词
        if (!tokens.Any())
        {
            tokens = SimpleTokenize(query);
        }
        
        return tokens.Distinct().ToList();
    }

    private bool TryInitializeJieba()
    {
        if (_jiebaInitializationAttempted)
        {
            return _jiebaInitialized;
        }

        _jiebaInitializationAttempted = true;

        try
        {
            // 尝试创建 Jieba 分词器实例
            var segmenter = new JiebaSegmenter();
            
            // 测试分词是否正常工作
            var testResult = segmenter.Cut("测试", cutAll: false);
            
            if (testResult != null && testResult.Any())
            {
                _jiebaInitialized = true;
                logger.LogInformation("Jieba 分词器初始化成功");
                return true;
            }
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Jieba 分词器初始化失败，将使用简单分词方式");
        }

        return false;
    }

    private JiebaNet.Segmenter.JiebaSegmenter GetJiebaSegmenter()
    {
        return new JiebaNet.Segmenter.JiebaSegmenter();
    }

    private List<string> SimpleTokenize(string query)
    {
        var tokens = new List<string>();
        
        // 简单分词：按空格、标点符号分割
        var separators = new[] { ' ', ',', '.', '!', '?', ';', ':', '，', '。', '！', '？', '；', '：', '\n', '\r', '\t' };
        var parts = query.Split(separators, StringSplitOptions.RemoveEmptyEntries);
        
        foreach (var part in parts)
        {
            var trimmedPart = part.Trim();
            if (string.IsNullOrWhiteSpace(trimmedPart)) continue;
            
            // 对于中文，尝试提取连续的中文字符
            var chineseChars = new List<char>();
            var otherChars = new List<char>();
            
            foreach (var c in trimmedPart)
            {
                if (c >= 0x4E00 && c <= 0x9FFF)
                {
                    chineseChars.Add(c);
                }
                else
                {
                    otherChars.Add(c);
                }
            }
            
            // 添加中文词（2个或更多连续中文字符）
            if (chineseChars.Count >= 2)
            {
                tokens.Add(new string(chineseChars.ToArray()));
            }
            else if (chineseChars.Count == 1)
            {
                tokens.Add(chineseChars[0].ToString());
            }
            
            // 添加英文单词或数字
            if (otherChars.Count > 0)
            {
                var otherText = new string(otherChars.ToArray()).Trim();
                if (!string.IsNullOrWhiteSpace(otherText))
                {
                    tokens.Add(otherText);
                }
            }
        }
        
        return tokens;
    }

    private async Task<List<KnowledgeDocumentChunk>> GetKeywordCandidates(
        Guid knowledgeBaseId, 
        List<string> queryTokens, 
        string originalQuery, 
        string? docName, 
        CancellationToken cancellationToken)
    {
        if (!queryTokens.Any())
        {
            return [];
        }

        var candidates = new List<KnowledgeDocumentChunk>();
        
        // 优先匹配完整短语
        var phraseMatches = await dbContext.KnowledgeDocumentChunks
            .Include(c => c.Document)
            .AsNoTracking()
            .Where(c => c.Document!.KnowledgeBaseId == knowledgeBaseId && 
                        EF.Functions.Like(c.Content!, $"%{originalQuery}%"))
            .WhereIf(string.IsNullOrWhiteSpace(docName) is false, c => c.Document!.Title == docName)
            .OrderByDescending(c => c.Id)
            .Take(30)
            .ToListAsync(cancellationToken);
        
        candidates.AddRange(phraseMatches);
        
        // 如果短语匹配结果不足，补充关键词匹配
        if (candidates.Count < 50)
        {
            var remainingCount = 50 - candidates.Count;
            var candidateIds = candidates.Select(c => c.Id).ToHashSet();
            
            // 构建关键词匹配条件
            var keywordQuery = dbContext.KnowledgeDocumentChunks
                .Include(c => c.Document)
                .AsNoTracking()
                .Where(c => c.Document!.KnowledgeBaseId == knowledgeBaseId);
            
            if (string.IsNullOrWhiteSpace(docName) is false)
            {
                keywordQuery = keywordQuery.Where(c => c.Document!.Title == docName);
            }
            
            // 对每个关键词进行模糊匹配
            var keywordMatches = new List<KnowledgeDocumentChunk>();
            foreach (var token in queryTokens.Take(5)) // 限制最多5个关键词避免性能问题
            {
                var matches = await keywordQuery
                    .Where(c => EF.Functions.Like(c.Content!, $"%{token}%"))
                    .Where(c => !candidateIds.Contains(c.Id))
                    .OrderByDescending(c => c.Id)
                    .Take(remainingCount)
                    .ToListAsync(cancellationToken);
                
                keywordMatches.AddRange(matches);
                
                // 更新已匹配的ID集合
                foreach (var match in matches)
                {
                    candidateIds.Add(match.Id);
                }
                
                if (keywordMatches.Count >= remainingCount)
                    break;
            }
            
            candidates.AddRange(keywordMatches.DistinctBy(c => c.Id));
        }
        
        return candidates.Take(50).ToList();
    }

    private double ComputeKeywordScore(string content, List<string> queryTokens, string originalQuery)
    {
        if (!queryTokens.Any() || string.IsNullOrWhiteSpace(content)) return 0;

        var contentLower = content.ToLower();
        var originalQueryLower = originalQuery.ToLower().Trim();
        
        // 1. 完整短语匹配奖励（最高优先级）
        if (contentLower.Contains(originalQueryLower))
        {
            return 1.0;
        }

        double matchedTokens = 0;
        double frequencyScore = 0;
        double proximityScore = 0;
        var matchedPositions = new List<int>();

        foreach (var token in queryTokens)
        {
            var tokenLower = token.ToLower();
            int count = 0;
            int index = contentLower.IndexOf(tokenLower);
            
            while (index != -1)
            {
                count++;
                matchedPositions.Add(index);
                index = contentLower.IndexOf(tokenLower, index + tokenLower.Length);
            }

            if (count > 0)
            {
                matchedTokens++;
                // 词频贡献采用对数缩放，防止单个词出现次数过多导致评分失真
                frequencyScore += Math.Log10(count + 9); 
            }
        }

        if (matchedTokens == 0) return 0;

        // 2. 基础分：关键词覆盖率 (占 50% 权重)
        var coverageScore = matchedTokens / queryTokens.Count;

        // 3. 词频分：关键词在文中出现的频率密度 (占 30% 权重)
        var avgFrequencyScore = Math.Min(frequencyScore / queryTokens.Count, 1.0);

        // 4. 邻近度分：关键词在文中出现的紧凑程度 (占 20% 权重)
        if (matchedPositions.Count > 1)
        {
            matchedPositions.Sort();
            var totalDistance = 0;
            for (int i = 1; i < matchedPositions.Count; i++)
            {
                totalDistance += matchedPositions[i] - matchedPositions[i - 1];
            }
            var avgDistance = (double)totalDistance / (matchedPositions.Count - 1);
            // 距离越小，分数越高（最大100字符内为满分）
            proximityScore = Math.Max(0, 1.0 - (avgDistance / 100.0));
        }
        else
        {
            proximityScore = 0.5; // 单个匹配给予中等分数
        }

        var finalScore = (coverageScore * 0.5) + (avgFrequencyScore * 0.3) + (proximityScore * 0.2);

        return Math.Round(finalScore, 4);
    }

    private string PrepareTextForEmbedding(string content)
    {
        if (string.IsNullOrWhiteSpace(content)) return string.Empty;

        var maxLength = GetOptimalMaxLengthFor768(content);

        var processedContent = content
            .Trim()
            .Replace("\r\n", "\n")
            .Replace("\t", " ")
            .Replace("  ", " ");

        if (processedContent.Length <= maxLength)
            return processedContent;

        var truncatedContent = processedContent[..maxLength];

        var lastNewLine = truncatedContent.LastIndexOf('\n');
        if (lastNewLine > maxLength * 0.8)
        {
            return truncatedContent[..lastNewLine].Trim();
        }

        var sentenceEndChars = new[] { '.', '!', '?', '。', '！', '？' };
        var lastSentence = truncatedContent.LastIndexOfAny(sentenceEndChars);
        if (lastSentence > maxLength * 0.7)
        {
            var endIndex = Math.Min(lastSentence + 1, truncatedContent.Length);
            return truncatedContent[..endIndex].Trim();
        }

        return truncatedContent.Trim();
    }

    private int GetOptimalMaxLengthFor768(string content)
    {
        var contentLength = content.Length;

        return contentLength switch
        {
            < 1000 => 1200,
            < 2000 => 1800,
            < 5000 => 2500,
            < 10000 => 3500,
            _ => 4500
        };
    }
    private async Task RetryEmbeddingAsync(KnowledgeDocumentChunk chunk, CancellationToken cancellationToken, int maxRetries = 3)
    {
        for (int attempt = 1; attempt <= maxRetries; attempt++)
        {
            try
            {
                await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, attempt - 1)), cancellationToken);

                var textToEmbed = PrepareTextForEmbedding(chunk.Content);
                var embeddedResponse = await embeddingGenerator.GenerateAsync(textToEmbed, cancellationToken: cancellationToken);

                if (embeddedResponse.Vector.Length != 768)
                {
                    logger.LogWarning("Retry {Attempt}: Embedding vector length mismatch. Expected: 768, Actual: {Length}",
                        attempt, embeddedResponse.Vector.Length);
                }

                chunk.Embedding = new Pgvector.Vector(embeddedResponse.Vector);

                logger.LogInformation("Successfully embedded chunk {ChunkId} on attempt {Attempt}", chunk.Id, attempt);
                return;
            }
            catch (Exception ex) when (attempt < maxRetries)
            {
                logger.LogWarning(ex, "Attempt {Attempt} failed for chunk {ChunkId}, retrying...", attempt, chunk.Id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "All {MaxRetries} attempts failed for chunk {ChunkId}", maxRetries, chunk.Id);
                throw;
            }
        }
    }
}
