using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Pgvector;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AI.Nova.Server.Api.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "jobs");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:vector", ",,");

            migrationBuilder.CreateSequence<int>(
                name: "ProductShortId",
                startValue: 10051L);

            migrationBuilder.CreateTable(
                name: "AreaCodes",
                columns: table => new
                {
                    Code = table.Column<long>(type: "bigint", nullable: false, comment: "行政区划代码 (主键)，遵循国家标准 (如 GB/T 2260)"),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, comment: "行政区域名称"),
                    Level = table.Column<short>(type: "smallint", nullable: true, comment: "行政级别 (1-5): 1=省级, 2=地级, 3=县级, 4=乡级, 5=村级"),
                    Pcode = table.Column<long>(type: "bigint", nullable: true, comment: "父级行政区划代码 (自关联外键)"),
                    Category = table.Column<int>(type: "integer", nullable: true, comment: "城乡分类代码 (如：111-主城区, 121-镇中心区, 220-村庄)"),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录创建时间"),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录创建者ID"),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录最后修改时间"),
                    ModifiedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录最后修改者ID"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: true, comment: "软删除标记：true表示已删除"),
                    DeletedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录删除时间"),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录删除者ID")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AreaCodes", x => x.Code);
                    table.ForeignKey(
                        name: "FK_AreaCode_ParentCode",
                        column: x => x.Pcode,
                        principalTable: "AreaCodes",
                        principalColumn: "Code");
                },
                comment: "行政区划代码表：存储各级行政区域信息及层级关系");

            migrationBuilder.CreateTable(
                name: "Attachments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuidv7()", comment: "主键ID：附件的唯一标识"),
                    Kind = table.Column<int>(type: "integer", nullable: false, comment: "附件类型：0=用户头像小图, 1=用户头像原图, 2=商品中图, 3=商品原图"),
                    Path = table.Column<string>(type: "text", nullable: true, comment: "存储路径：附件在服务器或云存储上的路径"),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录创建时间"),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录创建者ID"),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录最后修改时间"),
                    ModifiedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录最后修改者ID"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: true, comment: "软删除标记：true表示已删除"),
                    DeletedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录删除时间"),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录删除者ID")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachments", x => new { x.Id, x.Kind });
                },
                comment: "附件表：存储系统中的文件引用信息");

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuidv7()", comment: "主键ID：分类的唯一标识"),
                    Name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true, comment: "分类名称：商品的类别名称，最大长度64字符"),
                    Color = table.Column<string>(type: "text", nullable: true, comment: "颜色代码：用于前端展示分类标签的颜色，如 #FF5733"),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false, comment: "版本号：用于乐观并发控制，每次更新自动递增"),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录创建时间"),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录创建者ID"),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录最后修改时间"),
                    ModifiedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录最后修改者ID"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: true, comment: "软删除标记：true表示已删除"),
                    DeletedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录删除时间"),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录删除者ID")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                },
                comment: "商品分类表：用于管理商品的类别和标签");

            migrationBuilder.CreateTable(
                name: "DataProtectionKeys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false, comment: "主键标识符")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FriendlyName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, comment: "密钥的友好名称，用于标识密钥的用途或环境"),
                    Xml = table.Column<string>(type: "text", nullable: true, comment: "密钥的 XML 序列化数据，包含加密密钥材料")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataProtectionKeys", x => x.Id);
                },
                comment: "数据保护系统的密钥存储表：存储 ASP.NET Core DataProtection 的密钥环，用于在服务器重启或集群环境下保持 Cookie 和 Token 有效性");

            migrationBuilder.CreateTable(
                name: "HangfireCounter",
                schema: "jobs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Key = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Value = table.Column<long>(type: "bigint", nullable: false),
                    ExpireAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HangfireCounter", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HangfireHash",
                schema: "jobs",
                columns: table => new
                {
                    Key = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Field = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true),
                    ExpireAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HangfireHash", x => new { x.Key, x.Field });
                });

            migrationBuilder.CreateTable(
                name: "HangfireList",
                schema: "jobs",
                columns: table => new
                {
                    Key = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Position = table.Column<int>(type: "integer", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true),
                    ExpireAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HangfireList", x => new { x.Key, x.Position });
                });

            migrationBuilder.CreateTable(
                name: "HangfireLock",
                schema: "jobs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    AcquiredAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HangfireLock", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HangfireServer",
                schema: "jobs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    StartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Heartbeat = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    WorkerCount = table.Column<int>(type: "integer", nullable: false),
                    Queues = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HangfireServer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HangfireSet",
                schema: "jobs",
                columns: table => new
                {
                    Key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Value = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Score = table.Column<double>(type: "double precision", nullable: false),
                    ExpireAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HangfireSet", x => new { x.Key, x.Value });
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuidv7()"),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录创建时间"),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录创建者ID"),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录最后修改时间"),
                    ModifiedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录最后修改者ID"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: true, comment: "软删除标记：true表示已删除"),
                    DeletedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录删除时间"),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录删除者ID"),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true, comment: "角色名称：如 SuperAdmin, Demo 等"),
                    NormalizedName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true, comment: "标准化名称：用于数据库查询的大写名称"),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true, comment: "并发标记：用于乐观锁控制")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                },
                comment: "角色表：用于系统权限管理的角色定义");

            migrationBuilder.CreateTable(
                name: "SystemPrompts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuidv7()", comment: "系统提示词的唯一标识符 (GUID)"),
                    PromptKind = table.Column<int>(type: "integer", nullable: true, comment: "提示词的类型枚举 (PromptKind)"),
                    Markdown = table.Column<string>(type: "text", nullable: true, comment: "提示词的内容 (Markdown 格式)"),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemPrompts", x => x.Id);
                },
                comment: "系统提示词表：存储 AI 系统提示词及其版本配置的主表");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuidv7()"),
                    FullName = table.Column<string>(type: "text", nullable: true, comment: "用户的全名"),
                    Gender = table.Column<int>(type: "integer", nullable: true, comment: "用户性别"),
                    BirthDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "用户出生日期"),
                    EmailTokenRequestedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "邮箱验证/修改令牌的最后请求时间，用于安全校验"),
                    PhoneNumberTokenRequestedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "手机号验证令牌的最后请求时间"),
                    ResetPasswordTokenRequestedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "重置密码令牌的最后请求时间"),
                    TwoFactorTokenRequestedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "双因素认证 (2FA) 令牌的最后请求时间"),
                    OtpRequestedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "一次性密码 (OTP) 的最后请求时间"),
                    ElevatedAccessTokenRequestedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "高权限访问令牌 (Elevated Access) 的最后请求时间"),
                    HasProfilePicture = table.Column<bool>(type: "boolean", nullable: true, comment: "是否拥有头像标记"),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录创建时间"),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录创建者ID"),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录最后修改时间"),
                    ModifiedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录最后修改者ID"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: true, comment: "软删除标记：true表示已删除"),
                    DeletedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录删除时间"),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录删除者ID"),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true, comment: "用户名"),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true, comment: "标准化用户名 (用于索引和查找)"),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true, comment: "电子邮件地址"),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true, comment: "标准化电子邮件地址"),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false, comment: "邮箱是否已确认"),
                    PasswordHash = table.Column<string>(type: "text", nullable: true, comment: "密码的加盐哈希值"),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true, comment: "安全戳：当用户凭据变更（如改密、删登录）时更改，用于使旧令牌失效"),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true, comment: "并发戳：用于乐观并发控制，每次持久化到数据库时更改"),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true, comment: "电话号码"),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false, comment: "电话号码是否已确认"),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false, comment: "是否启用双因素认证 (2FA)"),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "锁定期结束时间 (UTC)。如果为过去时间或空，表示未锁定"),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false, comment: "是否允许用户被锁定"),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false, comment: "登录失败尝试次数")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                },
                comment: "用户核心表：存储系统用户的账户信息、个人资料及安全凭证。");

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuidv7()", comment: "产品的唯一标识符 (GUID)"),
                    ShortId = table.Column<int>(type: "integer", nullable: true, defaultValueSql: "nextval('\"ProductShortId\"')", comment: "用于生成友好 URL 的短整型 ID"),
                    Name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true, comment: "产品名称 (最大长度 64 字符)"),
                    Price = table.Column<decimal>(type: "numeric(18,3)", precision: 18, scale: 3, nullable: true, comment: "产品价格 (十进制)"),
                    DescriptionHTML = table.Column<string>(type: "character varying(4096)", maxLength: 4096, nullable: true, comment: "产品的 HTML 格式描述"),
                    DescriptionText = table.Column<string>(type: "character varying(4096)", maxLength: 4096, nullable: true, comment: "产品的纯文本描述，用于搜索或摘要"),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: true, comment: "关联的分类 ID"),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false, comment: "行版本号 (用于乐观并发控制)"),
                    HasPrimaryImage = table.Column<bool>(type: "boolean", nullable: true, comment: "标记该产品是否拥有主图"),
                    PrimaryImageAltText = table.Column<string>(type: "text", nullable: true, comment: "主图片的替代文本 (Alt Text)"),
                    Embedding = table.Column<Vector>(type: "vector(768)", nullable: true, comment: "用于语义搜索的向量嵌入 (Pgvector)"),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录创建时间"),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录创建者ID"),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录最后修改时间"),
                    ModifiedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录最后修改者ID"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: true, comment: "软删除标记：true表示已删除"),
                    DeletedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录删除时间"),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录删除者ID")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id");
                },
                comment: "产品核心表：存储电商商品的详细信息、价格策略、多格式描述及用于 AI 语义搜索的向量数据。");

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录创建时间"),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录创建者ID"),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录最后修改时间"),
                    ModifiedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录最后修改者ID"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: true, comment: "软删除标记：true表示已删除"),
                    DeletedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录删除时间"),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录删除者ID"),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false, comment: "关联的角色 ID：外键，指向该声明所属的角色"),
                    ClaimType = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, comment: "声明类型：定义权限的种类，例如 'MaxPrivilegedSessions' 或 'Features'"),
                    ClaimValue = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, comment: "声明值：权限的具体数值或标识，例如 '-1' 代表无限制，或具体的功能代码")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "角色声明表：存储角色关联的声明数据（如权限标识、自定义属性等）。");

            migrationBuilder.CreateTable(
                name: "TodoItems",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false, comment: "主键ID：待办事项的唯一标识"),
                    Title = table.Column<string>(type: "text", nullable: true, comment: "任务标题：待办事项的名称或简短描述"),
                    IsDone = table.Column<bool>(type: "boolean", nullable: true, comment: "完成状态：true表示已完成，false表示未完成"),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true, comment: "关联的用户ID：标识该任务属于哪位用户"),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录创建时间"),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录创建者ID"),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录最后修改时间"),
                    ModifiedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录最后修改者ID"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: true, comment: "软删除标记：true表示已删除"),
                    DeletedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录删除时间"),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录删除者ID")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TodoItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TodoItems_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "待办事项表：存储用户的任务列表");

            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录创建时间"),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录创建者ID"),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录最后修改时间"),
                    ModifiedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录最后修改者ID"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: true, comment: "软删除标记：true表示已删除"),
                    DeletedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录删除时间"),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录删除者ID"),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false, comment: "用户表主键Id：关联Users表Id字段"),
                    ClaimType = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true, comment: "声明的类型（例如：'Permission.Read', 'Department', 'FullName'）"),
                    ClaimValue = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true, comment: "声明的具体值（例如：'Admin', 'HR', 'John Doe'）")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "用户声明表：存储用户特定的声明数据（如权限、自定义属性等。");

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false, comment: "登录提供商名称（例如：'Google', 'Facebook', 'Microsoft'）"),
                    ProviderKey = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false, comment: "提供商端的用户唯一标识符（Provider User ID）"),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录创建时间"),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录创建者ID"),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录最后修改时间"),
                    ModifiedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录最后修改者ID"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: true, comment: "软删除标记：true表示已删除"),
                    DeletedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录删除时间"),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录删除者ID"),
                    ProviderDisplayName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true, comment: "登录提供商的显示名称"),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false, comment: "用户表主键Id：关联Users表Id字段")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "用户外部登录表：存储用户关联的第三方登录提供商信息（如 Google, Microsoft, Facebook 等）。");

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuidv7()", comment: "用户ID（主键的一部分）：关联到 Users 表"),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuidv7()", comment: "角色ID（主键的一部分）：关联到 Roles 表"),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录创建时间"),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录创建者ID"),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录最后修改时间"),
                    ModifiedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录最后修改者ID"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: true, comment: "软删除标记：true表示已删除"),
                    DeletedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录删除时间"),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录删除者ID")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "用户角色关联表：用于实现用户与角色的多对多关系。");

            migrationBuilder.CreateTable(
                name: "UserSessions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuidv7()", comment: "会话的唯一标识符 (主键)"),
                    IP = table.Column<string>(type: "text", nullable: true, comment: "用户会话的 IP 地址"),
                    Address = table.Column<string>(type: "text", nullable: true, comment: "基于 IP 地址解析的地理位置信息"),
                    Privileged = table.Column<bool>(type: "boolean", nullable: true, comment: "特权访问标记：指示该会话是否拥有高权限（如管理员操作）"),
                    StartedOn = table.Column<long>(type: "bigint", nullable: true, comment: "会话开始时间 (Unix 时间戳，单位：秒)"),
                    RenewedOn = table.Column<long>(type: "bigint", nullable: true, comment: "会话最后续期时间 (Unix 时间戳，单位：秒)"),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true, comment: "关联用户的 ID (外键)"),
                    SignalRConnectionId = table.Column<string>(type: "text", nullable: true, comment: "SignalR 连接 ID，用于实时消息推送"),
                    NotificationStatus = table.Column<int>(type: "integer", nullable: true, comment: "推送通知状态：0=未配置, 1=允许, 2=静音"),
                    DeviceInfo = table.Column<string>(type: "text", nullable: true, comment: "设备详细信息（浏览器、操作系统、设备型号等）"),
                    PlatformType = table.Column<int>(type: "integer", nullable: true, comment: "客户端应用平台类型（如：Web, iOS, Android, Windows）"),
                    CultureName = table.Column<string>(type: "text", nullable: true, comment: "用户在该会话中选择的语言文化代码 (如：zh-CN, en-US)"),
                    AppVersion = table.Column<string>(type: "text", nullable: true, comment: "客户端应用程序的版本号"),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录创建时间"),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录创建者ID"),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录最后修改时间"),
                    ModifiedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录最后修改者ID"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: true, comment: "软删除标记：true表示已删除"),
                    DeletedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录删除时间"),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录删除者ID")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSessions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                },
                comment: "用户会话表：记录用户的登录会话信息，用于设备管理、安全审计和推送通知。");

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuidv7()", comment: "用户ID（主键的一部分）：关联到 Users 表"),
                    LoginProvider = table.Column<string>(type: "text", nullable: false, comment: "令牌提供商名称（主键的一部分）：例如 'AspNetCore.Identity' 或 'Google'"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "令牌名称（主键的一部分）：例如 'SecurityStamp' 或 'AccessToken'"),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录创建时间"),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录创建者ID"),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录最后修改时间"),
                    ModifiedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录最后修改者ID"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: true, comment: "软删除标记：true表示已删除"),
                    DeletedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录删除时间"),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录删除者ID"),
                    Value = table.Column<string>(type: "text", nullable: true, comment: "令牌的具体值（敏感数据，通常经过哈希处理或加密）")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "用户令牌表：存储用户的身份验证令牌、刷新令牌或第三方登录令牌。");

            migrationBuilder.CreateTable(
                name: "WebAuthnCredentials",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "bytea", nullable: false, comment: "凭证的唯一标识符 (Credential ID)"),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true, comment: "关联用户的唯一标识符"),
                    PublicKey = table.Column<byte[]>(type: "bytea", nullable: true, comment: "用户的公钥 (COSE Key 格式)，用于验证签名"),
                    SignCount = table.Column<long>(type: "bigint", nullable: true, comment: "签名计数器，用于防止重放攻击"),
                    Transports = table.Column<int[]>(type: "integer[]", nullable: true, comment: "认证器支持的传输方式 (USB, NFC, BLE 等)"),
                    IsBackupEligible = table.Column<bool>(type: "boolean", nullable: true, comment: "指示该凭证是否具备备份资格"),
                    IsBackedUp = table.Column<bool>(type: "boolean", nullable: true, comment: "指示该凭证是否已被备份"),
                    AttestationObject = table.Column<byte[]>(type: "bytea", nullable: true, comment: "证明对象 (Attestation Object) 的原始 CBOR 数据"),
                    AttestationClientDataJson = table.Column<byte[]>(type: "bytea", nullable: true, comment: "证明时的客户端数据 JSON 的原始二进制数据"),
                    UserHandle = table.Column<byte[]>(type: "bytea", nullable: true, comment: "关联到此凭证的用户的标识符"),
                    AttestationFormat = table.Column<string>(type: "text", nullable: true, comment: "证明数据的格式 (如 packed, tpm)"),
                    RegDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "凭证注册的日期和时间"),
                    AaGuid = table.Column<Guid>(type: "uuid", nullable: true, comment: "认证器的 AAGUID，用于识别型号"),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录创建时间"),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录创建者ID"),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录最后修改时间"),
                    ModifiedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录最后修改者ID"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: true, comment: "软删除标记：true表示已删除"),
                    DeletedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录删除时间"),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录删除者ID")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebAuthnCredentials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WebAuthnCredentials_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "WebAuthn凭据表：存储用户的 WebAuthn (FIDO2) 认证凭证，用于实现无密码登录。");

            migrationBuilder.CreateTable(
                name: "PushNotificationSubscriptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false, comment: "订阅记录的主键 ID")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DeviceId = table.Column<string>(type: "text", nullable: true, comment: "设备的唯一标识符 (Device ID)"),
                    Platform = table.Column<string>(type: "text", nullable: true, comment: "推送平台类型：'apns' (Apple), 'fcmV1' (Firebase), 'browser' (Web Push)"),
                    PushChannel = table.Column<string>(type: "text", nullable: true, comment: "推送服务提供的订阅 URL (Endpoint) 或渠道标识"),
                    P256dh = table.Column<string>(type: "text", nullable: true, comment: "Web Push 用户代理公钥 (P-256dh)，用于消息加密"),
                    Auth = table.Column<string>(type: "text", nullable: true, comment: "Web Push 认证密钥 (Auth Secret)，用于生成加密盐"),
                    Endpoint = table.Column<string>(type: "text", nullable: true, comment: "完整的推送消息发送 Endpoint URL"),
                    UserSessionId = table.Column<Guid>(type: "uuid", nullable: true, comment: "关联的用户会话 ID (外键)，用于追踪订阅来源设备"),
                    Tags = table.Column<string[]>(type: "text[]", nullable: true, comment: "订阅标签数组 (JSON)，用于按主题过滤推送消息"),
                    ExpirationTime = table.Column<long>(type: "bigint", nullable: true, comment: "订阅过期时间 (Unix 时间戳，单位：秒)"),
                    RenewedOn = table.Column<long>(type: "bigint", nullable: true, comment: "订阅最后续期时间 (Unix 时间戳，单位：秒)"),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录创建时间"),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录创建者ID"),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录最后修改时间"),
                    ModifiedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录最后修改者ID"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: true, comment: "软删除标记：true表示已删除"),
                    DeletedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录删除时间"),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录删除者ID")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PushNotificationSubscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PushNotificationSubscriptions_UserSessions_UserSessionId",
                        column: x => x.UserSessionId,
                        principalTable: "UserSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                },
                comment: "推送通知订阅表：存储用户设备接收推送通知所需的凭证（如 Endpoint, P256dh, Auth）和订阅状态。");

            migrationBuilder.CreateTable(
                name: "HangfireJob",
                schema: "jobs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    StateId = table.Column<long>(type: "bigint", nullable: true),
                    StateName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ExpireAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    InvocationData = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HangfireJob", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HangfireJobParameter",
                schema: "jobs",
                columns: table => new
                {
                    JobId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HangfireJobParameter", x => new { x.JobId, x.Name });
                    table.ForeignKey(
                        name: "FK_HangfireJobParameter_HangfireJob_JobId",
                        column: x => x.JobId,
                        principalSchema: "jobs",
                        principalTable: "HangfireJob",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HangfireQueuedJob",
                schema: "jobs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    JobId = table.Column<long>(type: "bigint", nullable: false),
                    Queue = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    FetchedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HangfireQueuedJob", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HangfireQueuedJob_HangfireJob_JobId",
                        column: x => x.JobId,
                        principalSchema: "jobs",
                        principalTable: "HangfireJob",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HangfireState",
                schema: "jobs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    JobId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Reason = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Data = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HangfireState", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HangfireState_HangfireJob_JobId",
                        column: x => x.JobId,
                        principalSchema: "jobs",
                        principalTable: "HangfireJob",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "CreatedBy", "CreatedOn", "DeletedBy", "DeletedOn", "IsDeleted", "ModifiedBy", "ModifiedOn", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("8ff71671-a1d6-5f97-abb9-d87d7b47d6e7"), "8ff71671-a1d6-5f97-abb9-d87d7b47d6e7", null, null, null, null, null, null, null, "s-admin", "S-ADMIN" },
                    { new Guid("9ff71672-a1d5-4f97-abb7-d87d6b47d5e8"), "9ff71672-a1d5-4f97-abb7-d87d6b47d5e8", null, null, null, null, null, null, null, "demo", "DEMO" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "BirthDate", "ConcurrencyStamp", "CreatedBy", "CreatedOn", "DeletedBy", "DeletedOn", "ElevatedAccessTokenRequestedOn", "Email", "EmailConfirmed", "EmailTokenRequestedOn", "FullName", "Gender", "HasProfilePicture", "IsDeleted", "LockoutEnabled", "LockoutEnd", "ModifiedBy", "ModifiedOn", "NormalizedEmail", "NormalizedUserName", "OtpRequestedOn", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PhoneNumberTokenRequestedOn", "ResetPasswordTokenRequestedOn", "SecurityStamp", "TwoFactorEnabled", "TwoFactorTokenRequestedOn", "UserName" },
                values: new object[] { new Guid("8ff71671-a1d6-4f97-abb9-d87d7b47d6e7"), 0, new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "315e1a26-5b3a-4544-8e91-2760cd28e231", null, null, null, null, null, "761516331@qq.com", true, new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "AI.Nova test account", 0, null, null, true, null, null, null, "761516331@QQ.COM", "TEST", null, "AQAAAAIAAYagAAAAEP0v3wxkdWtMkHA3Pp5/JfS+42/Qto9G05p2mta6dncSK37hPxEHa3PGE4aqN30Aag==", "+31684207362", true, null, null, "959ff4a9-4b07-4cc1-8141-c5fc033daf83", false, null, "test" });

            migrationBuilder.CreateIndex(
                name: "IX_AreaCodes_Pcode",
                table: "AreaCodes",
                column: "Pcode");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HangfireCounter_ExpireAt",
                schema: "jobs",
                table: "HangfireCounter",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_HangfireCounter_Key_Value",
                schema: "jobs",
                table: "HangfireCounter",
                columns: new[] { "Key", "Value" });

            migrationBuilder.CreateIndex(
                name: "IX_HangfireHash_ExpireAt",
                schema: "jobs",
                table: "HangfireHash",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_HangfireJob_ExpireAt",
                schema: "jobs",
                table: "HangfireJob",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_HangfireJob_StateId",
                schema: "jobs",
                table: "HangfireJob",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_HangfireJob_StateName",
                schema: "jobs",
                table: "HangfireJob",
                column: "StateName");

            migrationBuilder.CreateIndex(
                name: "IX_HangfireList_ExpireAt",
                schema: "jobs",
                table: "HangfireList",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_HangfireQueuedJob_JobId",
                schema: "jobs",
                table: "HangfireQueuedJob",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_HangfireQueuedJob_Queue_FetchedAt",
                schema: "jobs",
                table: "HangfireQueuedJob",
                columns: new[] { "Queue", "FetchedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_HangfireServer_Heartbeat",
                schema: "jobs",
                table: "HangfireServer",
                column: "Heartbeat");

            migrationBuilder.CreateIndex(
                name: "IX_HangfireSet_ExpireAt",
                schema: "jobs",
                table: "HangfireSet",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_HangfireSet_Key_Score",
                schema: "jobs",
                table: "HangfireSet",
                columns: new[] { "Key", "Score" });

            migrationBuilder.CreateIndex(
                name: "IX_HangfireState_JobId",
                schema: "jobs",
                table: "HangfireState",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Name",
                table: "Products",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_ShortId",
                table: "Products",
                column: "ShortId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PushNotificationSubscriptions_UserSessionId",
                table: "PushNotificationSubscriptions",
                column: "UserSessionId",
                unique: true,
                filter: "'UserSessionId' IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId_ClaimType_ClaimValue",
                table: "RoleClaims",
                columns: new[] { "RoleId", "ClaimType", "ClaimValue" });

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Name",
                table: "Roles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Roles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SystemPrompts_PromptKind",
                table: "SystemPrompts",
                column: "PromptKind",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TodoItems_UserId",
                table: "TodoItems",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UserId_ClaimType_ClaimValue",
                table: "UserClaims",
                columns: new[] { "UserId", "ClaimType", "ClaimValue" });

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                table: "UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId_UserId",
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true,
                filter: "'Email' IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PhoneNumber",
                table: "Users",
                column: "PhoneNumber",
                unique: true,
                filter: "'PhoneNumber' IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "Users",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserSessions_UserId",
                table: "UserSessions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WebAuthnCredentials_UserId",
                table: "WebAuthnCredentials",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_HangfireJob_HangfireState_StateId",
                schema: "jobs",
                table: "HangfireJob",
                column: "StateId",
                principalSchema: "jobs",
                principalTable: "HangfireState",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HangfireJob_HangfireState_StateId",
                schema: "jobs",
                table: "HangfireJob");

            migrationBuilder.DropTable(
                name: "AreaCodes");

            migrationBuilder.DropTable(
                name: "Attachments");

            migrationBuilder.DropTable(
                name: "DataProtectionKeys");

            migrationBuilder.DropTable(
                name: "HangfireCounter",
                schema: "jobs");

            migrationBuilder.DropTable(
                name: "HangfireHash",
                schema: "jobs");

            migrationBuilder.DropTable(
                name: "HangfireJobParameter",
                schema: "jobs");

            migrationBuilder.DropTable(
                name: "HangfireList",
                schema: "jobs");

            migrationBuilder.DropTable(
                name: "HangfireLock",
                schema: "jobs");

            migrationBuilder.DropTable(
                name: "HangfireQueuedJob",
                schema: "jobs");

            migrationBuilder.DropTable(
                name: "HangfireServer",
                schema: "jobs");

            migrationBuilder.DropTable(
                name: "HangfireSet",
                schema: "jobs");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "PushNotificationSubscriptions");

            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.DropTable(
                name: "SystemPrompts");

            migrationBuilder.DropTable(
                name: "TodoItems");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "WebAuthnCredentials");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "UserSessions");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "HangfireState",
                schema: "jobs");

            migrationBuilder.DropTable(
                name: "HangfireJob",
                schema: "jobs");

            migrationBuilder.DropSequence(
                name: "ProductShortId");
        }
    }
}
