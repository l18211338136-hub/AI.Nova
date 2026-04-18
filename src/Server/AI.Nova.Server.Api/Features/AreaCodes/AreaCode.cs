using AI.Nova.Server.Api.Infrastructure.Data.Audit;

namespace AI.Nova.Server.Api.Features.AreaCodes;

/// <summary>
/// 行政区划实体。
/// 用于存储国家、省、市、县、乡镇及村级区域的层级数据。
/// 继承自 AuditEntity，包含创建/修改审计信息。
/// </summary>
[Table("AreaCodes")]
[Comment("行政区划代码表：存储各级行政区域信息及层级关系")]
public class AreaCode : AuditEntity
{
    /// <summary>
    /// 区划代码（主键）。
    /// 通常为 6 位或 12 位数字（如：110000 北京市）。
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)] // 通常区划代码由外部标准定义，非自增
    [Comment("行政区划代码 (主键)，遵循国家标准 (如 GB/T 2260)")]
    public long Code { get; set; }

    /// <summary>
    /// 行政区域名称。
    /// 例如：北京市、朝阳区。
    /// </summary>
    [MaxLength(255)] // 建议添加长度限制
    [Comment("行政区域名称")]
    public string? Name { get; set; }

    /// <summary>
    /// 行政级别。
    /// 1:省级, 2:地级, 3:县级, 4:乡级, 5:村级
    /// </summary>
    [Range(1, 5)]
    [Comment("行政级别 (1-5): 1=省级, 2=地级, 3=县级, 4=乡级, 5=村级")]
    public short? Level { get; set; }

    /// <summary>
    /// 父级区划代码。
    /// 用于构建树形层级结构（自关联外键）。
    /// </summary>
    [ForeignKey(nameof(Pcode))]
    [Comment("父级行政区划代码 (自关联外键)")]
    public long? Pcode { get; set; }

    /// <summary>
    /// 城乡分类代码。
    /// 标识该区域属于城市、镇还是乡村（如：111=主城区, 220=村庄）。
    /// </summary>
    [Comment("城乡分类代码 (如：111-主城区, 121-镇中心区, 220-村庄)")]
    public int? Category { get; set; }
}
