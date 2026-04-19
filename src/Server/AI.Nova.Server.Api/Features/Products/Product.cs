using AI.Nova.Server.Api.Features.Categories;
using AI.Nova.Server.Api.Infrastructure.Data.Audit;

namespace AI.Nova.Server.Api.Features.Products;

/// <summary>
/// 产品核心表：存储电商商品的详细信息、价格策略、多格式描述及用于 AI 语义搜索的向量数据。
/// </summary>
[Table("Products")]
[Comment("产品核心表：存储电商商品的详细信息、价格策略、多格式描述及用于 AI 语义搜索的向量数据。")]
public partial class Product : AuditEntity
{
    /// <summary>
    /// 产品的唯一标识符 (主键)。
    /// </summary>
    [Key] // 如果未使用 EF Core 约定，建议显式标注
    [Comment("产品的唯一标识符 (GUID)")]
    public Guid Id { get; set; }

    /// <summary>
    /// 产品的短 ID，用于创建更人性化的 URL。
    /// </summary>
    [Range(0, int.MaxValue)]
    [Comment("用于生成友好 URL 的短整型 ID")]
    public int ShortId { get; set; }

    /// <summary>
    /// 产品名称。
    /// </summary>
    [MaxLength(64)]
    [Comment("产品名称 (最大长度 64 字符)")]
    public string? Name { get; set; }

    /// <summary>
    /// 产品价格。
    /// </summary>
    [Range(0, double.MaxValue)]
    [Comment("产品价格 (十进制)")]
    public decimal? Price { get; set; }

    /// <summary>
    /// 产品详细描述 (HTML 格式)。
    /// </summary>
    [MaxLength(4096)]
    [Comment("产品的 HTML 格式描述")]
    public string? DescriptionHTML { get; set; }

    /// <summary>
    /// 产品详细描述 (纯文本格式)。
    /// </summary>
    [MaxLength(4096)]
    [Comment("产品的纯文本描述，用于搜索或摘要")]
    public string? DescriptionText { get; set; }

    /// <summary>
    /// 导航属性：关联的分类。
    /// </summary>
    [ForeignKey(nameof(CategoryId))]
    [Comment("关联的分类 ID")]
    public Guid? CategoryId { get; set; }

    /// <summary>
    /// 导航属性：关联的分类对象。
    /// </summary>
    [Comment("关联的分类实体")]
    public Category? Category { get; set; }

    /// <summary>
    /// 行版本号，用于乐观并发控制。
    /// </summary>
    [Timestamp] // 如果是用于并发控制，建议加上此特性
    [Comment("行版本号 (用于乐观并发控制)")]
    public long Version { get; set; }

    /// <summary>
    /// 是否包含主图。
    /// </summary>
    [Comment("标记该产品是否拥有主图")]
    public bool? HasPrimaryImage { get; set; } = false;

    /// <summary>
    /// 主图的替代文本 (用于无障碍访问或加载失败时显示)。
    /// </summary>
    [Comment("主图片的替代文本 (Alt Text)")]
    public string? PrimaryImageAltText { get; set; }

    /// <summary>
    /// 产品的向量嵌入，用于 AI 语义搜索。
    /// </summary>
    [Comment("用于语义搜索的向量嵌入 (Pgvector)")]
    public Pgvector.Vector? Embedding { get; set; }
}
