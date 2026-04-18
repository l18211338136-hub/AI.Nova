using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;

namespace AI.Nova.Server.Api.Infrastructure.Data.Configurations;

/// <summary>
/// 配置 ASP.NET Core DataProtection 密钥存储实体
/// </summary>
public class DataProtectionKeyConfiguration : IEntityTypeConfiguration<DataProtectionKey>
{
    public void Configure(EntityTypeBuilder<DataProtectionKey> builder)
    {
        // 1. 配置表名和表注释
        builder.ToTable("DataProtectionKeys", t =>
        {
            t.HasComment("数据保护系统的密钥存储表：存储 ASP.NET Core DataProtection 的密钥环，用于在服务器重启或集群环境下保持 Cookie 和 Token 有效性");
        });

        // 2. 配置主键
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd() // 数据库自增
            .HasComment("主键标识符");

        // 3. 配置友好名称
        builder.Property(x => x.FriendlyName)
            .HasMaxLength(255) // 建议设置最大长度，避免数据库生成 nvarchar(max) 或 text
            .HasComment("密钥的友好名称，用于标识密钥的用途或环境");

        // 4. 配置 XML 数据
        // 注意：DataProtection 存储的是 XML 格式的字符串。
        // 在 PostgreSQL 中通常映射为 text，SQL Server 中为 nvarchar(max)。
        builder.Property(x => x.Xml)
            .HasColumnType("text") // 显式指定类型，确保能存下长文本
            .HasComment("密钥的 XML 序列化数据，包含加密密钥材料");
    }
}
