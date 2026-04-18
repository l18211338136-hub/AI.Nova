using AI.Nova.Server.Api.Infrastructure.Data.Audit;
using AI.Nova.Shared.Features.Attachments;

namespace AI.Nova.Server.Api.Features.Attachments;

[Table("Attachments")]
[Comment("附件表：存储系统中的文件引用信息")]
public partial class Attachment : AuditEntity
{
    [Key]
    [Comment("主键ID：附件的唯一标识")]
    public Guid Id { get; set; }

    [Comment("附件类型：0=用户头像小图, 1=用户头像原图, 2=商品中图, 3=商品原图")]
    public AttachmentKind? Kind { get; set; }

    [Comment("存储路径：附件在服务器或云存储上的路径")]
    public string? Path { get; set; }
}
