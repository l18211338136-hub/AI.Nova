using AI.Nova.Shared.Features.Chatbot;

namespace AI.Nova.Server.Api.Features.Chatbot;

/// <summary>
/// 实体：系统提示词配置。
/// 用于存储和管理不同场景下的 AI 系统提示词。
/// </summary>
[Table("SystemPrompts")] // 显式定义表名
[Comment("系统提示词表：存储 AI 系统提示词及其版本配置的主表")] // 数据库表的备注
public class SystemPrompt
{
    /// <summary>
    /// 系统提示词的唯一标识符 (主键)。
    /// </summary>
    [Key]
    [Comment("系统提示词的唯一标识符 (GUID)")]
    public Guid Id { get; set; }

    // <summary>
    /// 提示词的类型或分类 (例如：总结、翻译、代码生成等)。
    /// </summary>
    [Comment("提示词的类型枚举 (PromptKind)")]
    public PromptKind? PromptKind { get; set; }

    /// <summary>
    /// 提示词的具体内容，使用 Markdown 格式。
    /// </summary>
    [Comment("提示词的内容 (Markdown 格式)")]
    public string? Markdown { get; set; }

    public long Version { get; set; }
}
