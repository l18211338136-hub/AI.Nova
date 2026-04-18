using AI.Nova.Server.Api.Features.Identity.Models;
using AI.Nova.Server.Api.Infrastructure.Data.Audit;

namespace AI.Nova.Server.Api.Features.Todo;


[Comment("待办事项表：存储用户的任务列表")]
public partial class TodoItem : AuditEntity
{
    [Key]
    [Comment("主键ID：待办事项的唯一标识")]
    public string Id { get; set; }

    [Comment("任务标题：待办事项的名称或简短描述")]
    public string? Title { get; set; }

    [Comment("完成状态：true表示已完成，false表示未完成")]
    public bool? IsDone { get; set; }

    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }

    [Comment("关联的用户ID：标识该任务属于哪位用户")]
    public Guid? UserId { get; set; }
}
