using AI.Nova.Server.Api.Infrastructure.Data.Audit;
using AI.Nova.Shared.Features.Attachments;

namespace AI.Nova.Server.Api.Features.Attachments;

[Table("Attachments")]
[Comment("附件表")]
public partial class Attachment : AuditEntity
{
    [Key]
    [Comment("主键")]
    public Guid Id { get; set; }

    [Comment("附件类型：0：用户头像小图,；1：用户头像原图,；2：商品中图,；3：商品原图")]
    public AttachmentKind? Kind { get; set; }

    [Comment("附件路径")]
    public string? Path { get; set; }
}
