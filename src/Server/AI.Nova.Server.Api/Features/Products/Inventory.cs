using AI.Nova.Server.Api.Infrastructure.Data.Audit;

namespace AI.Nova.Server.Api.Features.Products;

/// <summary>
/// 商品库存管理表：维护商品实时库存容量，处理预估销量占用及低库存预警提醒。
/// </summary>
[Table("Inventories")]
[Comment("商品库存表：维护商品实时库存、占用库存及库存报警阈值。")]
public partial class Inventory : AuditEntity
{
    /// <summary>
    /// 库存记录唯一标识。
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Comment("库存记录唯一标识")]
    public Guid Id { get; set; }

    /// <summary>
    /// 导航属性：关联的商品。
    /// </summary>
    [ForeignKey(nameof(ProductId))]
    [Comment("所属的商品对象")]
    public Product? Product { get; set; }

    /// <summary>
    /// 外键：商品 ID。
    /// </summary>
    [Comment("关联的商品 ID")]
    public Guid? ProductId { get; set; }

    /// <summary>
    /// 仓库中的实际可用库存数量。
    /// </summary>
    [Comment("当前真实的可用库存余量")]
    public int? StockQuantity { get; set; }

    /// <summary>
    /// 已锁定但尚未支付的预占库存数量。
    /// </summary>
    [Comment("已下单但未支付的冻结/预占库存数量")]
    public int? ReservedQuantity { get; set; }

    /// <summary>
    /// 触发系统低库存提醒的临界数值。
    /// </summary>
    [Comment("触发低库存自动提醒的阈值高度")]
    public int? LowStockThreshold { get; set; }
}
