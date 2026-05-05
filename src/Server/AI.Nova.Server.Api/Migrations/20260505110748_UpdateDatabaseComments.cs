using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Pgvector;

#nullable disable

namespace AI.Nova.Server.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabaseComments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterTable(
                name: "WebAuthnCredentials",
                comment: "凭据表",
                oldComment: "WebAuthn凭据表：存储用户的 WebAuthn (FIDO2) 认证凭证，用于实现无密码登录。");

            migrationBuilder.AlterTable(
                name: "UserTokens",
                comment: "用户令牌表",
                oldComment: "用户令牌表：存储用户的身份验证令牌、刷新令牌或第三方登录令牌。");

            migrationBuilder.AlterTable(
                name: "UserSessions",
                comment: "用户会话表",
                oldComment: "用户会话表：记录用户的登录会话信息，用于设备管理、安全审计和推送通知。");

            migrationBuilder.AlterTable(
                name: "Users",
                comment: "用户表",
                oldComment: "用户核心表：存储系统用户的账户信息、个人资料及安全凭证。");

            migrationBuilder.AlterTable(
                name: "UserRoles",
                comment: "用户角色表",
                oldComment: "用户角色关联表：用于实现用户与角色的多对多关系。");

            migrationBuilder.AlterTable(
                name: "UserLogins",
                comment: "用户登录表",
                oldComment: "用户外部登录表：存储用户关联的第三方登录提供商信息（如 Google, Microsoft, Facebook 等）。");

            migrationBuilder.AlterTable(
                name: "UserClaims",
                comment: "用户声明表",
                oldComment: "用户声明表：存储用户特定的声明数据（如权限、自定义属性等。");

            migrationBuilder.AlterTable(
                name: "SystemPrompts",
                comment: "系统提示词表",
                oldComment: "系统提示词表：存储 AI 系统提示词及其版本配置的主表");

            migrationBuilder.AlterTable(
                name: "Roles",
                comment: "角色表",
                oldComment: "角色表：用于系统权限管理的角色定义");

            migrationBuilder.AlterTable(
                name: "RoleClaims",
                comment: "角色声明表",
                oldComment: "角色声明表：存储角色关联的声明数据（如权限标识、自定义属性等）。");

            migrationBuilder.AlterTable(
                name: "PushNotificationSubscriptions",
                comment: "推送通知订阅表",
                oldComment: "推送通知订阅表：存储用户设备接收推送通知所需的凭证（如 Endpoint, P256dh, Auth）和订阅状态。");

            migrationBuilder.AlterTable(
                name: "Products",
                comment: "商品表",
                oldComment: "产品核心表：存储电商商品的详细信息、价格策略、多格式描述及用于 AI 语义搜索的向量数据。");

            migrationBuilder.AlterTable(
                name: "ProductImages",
                comment: "商品图片表",
                oldComment: "商品图片关联表：存储商品的多媒体展示资源，支持多图展示、主图标记及排序展示。");

            migrationBuilder.AlterTable(
                name: "Payments",
                comment: "支付流水表",
                oldComment: "支付流水表：记录用户对订单进行的每一笔支付尝试及其最终状态。");

            migrationBuilder.AlterTable(
                name: "Orders",
                comment: "订单表",
                oldComment: "订单主表：电商交易的核心记录，维护订单生命周期状态及金额明细。");

            migrationBuilder.AlterTable(
                name: "OrderItems",
                comment: "订单子表",
                oldComment: "订单明细表：记录订单中每一项商品的详细快照及购买数量。");

            migrationBuilder.AlterTable(
                name: "Inventories",
                comment: "商品库存表",
                oldComment: "商品库存表：维护商品实时库存、占用库存及库存报警阈值。");

            migrationBuilder.AlterTable(
                name: "DataProtectionKeys",
                comment: "密钥存储表",
                oldComment: "数据保护系统的密钥存储表：存储 ASP.NET Core DataProtection 的密钥环，用于在服务器重启或集群环境下保持 Cookie 和 Token 有效性");

            migrationBuilder.AlterTable(
                name: "Categories",
                comment: "商品分类表",
                oldComment: "商品分类表：用于管理商品的类别和标签");

            migrationBuilder.AlterTable(
                name: "CartItems",
                comment: "购物车",
                oldComment: "购物车明细表：记录用户添加到结算清单的商品、数量及勾选状态。");

            migrationBuilder.AlterTable(
                name: "Attachments",
                comment: "附件表",
                oldComment: "附件表：存储系统中的文件引用信息");

            migrationBuilder.AlterTable(
                name: "Addresses",
                comment: "用户收货地址表",
                oldComment: "用户收货地址表：存储用户的收货联系人、电话及多级行政区划详细地址。");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "WebAuthnCredentials",
                type: "uuid",
                nullable: true,
                comment: "用户外键",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "关联用户的唯一标识符");

            migrationBuilder.AlterColumn<int[]>(
                name: "Transports",
                table: "WebAuthnCredentials",
                type: "integer[]",
                nullable: true,
                comment: "传输方式 (0：usb，1：nfc，2：ble，3：smart-card，4：hybrid，5，internal",
                oldClrType: typeof(int[]),
                oldType: "integer[]",
                oldNullable: true,
                oldComment: "认证器支持的传输方式 (USB, NFC, BLE 等)");

            migrationBuilder.AlterColumn<long>(
                name: "SignCount",
                table: "WebAuthnCredentials",
                type: "bigint",
                nullable: true,
                comment: "签名计数器",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "签名计数器，用于防止重放攻击");

            migrationBuilder.AlterColumn<byte[]>(
                name: "PublicKey",
                table: "WebAuthnCredentials",
                type: "bytea",
                nullable: true,
                comment: "公钥",
                oldClrType: typeof(byte[]),
                oldType: "bytea",
                oldNullable: true,
                oldComment: "用户的公钥 (COSE Key 格式)，用于验证签名");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "WebAuthnCredentials",
                type: "timestamp with time zone",
                nullable: true,
                comment: "修改时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录最后修改时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "ModifiedBy",
                table: "WebAuthnCredentials",
                type: "uuid",
                nullable: true,
                comment: "修改者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录最后修改者ID");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "WebAuthnCredentials",
                type: "boolean",
                nullable: true,
                comment: "是否删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "软删除标记：true表示已删除");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "WebAuthnCredentials",
                type: "timestamp with time zone",
                nullable: true,
                comment: "删除时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录删除时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeletedBy",
                table: "WebAuthnCredentials",
                type: "uuid",
                nullable: true,
                comment: "删除者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录删除者ID");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "WebAuthnCredentials",
                type: "timestamp with time zone",
                nullable: true,
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录创建时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "WebAuthnCredentials",
                type: "uuid",
                nullable: true,
                comment: "创建者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录创建者ID");

            migrationBuilder.AlterColumn<byte[]>(
                name: "Id",
                table: "WebAuthnCredentials",
                type: "bytea",
                nullable: false,
                comment: "主键",
                oldClrType: typeof(byte[]),
                oldType: "bytea",
                oldComment: "凭证的唯一标识符 (Credential ID)");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "UserTokens",
                type: "text",
                nullable: true,
                comment: "令牌值",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "令牌的具体值（敏感数据，通常经过哈希处理或加密）");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "UserTokens",
                type: "timestamp with time zone",
                nullable: true,
                comment: "修改时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录最后修改时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "ModifiedBy",
                table: "UserTokens",
                type: "uuid",
                nullable: true,
                comment: "修改者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录最后修改者ID");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "UserTokens",
                type: "boolean",
                nullable: true,
                comment: "软删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "软删除标记：true表示已删除");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "UserTokens",
                type: "timestamp with time zone",
                nullable: true,
                comment: "删除时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录删除时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeletedBy",
                table: "UserTokens",
                type: "uuid",
                nullable: true,
                comment: "删除者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录删除者ID");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "UserTokens",
                type: "timestamp with time zone",
                nullable: true,
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录创建时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "UserTokens",
                type: "uuid",
                nullable: true,
                comment: "创建者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录创建者ID");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "UserTokens",
                type: "text",
                nullable: false,
                comment: "名称（主键）：例如 'SecurityStamp' 或 'AccessToken'",
                oldClrType: typeof(string),
                oldType: "text",
                oldComment: "令牌名称（主键的一部分）：例如 'SecurityStamp' 或 'AccessToken'");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "UserTokens",
                type: "text",
                nullable: false,
                comment: "提供商（主键）：例如 'AspNetCore.Identity' 或 'Google'",
                oldClrType: typeof(string),
                oldType: "text",
                oldComment: "令牌提供商名称（主键的一部分）：例如 'AspNetCore.Identity' 或 'Google'");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "UserTokens",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuidv7()",
                comment: "用户外键",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "uuidv7()",
                oldComment: "用户ID（主键的一部分）：关联到 Users 表");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "UserSessions",
                type: "uuid",
                nullable: true,
                comment: "用话外键",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "关联用户的 ID (外键)");

            migrationBuilder.AlterColumn<long>(
                name: "StartedOn",
                table: "UserSessions",
                type: "bigint",
                nullable: true,
                comment: "开始时间",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "会话开始时间 (Unix 时间戳，单位：秒)");

            migrationBuilder.AlterColumn<string>(
                name: "SignalRConnectionId",
                table: "UserSessions",
                type: "text",
                nullable: true,
                comment: "连接ID",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "SignalR 连接 ID，用于实时消息推送");

            migrationBuilder.AlterColumn<long>(
                name: "RenewedOn",
                table: "UserSessions",
                type: "bigint",
                nullable: true,
                comment: "续期时间",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "会话最后续期时间 (Unix 时间戳，单位：秒)");

            migrationBuilder.AlterColumn<bool>(
                name: "Privileged",
                table: "UserSessions",
                type: "boolean",
                nullable: true,
                comment: "访问标记",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "特权访问标记：指示该会话是否拥有高权限（如管理员操作）");

            migrationBuilder.AlterColumn<int>(
                name: "PlatformType",
                table: "UserSessions",
                type: "integer",
                nullable: true,
                comment: "客户端应用平台类型（如：0：Web, 1：Ios,2：MacOS,3：Linux, 4：Android,5：Windows）",
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true,
                oldComment: "客户端应用平台类型（如：Web, iOS, Android, Windows）");

            migrationBuilder.AlterColumn<int>(
                name: "NotificationStatus",
                table: "UserSessions",
                type: "integer",
                nullable: true,
                comment: "推送通知状态，0：未配置；1：允许；2：静音",
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true,
                oldComment: "推送通知状态：0=未配置, 1=允许, 2=静音");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "UserSessions",
                type: "timestamp with time zone",
                nullable: true,
                comment: "修改时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录最后修改时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "ModifiedBy",
                table: "UserSessions",
                type: "uuid",
                nullable: true,
                comment: "修改者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录最后修改者ID");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "UserSessions",
                type: "boolean",
                nullable: true,
                comment: "是否删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "软删除标记：true表示已删除");

            migrationBuilder.AlterColumn<string>(
                name: "IP",
                table: "UserSessions",
                type: "text",
                nullable: true,
                comment: "IP",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "用户会话的 IP 地址");

            migrationBuilder.AlterColumn<string>(
                name: "DeviceInfo",
                table: "UserSessions",
                type: "text",
                nullable: true,
                comment: "设备详细信息",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "设备详细信息（浏览器、操作系统、设备型号等）");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "UserSessions",
                type: "timestamp with time zone",
                nullable: true,
                comment: "删除时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录删除时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeletedBy",
                table: "UserSessions",
                type: "uuid",
                nullable: true,
                comment: "删除者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录删除者ID");

            migrationBuilder.AlterColumn<string>(
                name: "CultureName",
                table: "UserSessions",
                type: "text",
                nullable: true,
                comment: "当前语言",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "用户在该会话中选择的语言文化代码 (如：zh-CN, en-US)");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "UserSessions",
                type: "timestamp with time zone",
                nullable: true,
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录创建时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "UserSessions",
                type: "uuid",
                nullable: true,
                comment: "创建者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录创建者ID");

            migrationBuilder.AlterColumn<string>(
                name: "AppVersion",
                table: "UserSessions",
                type: "text",
                nullable: true,
                comment: "版本号",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "客户端应用程序的版本号");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "UserSessions",
                type: "text",
                nullable: true,
                comment: "地理位置信息",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "基于 IP 地址解析的地理位置信息");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "UserSessions",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuidv7()",
                comment: "主键",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "uuidv7()",
                oldComment: "会话的唯一标识符 (主键)");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "TwoFactorTokenRequestedOn",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true,
                comment: "双因素认证 (2FA) 时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "双因素认证 (2FA) 令牌的最后请求时间");

            migrationBuilder.AlterColumn<string>(
                name: "SecurityStamp",
                table: "Users",
                type: "text",
                nullable: true,
                comment: "安全戳",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "安全戳：当用户凭据变更（如改密、删登录）时更改，用于使旧令牌失效");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ResetPasswordTokenRequestedOn",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true,
                comment: "重置密码时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "重置密码令牌的最后请求时间");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "PhoneNumberTokenRequestedOn",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true,
                comment: "手机号时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "手机号验证令牌的最后请求时间");

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "Users",
                type: "text",
                nullable: true,
                comment: "密码",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "密码的加盐哈希值");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "OtpRequestedOn",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true,
                comment: "一次性密码 (OTP) 的请求时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "一次性密码 (OTP) 的最后请求时间");

            migrationBuilder.AlterColumn<string>(
                name: "NormalizedUserName",
                table: "Users",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true,
                comment: "标准化用户名",
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256,
                oldNullable: true,
                oldComment: "标准化用户名 (用于索引和查找)");

            migrationBuilder.AlterColumn<string>(
                name: "NormalizedEmail",
                table: "Users",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true,
                comment: "邮件地址",
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256,
                oldNullable: true,
                oldComment: "标准化电子邮件地址");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true,
                comment: "修改时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录最后修改时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "ModifiedBy",
                table: "Users",
                type: "uuid",
                nullable: true,
                comment: "修改者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录最后修改者ID");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "LockoutEnd",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true,
                comment: "锁定期结束时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "锁定期结束时间 (UTC)。如果为过去时间或空，表示未锁定");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Users",
                type: "boolean",
                nullable: true,
                comment: "软删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "软删除标记：true表示已删除");

            migrationBuilder.AlterColumn<bool>(
                name: "HasProfilePicture",
                table: "Users",
                type: "boolean",
                nullable: true,
                comment: "是否有头像",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "是否拥有头像标记");

            migrationBuilder.AlterColumn<int>(
                name: "Gender",
                table: "Users",
                type: "integer",
                nullable: true,
                comment: "性别",
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true,
                oldComment: "用户性别");

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "Users",
                type: "text",
                nullable: true,
                comment: "全名",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "用户的全名");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "EmailTokenRequestedOn",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true,
                comment: "邮箱验证时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "邮箱验证/修改令牌的最后请求时间，用于安全校验");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true,
                comment: "邮件地址",
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256,
                oldNullable: true,
                oldComment: "电子邮件地址");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ElevatedAccessTokenRequestedOn",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true,
                comment: "Elevated Access请求时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "高权限访问令牌 (Elevated Access) 的最后请求时间");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true,
                comment: "除时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录删除时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeletedBy",
                table: "Users",
                type: "uuid",
                nullable: true,
                comment: "删除者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录删除者ID");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true,
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录创建时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "Users",
                type: "uuid",
                nullable: true,
                comment: "创建者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录创建者ID");

            migrationBuilder.AlterColumn<string>(
                name: "ConcurrencyStamp",
                table: "Users",
                type: "text",
                nullable: true,
                comment: "并发戳",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "并发戳：用于乐观并发控制，每次持久化到数据库时更改");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "BirthDate",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true,
                comment: "出生日期",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "用户出生日期");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "UserRoles",
                type: "timestamp with time zone",
                nullable: true,
                comment: "修改时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录最后修改时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "ModifiedBy",
                table: "UserRoles",
                type: "uuid",
                nullable: true,
                comment: "修改者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录最后修改者ID");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "UserRoles",
                type: "boolean",
                nullable: true,
                comment: "软删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "软删除标记：true表示已删除");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "UserRoles",
                type: "timestamp with time zone",
                nullable: true,
                comment: "删除时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录删除时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeletedBy",
                table: "UserRoles",
                type: "uuid",
                nullable: true,
                comment: "删除者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录删除者ID");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "UserRoles",
                type: "timestamp with time zone",
                nullable: true,
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录创建时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "UserRoles",
                type: "uuid",
                nullable: true,
                comment: "创建者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录创建者ID");

            migrationBuilder.AlterColumn<Guid>(
                name: "RoleId",
                table: "UserRoles",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuidv7()",
                comment: "角色外键",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "uuidv7()",
                oldComment: "角色ID（主键的一部分）：关联到 Roles 表");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "UserRoles",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuidv7()",
                comment: "用户外键",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "uuidv7()",
                oldComment: "用户ID（主键的一部分）：关联到 Users 表");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "UserLogins",
                type: "uuid",
                nullable: false,
                comment: "用户外键",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "用户表主键Id：关联Users表Id字段");

            migrationBuilder.AlterColumn<string>(
                name: "ProviderDisplayName",
                table: "UserLogins",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true,
                comment: "提供商名称",
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128,
                oldNullable: true,
                oldComment: "登录提供商的显示名称");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "UserLogins",
                type: "timestamp with time zone",
                nullable: true,
                comment: "修改时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录最后修改时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "ModifiedBy",
                table: "UserLogins",
                type: "uuid",
                nullable: true,
                comment: "修改者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录最后修改者ID");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "UserLogins",
                type: "boolean",
                nullable: true,
                comment: "软删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "软删除标记：true表示已删除");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "UserLogins",
                type: "timestamp with time zone",
                nullable: true,
                comment: "删除时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录删除时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeletedBy",
                table: "UserLogins",
                type: "uuid",
                nullable: true,
                comment: "删除者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录删除者ID");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "UserLogins",
                type: "timestamp with time zone",
                nullable: true,
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录创建时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "UserLogins",
                type: "uuid",
                nullable: true,
                comment: "创建者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录创建者ID");

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "UserLogins",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                comment: "提供商",
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128,
                oldComment: "提供商端的用户唯一标识符（Provider User ID）");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "UserLogins",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                comment: "提供商名称（例如：'Google', 'Facebook', 'Microsoft'）",
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128,
                oldComment: "登录提供商名称（例如：'Google', 'Facebook', 'Microsoft'）");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "UserClaims",
                type: "uuid",
                nullable: false,
                comment: "用户外键",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "用户表主键Id：关联Users表Id字段");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "UserClaims",
                type: "timestamp with time zone",
                nullable: true,
                comment: "修改时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录最后修改时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "ModifiedBy",
                table: "UserClaims",
                type: "uuid",
                nullable: true,
                comment: "修改者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录最后修改者ID");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "UserClaims",
                type: "boolean",
                nullable: true,
                comment: "软删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "软删除标记：true表示已删除");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "UserClaims",
                type: "timestamp with time zone",
                nullable: true,
                comment: "删除时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录删除时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeletedBy",
                table: "UserClaims",
                type: "uuid",
                nullable: true,
                comment: "删除者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录删除者ID");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "UserClaims",
                type: "timestamp with time zone",
                nullable: true,
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录创建时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "UserClaims",
                type: "uuid",
                nullable: true,
                comment: "创建者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录创建者ID");

            migrationBuilder.AlterColumn<string>(
                name: "ClaimValue",
                table: "UserClaims",
                type: "character varying(1024)",
                maxLength: 1024,
                nullable: true,
                comment: "声明值",
                oldClrType: typeof(string),
                oldType: "character varying(1024)",
                oldMaxLength: 1024,
                oldNullable: true,
                oldComment: "声明的具体值（例如：'Admin', 'HR', 'John Doe'）");

            migrationBuilder.AlterColumn<string>(
                name: "ClaimType",
                table: "UserClaims",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true,
                comment: "声明类型",
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256,
                oldNullable: true,
                oldComment: "声明的类型（例如：'Permission.Read', 'Department', 'FullName'）");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "TodoItems",
                type: "timestamp with time zone",
                nullable: true,
                comment: "修改时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录最后修改时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "ModifiedBy",
                table: "TodoItems",
                type: "uuid",
                nullable: true,
                comment: "修改者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录最后修改者ID");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "TodoItems",
                type: "boolean",
                nullable: true,
                comment: "是否删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "软删除标记：true表示已删除");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "TodoItems",
                type: "timestamp with time zone",
                nullable: true,
                comment: "删除时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录删除时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeletedBy",
                table: "TodoItems",
                type: "uuid",
                nullable: true,
                comment: "删除者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录删除者ID");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "TodoItems",
                type: "timestamp with time zone",
                nullable: true,
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录创建时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "TodoItems",
                type: "uuid",
                nullable: true,
                comment: "创建者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录创建者ID");

            migrationBuilder.AlterColumn<int>(
                name: "PromptKind",
                table: "SystemPrompts",
                type: "integer",
                nullable: true,
                comment: "类别，0：Support",
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true,
                oldComment: "提示词的类型枚举 (PromptKind)");

            migrationBuilder.AlterColumn<string>(
                name: "Markdown",
                table: "SystemPrompts",
                type: "text",
                nullable: true,
                comment: "内容",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "提示词的内容 (Markdown 格式)");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "SystemPrompts",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuidv7()",
                comment: "主键",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "uuidv7()",
                oldComment: "系统提示词的唯一标识符 (GUID)");

            migrationBuilder.AlterColumn<string>(
                name: "NormalizedName",
                table: "Roles",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                comment: "标准化名称",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true,
                oldComment: "标准化名称：用于数据库查询的大写名称");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Roles",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                comment: "名称",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true,
                oldComment: "角色名称：如 SuperAdmin, Demo 等");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "Roles",
                type: "timestamp with time zone",
                nullable: true,
                comment: "修改时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录最后修改时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "ModifiedBy",
                table: "Roles",
                type: "uuid",
                nullable: true,
                comment: "修改者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录最后修改者ID");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Roles",
                type: "boolean",
                nullable: true,
                comment: "软删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "软删除标记：true表示已删除");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "Roles",
                type: "timestamp with time zone",
                nullable: true,
                comment: "删除时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录删除时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeletedBy",
                table: "Roles",
                type: "uuid",
                nullable: true,
                comment: "删除者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录删除者ID");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "Roles",
                type: "timestamp with time zone",
                nullable: true,
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录创建时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "Roles",
                type: "uuid",
                nullable: true,
                comment: "创建者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录创建者ID");

            migrationBuilder.AlterColumn<string>(
                name: "ConcurrencyStamp",
                table: "Roles",
                type: "text",
                nullable: true,
                comment: "并发标记",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "并发标记：用于乐观锁控制");

            migrationBuilder.AlterColumn<Guid>(
                name: "RoleId",
                table: "RoleClaims",
                type: "uuid",
                nullable: false,
                comment: "角色外键",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "关联的角色 ID：外键，指向该声明所属的角色");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "RoleClaims",
                type: "timestamp with time zone",
                nullable: true,
                comment: "修改时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录最后修改时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "ModifiedBy",
                table: "RoleClaims",
                type: "uuid",
                nullable: true,
                comment: "修改者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录最后修改者ID");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "RoleClaims",
                type: "boolean",
                nullable: true,
                comment: "软删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "软删除标记：true表示已删除");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "RoleClaims",
                type: "timestamp with time zone",
                nullable: true,
                comment: "删除时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录删除时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeletedBy",
                table: "RoleClaims",
                type: "uuid",
                nullable: true,
                comment: "删除者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录删除者ID");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "RoleClaims",
                type: "timestamp with time zone",
                nullable: true,
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录创建时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "RoleClaims",
                type: "uuid",
                nullable: true,
                comment: "创建者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录创建者ID");

            migrationBuilder.AlterColumn<string>(
                name: "ClaimValue",
                table: "RoleClaims",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true,
                comment: "声明值",
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255,
                oldNullable: true,
                oldComment: "声明值：权限的具体数值或标识，例如 '-1' 代表无限制，或具体的功能代码");

            migrationBuilder.AlterColumn<string>(
                name: "ClaimType",
                table: "RoleClaims",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true,
                comment: "声明类型",
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255,
                oldNullable: true,
                oldComment: "声明类型：定义权限的种类，例如 'MaxPrivilegedSessions' 或 'Features'");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserSessionId",
                table: "PushNotificationSubscriptions",
                type: "uuid",
                nullable: true,
                comment: "用户会话外键",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "关联的用户会话 ID (外键)，用于追踪订阅来源设备");

            migrationBuilder.AlterColumn<string[]>(
                name: "Tags",
                table: "PushNotificationSubscriptions",
                type: "text[]",
                nullable: true,
                comment: "订阅标签数组[\"news\", \"alerts\"]",
                oldClrType: typeof(string[]),
                oldType: "text[]",
                oldNullable: true,
                oldComment: "订阅标签数组 (JSON)，用于按主题过滤推送消息");

            migrationBuilder.AlterColumn<long>(
                name: "RenewedOn",
                table: "PushNotificationSubscriptions",
                type: "bigint",
                nullable: true,
                comment: "订阅续期时间 （秒）",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "订阅最后续期时间 (Unix 时间戳，单位：秒)");

            migrationBuilder.AlterColumn<string>(
                name: "PushChannel",
                table: "PushNotificationSubscriptions",
                type: "text",
                nullable: true,
                comment: "推送渠道",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "推送服务提供的订阅 URL (Endpoint) 或渠道标识");

            migrationBuilder.AlterColumn<string>(
                name: "P256dh",
                table: "PushNotificationSubscriptions",
                type: "text",
                nullable: true,
                comment: "加密公钥",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "Web Push 用户代理公钥 (P-256dh)，用于消息加密");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "PushNotificationSubscriptions",
                type: "timestamp with time zone",
                nullable: true,
                comment: "修改时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录最后修改时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "ModifiedBy",
                table: "PushNotificationSubscriptions",
                type: "uuid",
                nullable: true,
                comment: "修改者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录最后修改者ID");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "PushNotificationSubscriptions",
                type: "boolean",
                nullable: true,
                comment: "是否删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "软删除标记：true表示已删除");

            migrationBuilder.AlterColumn<long>(
                name: "ExpirationTime",
                table: "PushNotificationSubscriptions",
                type: "bigint",
                nullable: true,
                comment: "订阅过期时间（秒）",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "订阅过期时间 (Unix 时间戳，单位：秒)");

            migrationBuilder.AlterColumn<string>(
                name: "Endpoint",
                table: "PushNotificationSubscriptions",
                type: "text",
                nullable: true,
                comment: "推送地址",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "完整的推送消息发送 Endpoint URL");

            migrationBuilder.AlterColumn<string>(
                name: "DeviceId",
                table: "PushNotificationSubscriptions",
                type: "text",
                nullable: true,
                comment: "设备标识符",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "设备的唯一标识符 (Device ID)");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "PushNotificationSubscriptions",
                type: "timestamp with time zone",
                nullable: true,
                comment: "删除时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录删除时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeletedBy",
                table: "PushNotificationSubscriptions",
                type: "uuid",
                nullable: true,
                comment: "删除者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录删除者ID");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "PushNotificationSubscriptions",
                type: "timestamp with time zone",
                nullable: true,
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录创建时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "PushNotificationSubscriptions",
                type: "uuid",
                nullable: true,
                comment: "创建者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录创建者ID");

            migrationBuilder.AlterColumn<string>(
                name: "Auth",
                table: "PushNotificationSubscriptions",
                type: "text",
                nullable: true,
                comment: "认证密钥",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "Web Push 认证密钥 (Auth Secret)，用于生成加密盐");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "PushNotificationSubscriptions",
                type: "integer",
                nullable: false,
                comment: "主键",
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "订阅记录的主键 ID")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<uint>(
                name: "xmin",
                table: "Products",
                type: "xid",
                rowVersion: true,
                nullable: false,
                comment: "版本号",
                oldClrType: typeof(uint),
                oldType: "xid",
                oldRowVersion: true,
                oldComment: "行版本号 (用于乐观并发控制)");

            migrationBuilder.AlterColumn<int>(
                name: "ShortId",
                table: "Products",
                type: "integer",
                nullable: false,
                comment: "商品短ID",
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "用于生成友好 URL 的短整型 ID");

            migrationBuilder.AlterColumn<string>(
                name: "PrimaryImageAltText",
                table: "Products",
                type: "text",
                nullable: true,
                comment: "主图片 Alt Text",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "主图片的替代文本 (Alt Text)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Products",
                type: "numeric(18,3)",
                precision: 18,
                scale: 3,
                nullable: true,
                comment: "价格",
                oldClrType: typeof(decimal),
                oldType: "numeric(18,3)",
                oldPrecision: 18,
                oldScale: 3,
                oldNullable: true,
                oldComment: "产品价格 (十进制)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Products",
                type: "character varying(64)",
                maxLength: 64,
                nullable: true,
                comment: "名称",
                oldClrType: typeof(string),
                oldType: "character varying(64)",
                oldMaxLength: 64,
                oldNullable: true,
                oldComment: "产品名称 (最大长度 64 字符)");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "Products",
                type: "timestamp with time zone",
                nullable: true,
                comment: "修改时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录最后修改时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "ModifiedBy",
                table: "Products",
                type: "uuid",
                nullable: true,
                comment: "修改者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录最后修改者ID");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Products",
                type: "boolean",
                nullable: true,
                comment: "是否删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "软删除标记：true表示已删除");

            migrationBuilder.AlterColumn<bool>(
                name: "HasPrimaryImage",
                table: "Products",
                type: "boolean",
                nullable: true,
                comment: "拥有主图片",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "标记该产品是否拥有主图");

            migrationBuilder.AlterColumn<Vector>(
                name: "Embedding",
                table: "Products",
                type: "vector(768)",
                nullable: true,
                comment: "向量",
                oldClrType: typeof(Vector),
                oldType: "vector(768)",
                oldNullable: true,
                oldComment: "用于语义搜索的向量嵌入 (Pgvector)");

            migrationBuilder.AlterColumn<string>(
                name: "DescriptionText",
                table: "Products",
                type: "character varying(4096)",
                maxLength: 4096,
                nullable: true,
                comment: "纯文本描述",
                oldClrType: typeof(string),
                oldType: "character varying(4096)",
                oldMaxLength: 4096,
                oldNullable: true,
                oldComment: "产品的纯文本描述，用于搜索或摘要");

            migrationBuilder.AlterColumn<string>(
                name: "DescriptionHTML",
                table: "Products",
                type: "character varying(4096)",
                maxLength: 4096,
                nullable: true,
                comment: "HTML描述",
                oldClrType: typeof(string),
                oldType: "character varying(4096)",
                oldMaxLength: 4096,
                oldNullable: true,
                oldComment: "产品的 HTML 格式描述");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "Products",
                type: "timestamp with time zone",
                nullable: true,
                comment: "删除时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录删除时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeletedBy",
                table: "Products",
                type: "uuid",
                nullable: true,
                comment: "删除者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录删除者ID");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "Products",
                type: "timestamp with time zone",
                nullable: true,
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录创建时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "Products",
                type: "uuid",
                nullable: true,
                comment: "创建者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录创建者ID");

            migrationBuilder.AlterColumn<Guid>(
                name: "CategoryId",
                table: "Products",
                type: "uuid",
                nullable: true,
                comment: "类别外键",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "关联的分类 ID");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Products",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuidv7()",
                comment: "主键",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "uuidv7()",
                oldComment: "产品的唯一标识符 (GUID)");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "ProductReviews",
                type: "uuid",
                nullable: true,
                comment: "用户外键",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "发表评论的用户 ID");

            migrationBuilder.AlterColumn<short>(
                name: "Rating",
                table: "ProductReviews",
                type: "smallint",
                nullable: true,
                comment: "评分 (取值范围 1-5)",
                oldClrType: typeof(short),
                oldType: "smallint",
                oldNullable: true,
                oldComment: "商品评分 (取值范围 1-5)");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductId",
                table: "ProductReviews",
                type: "uuid",
                nullable: true,
                comment: "商品外键",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "被评价的商品 ID");

            migrationBuilder.AlterColumn<Guid>(
                name: "OrderId",
                table: "ProductReviews",
                type: "uuid",
                nullable: true,
                comment: "订单外键",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "关联的订单 ID");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "ProductReviews",
                type: "timestamp with time zone",
                nullable: true,
                comment: "修改时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录最后修改时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "ModifiedBy",
                table: "ProductReviews",
                type: "uuid",
                nullable: true,
                comment: "修改者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录最后修改者ID");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "ProductReviews",
                type: "boolean",
                nullable: true,
                comment: "是否删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "软删除标记：true表示已删除");

            migrationBuilder.AlterColumn<bool>(
                name: "IsAnonymous",
                table: "ProductReviews",
                type: "boolean",
                nullable: true,
                comment: "是否匿名评论",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "是否启用匿名方式显示评价");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "ProductReviews",
                type: "timestamp with time zone",
                nullable: true,
                comment: "删除时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录删除时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeletedBy",
                table: "ProductReviews",
                type: "uuid",
                nullable: true,
                comment: "删除者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录删除者ID");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "ProductReviews",
                type: "timestamp with time zone",
                nullable: true,
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录创建时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "ProductReviews",
                type: "uuid",
                nullable: true,
                comment: "创建者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录创建者ID");

            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "ProductReviews",
                type: "character varying(1024)",
                maxLength: 1024,
                nullable: true,
                comment: "评论内容",
                oldClrType: typeof(string),
                oldType: "character varying(1024)",
                oldMaxLength: 1024,
                oldNullable: true,
                oldComment: "用户发表的评价正文内容");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "ProductReviews",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuidv7()",
                comment: "主键",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "uuidv7()",
                oldComment: "评价记录唯一标识");

            migrationBuilder.AlterColumn<int>(
                name: "SortOrder",
                table: "ProductImages",
                type: "integer",
                nullable: true,
                comment: "排序",
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true,
                oldComment: "图片在展示列表中的排序权重 (升序排列)");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductId",
                table: "ProductImages",
                type: "uuid",
                nullable: true,
                comment: "商品主键",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "关联的商品 ID");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "ProductImages",
                type: "timestamp with time zone",
                nullable: true,
                comment: "修改时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录最后修改时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "ModifiedBy",
                table: "ProductImages",
                type: "uuid",
                nullable: true,
                comment: "修改者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录最后修改者ID");

            migrationBuilder.AlterColumn<bool>(
                name: "IsPrimary",
                table: "ProductImages",
                type: "boolean",
                nullable: true,
                comment: "是否封面主图",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "是否将此图片设为商品封面主图");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "ProductImages",
                type: "boolean",
                nullable: true,
                comment: "是否删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "软删除标记：true表示已删除");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "ProductImages",
                type: "character varying(512)",
                maxLength: 512,
                nullable: true,
                comment: "图片路径",
                oldClrType: typeof(string),
                oldType: "character varying(512)",
                oldMaxLength: 512,
                oldNullable: true,
                oldComment: "图片素材的存储 URL 路径");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "ProductImages",
                type: "timestamp with time zone",
                nullable: true,
                comment: "删除时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录删除时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeletedBy",
                table: "ProductImages",
                type: "uuid",
                nullable: true,
                comment: "删除者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录删除者ID");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "ProductImages",
                type: "timestamp with time zone",
                nullable: true,
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录创建时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "ProductImages",
                type: "uuid",
                nullable: true,
                comment: "创建者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录创建者ID");

            migrationBuilder.AlterColumn<string>(
                name: "AltText",
                table: "ProductImages",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true,
                comment: "Alt Text",
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128,
                oldNullable: true,
                oldComment: "图片替代文本 (Alt Text)，用于 SEO 和无障碍显示");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "ProductImages",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuidv7()",
                comment: "主键",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "uuidv7()",
                oldComment: "图片记录唯一标识");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Payments",
                type: "uuid",
                nullable: true,
                comment: "用户外键",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "执行该支付流水记录的用户 ID");

            migrationBuilder.AlterColumn<string>(
                name: "TransactionId",
                table: "Payments",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true,
                comment: "交易单号",
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128,
                oldNullable: true,
                oldComment: "三方支付机构返回的全局唯一交易参考单号");

            migrationBuilder.AlterColumn<short>(
                name: "Status",
                table: "Payments",
                type: "smallint",
                nullable: true,
                comment: "支付状态（0:待支付, 1:成功, 2:失败, 3:已退款）",
                oldClrType: typeof(short),
                oldType: "smallint",
                oldNullable: true,
                oldComment: "该笔支付流水当前的处理状态枚举");

            migrationBuilder.AlterColumn<string>(
                name: "PaymentMethod",
                table: "Payments",
                type: "character varying(32)",
                maxLength: 32,
                nullable: true,
                comment: "支付方式 (如：Alipay, WeChatPay)",
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32,
                oldNullable: true,
                oldComment: "支付方式/渠道名称 (如：Alipay, WeChatPay)");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "PaidOn",
                table: "Payments",
                type: "timestamp with time zone",
                nullable: true,
                comment: "支付时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "支付状态被标记为成功的时间点");

            migrationBuilder.AlterColumn<Guid>(
                name: "OrderId",
                table: "Payments",
                type: "uuid",
                nullable: true,
                comment: "订单外键",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "关联的所属订单 ID");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "Payments",
                type: "timestamp with time zone",
                nullable: true,
                comment: "修改时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录最后修改时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "ModifiedBy",
                table: "Payments",
                type: "uuid",
                nullable: true,
                comment: "修改者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录最后修改者ID");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Payments",
                type: "boolean",
                nullable: true,
                comment: "是否删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "软删除标记：true表示已删除");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "Payments",
                type: "timestamp with time zone",
                nullable: true,
                comment: "删除时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录删除时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeletedBy",
                table: "Payments",
                type: "uuid",
                nullable: true,
                comment: "删除者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录删除者ID");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "Payments",
                type: "timestamp with time zone",
                nullable: true,
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录创建时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "Payments",
                type: "uuid",
                nullable: true,
                comment: "创建者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录创建者ID");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Payments",
                type: "numeric(18,3)",
                precision: 18,
                scale: 3,
                nullable: true,
                comment: "实付金额",
                oldClrType: typeof(decimal),
                oldType: "numeric(18,3)",
                oldPrecision: 18,
                oldScale: 3,
                oldNullable: true,
                oldComment: "本次支付流水的实付金额");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Payments",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuidv7()",
                comment: "主键",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "uuidv7()",
                oldComment: "支付记录唯一标识");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Orders",
                type: "uuid",
                nullable: true,
                comment: "用户外键",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "关联的下单用户 ID");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalAmount",
                table: "Orders",
                type: "numeric(18,3)",
                precision: 18,
                scale: 3,
                nullable: true,
                comment: "总价",
                oldClrType: typeof(decimal),
                oldType: "numeric(18,3)",
                oldPrecision: 18,
                oldScale: 3,
                oldNullable: true,
                oldComment: "商品原始销售总价");

            migrationBuilder.AlterColumn<short>(
                name: "Status",
                table: "Orders",
                type: "smallint",
                nullable: true,
                comment: "订单状态：0:待付款, 1:已付款, 2:已发货, 3:已完成, 4:已取消, 5:退款中",
                oldClrType: typeof(short),
                oldType: "smallint",
                oldNullable: true,
                oldComment: "0:待付款, 1:已付款, 2:已发货, 3:已完成, 4:已取消, 5:退款中");

            migrationBuilder.AlterColumn<decimal>(
                name: "ShippingFee",
                table: "Orders",
                type: "numeric(18,3)",
                precision: 18,
                scale: 3,
                nullable: true,
                comment: "配送费用",
                oldClrType: typeof(decimal),
                oldType: "numeric(18,3)",
                oldPrecision: 18,
                oldScale: 3,
                oldNullable: true,
                oldComment: "订单物流配送费用");

            migrationBuilder.AlterColumn<string>(
                name: "Remark",
                table: "Orders",
                type: "character varying(512)",
                maxLength: 512,
                nullable: true,
                comment: "备注",
                oldClrType: typeof(string),
                oldType: "character varying(512)",
                oldMaxLength: 512,
                oldNullable: true,
                oldComment: "用户提供的订单留言或特殊备注说明");

            migrationBuilder.AlterColumn<decimal>(
                name: "PayableAmount",
                table: "Orders",
                type: "numeric(18,3)",
                precision: 18,
                scale: 3,
                nullable: true,
                comment: "实际支付总金额",
                oldClrType: typeof(decimal),
                oldType: "numeric(18,3)",
                oldPrecision: 18,
                oldScale: 3,
                oldNullable: true,
                oldComment: "实付应结的总金额 (含运费并扣除优惠)");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "PaidOn",
                table: "Orders",
                type: "timestamp with time zone",
                nullable: true,
                comment: "支付时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "该订单由于支付成功而被确认的时间戳");

            migrationBuilder.AlterColumn<string>(
                name: "OrderNo",
                table: "Orders",
                type: "character varying(32)",
                maxLength: 32,
                nullable: true,
                comment: "订单编号",
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32,
                oldNullable: true,
                oldComment: "全局唯一业务订单编号");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "Orders",
                type: "timestamp with time zone",
                nullable: true,
                comment: "修改时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录最后修改时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "ModifiedBy",
                table: "Orders",
                type: "uuid",
                nullable: true,
                comment: "修改者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录最后修改者ID");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Orders",
                type: "boolean",
                nullable: true,
                comment: "是否删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "软删除标记：true表示已删除");

            migrationBuilder.AlterColumn<decimal>(
                name: "DiscountAmount",
                table: "Orders",
                type: "numeric(18,3)",
                precision: 18,
                scale: 3,
                nullable: true,
                comment: "抵扣总金额",
                oldClrType: typeof(decimal),
                oldType: "numeric(18,3)",
                oldPrecision: 18,
                oldScale: 3,
                oldNullable: true,
                oldComment: "优惠抵扣的总金额");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "Orders",
                type: "timestamp with time zone",
                nullable: true,
                comment: "删除时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录删除时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeletedBy",
                table: "Orders",
                type: "uuid",
                nullable: true,
                comment: "删除者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录删除者ID");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "Orders",
                type: "timestamp with time zone",
                nullable: true,
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录创建时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "Orders",
                type: "uuid",
                nullable: true,
                comment: "创建者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录创建者ID");

            migrationBuilder.AlterColumn<Guid>(
                name: "AddressId",
                table: "Orders",
                type: "uuid",
                nullable: true,
                comment: "收货地址外键",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "关联的配送地址 ID");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Orders",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuidv7()",
                comment: "主键",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "uuidv7()",
                oldComment: "订单记录唯一标识");

            migrationBuilder.AlterColumn<decimal>(
                name: "UnitPrice",
                table: "OrderItems",
                type: "numeric(18,3)",
                precision: 18,
                scale: 3,
                nullable: true,
                comment: "单价",
                oldClrType: typeof(decimal),
                oldType: "numeric(18,3)",
                oldPrecision: 18,
                oldScale: 3,
                oldNullable: true,
                oldComment: "下单时刻存储的成交单价快照");

            migrationBuilder.AlterColumn<decimal>(
                name: "SubTotal",
                table: "OrderItems",
                type: "numeric(18,3)",
                precision: 18,
                scale: 3,
                nullable: true,
                comment: "总金额",
                oldClrType: typeof(decimal),
                oldType: "numeric(18,3)",
                oldPrecision: 18,
                oldScale: 3,
                oldNullable: true,
                oldComment: "该订单项对应的合计金额小计");

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "OrderItems",
                type: "integer",
                nullable: true,
                comment: "数量",
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true,
                oldComment: "用户购买商品的选择数量");

            migrationBuilder.AlterColumn<string>(
                name: "ProductName",
                table: "OrderItems",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true,
                comment: "商品名称",
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128,
                oldNullable: true,
                oldComment: "下单时刻存储的商品名称属性快照");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductId",
                table: "OrderItems",
                type: "uuid",
                nullable: true,
                comment: "商品外键",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "关联的商品 ID");

            migrationBuilder.AlterColumn<string>(
                name: "PrimaryImageAltText",
                table: "OrderItems",
                type: "text",
                nullable: true,
                comment: "图片地址",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "下单时刻存储的图片替代文本快照");

            migrationBuilder.AlterColumn<Guid>(
                name: "OrderId",
                table: "OrderItems",
                type: "uuid",
                nullable: true,
                comment: "订单外键",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "关联的主订单 ID");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "OrderItems",
                type: "timestamp with time zone",
                nullable: true,
                comment: "修改时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录最后修改时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "ModifiedBy",
                table: "OrderItems",
                type: "uuid",
                nullable: true,
                comment: "修改者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录最后修改者ID");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "OrderItems",
                type: "boolean",
                nullable: true,
                comment: "是否删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "软删除标记：true表示已删除");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "OrderItems",
                type: "timestamp with time zone",
                nullable: true,
                comment: "删除时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录删除时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeletedBy",
                table: "OrderItems",
                type: "uuid",
                nullable: true,
                comment: "删除者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录删除者ID");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "OrderItems",
                type: "timestamp with time zone",
                nullable: true,
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录创建时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "OrderItems",
                type: "uuid",
                nullable: true,
                comment: "创建者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录创建者ID");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "OrderItems",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuidv7()",
                comment: "主键",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "uuidv7()",
                oldComment: "订单项记录唯一标识");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "KnowledgeDocuments",
                type: "timestamp with time zone",
                nullable: true,
                comment: "修改时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录最后修改时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "ModifiedBy",
                table: "KnowledgeDocuments",
                type: "uuid",
                nullable: true,
                comment: "修改者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录最后修改者ID");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "KnowledgeDocuments",
                type: "boolean",
                nullable: true,
                comment: "是否删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "软删除标记：true表示已删除");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "KnowledgeDocuments",
                type: "timestamp with time zone",
                nullable: true,
                comment: "删除时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录删除时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeletedBy",
                table: "KnowledgeDocuments",
                type: "uuid",
                nullable: true,
                comment: "删除者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录删除者ID");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "KnowledgeDocuments",
                type: "timestamp with time zone",
                nullable: true,
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录创建时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "KnowledgeDocuments",
                type: "uuid",
                nullable: true,
                comment: "创建者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录创建者ID");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "KnowledgeDocumentChunks",
                type: "timestamp with time zone",
                nullable: true,
                comment: "修改时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录最后修改时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "ModifiedBy",
                table: "KnowledgeDocumentChunks",
                type: "uuid",
                nullable: true,
                comment: "修改者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录最后修改者ID");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "KnowledgeDocumentChunks",
                type: "boolean",
                nullable: true,
                comment: "是否删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "软删除标记：true表示已删除");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "KnowledgeDocumentChunks",
                type: "timestamp with time zone",
                nullable: true,
                comment: "删除时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录删除时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeletedBy",
                table: "KnowledgeDocumentChunks",
                type: "uuid",
                nullable: true,
                comment: "删除者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录删除者ID");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "KnowledgeDocumentChunks",
                type: "timestamp with time zone",
                nullable: true,
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录创建时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "KnowledgeDocumentChunks",
                type: "uuid",
                nullable: true,
                comment: "创建者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录创建者ID");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "KnowledgeBases",
                type: "timestamp with time zone",
                nullable: true,
                comment: "修改时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录最后修改时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "ModifiedBy",
                table: "KnowledgeBases",
                type: "uuid",
                nullable: true,
                comment: "修改者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录最后修改者ID");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "KnowledgeBases",
                type: "boolean",
                nullable: true,
                comment: "是否删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "软删除标记：true表示已删除");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "KnowledgeBases",
                type: "timestamp with time zone",
                nullable: true,
                comment: "删除时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录删除时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeletedBy",
                table: "KnowledgeBases",
                type: "uuid",
                nullable: true,
                comment: "删除者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录删除者ID");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "KnowledgeBases",
                type: "timestamp with time zone",
                nullable: true,
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录创建时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "KnowledgeBases",
                type: "uuid",
                nullable: true,
                comment: "创建者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录创建者ID");

            migrationBuilder.AlterColumn<int>(
                name: "StockQuantity",
                table: "Inventories",
                type: "integer",
                nullable: true,
                comment: "库存余量",
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true,
                oldComment: "当前真实的可用库存余量");

            migrationBuilder.AlterColumn<int>(
                name: "ReservedQuantity",
                table: "Inventories",
                type: "integer",
                nullable: true,
                comment: "预占库存数量",
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true,
                oldComment: "已下单但未支付的冻结/预占库存数量");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductId",
                table: "Inventories",
                type: "uuid",
                nullable: true,
                comment: "商品外键",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "关联的商品 ID");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "Inventories",
                type: "timestamp with time zone",
                nullable: true,
                comment: "修改时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录最后修改时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "ModifiedBy",
                table: "Inventories",
                type: "uuid",
                nullable: true,
                comment: "修改者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录最后修改者ID");

            migrationBuilder.AlterColumn<int>(
                name: "LowStockThreshold",
                table: "Inventories",
                type: "integer",
                nullable: true,
                comment: "库存阈值",
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true,
                oldComment: "触发低库存自动提醒的阈值高度");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Inventories",
                type: "boolean",
                nullable: true,
                comment: "是否删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "软删除标记：true表示已删除");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "Inventories",
                type: "timestamp with time zone",
                nullable: true,
                comment: "删除时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录删除时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeletedBy",
                table: "Inventories",
                type: "uuid",
                nullable: true,
                comment: "删除者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录删除者ID");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "Inventories",
                type: "timestamp with time zone",
                nullable: true,
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录创建时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "Inventories",
                type: "uuid",
                nullable: true,
                comment: "创建者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录创建者ID");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Inventories",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuidv7()",
                comment: "主键",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "uuidv7()",
                oldComment: "库存记录唯一标识");

            migrationBuilder.AlterColumn<string>(
                name: "Xml",
                table: "DataProtectionKeys",
                type: "text",
                nullable: true,
                comment: "密钥",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "密钥的 XML 序列化数据，包含加密密钥材料");

            migrationBuilder.AlterColumn<string>(
                name: "FriendlyName",
                table: "DataProtectionKeys",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true,
                comment: "密钥名称",
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255,
                oldNullable: true,
                oldComment: "密钥的友好名称，用于标识密钥的用途或环境");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "DataProtectionKeys",
                type: "integer",
                nullable: false,
                comment: "主键",
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "主键标识符")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<uint>(
                name: "xmin",
                table: "Categories",
                type: "xid",
                rowVersion: true,
                nullable: false,
                comment: "版本号",
                oldClrType: typeof(uint),
                oldType: "xid",
                oldRowVersion: true,
                oldComment: "版本号：用于乐观并发控制，每次更新自动递增");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categories",
                type: "character varying(64)",
                maxLength: 64,
                nullable: true,
                comment: "名称",
                oldClrType: typeof(string),
                oldType: "character varying(64)",
                oldMaxLength: 64,
                oldNullable: true,
                oldComment: "分类名称：商品的类别名称，最大长度64字符");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "Categories",
                type: "timestamp with time zone",
                nullable: true,
                comment: "修改时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录最后修改时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "ModifiedBy",
                table: "Categories",
                type: "uuid",
                nullable: true,
                comment: "修改者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录最后修改者ID");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Categories",
                type: "boolean",
                nullable: true,
                comment: "是否删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "软删除标记：true表示已删除");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "Categories",
                type: "timestamp with time zone",
                nullable: true,
                comment: "删除时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录删除时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeletedBy",
                table: "Categories",
                type: "uuid",
                nullable: true,
                comment: "删除者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录删除者ID");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "Categories",
                type: "timestamp with time zone",
                nullable: true,
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录创建时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "Categories",
                type: "uuid",
                nullable: true,
                comment: "创建者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录创建者ID");

            migrationBuilder.AlterColumn<string>(
                name: "Color",
                table: "Categories",
                type: "text",
                nullable: true,
                comment: "颜色如 #FF5733",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "颜色代码：用于前端展示分类标签的颜色，如 #FF5733");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Categories",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuidv7()",
                comment: "主键",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "uuidv7()",
                oldComment: "主键ID：分类的唯一标识");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "CartItems",
                type: "uuid",
                nullable: true,
                comment: "用户外键",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "关联的用户 ID");

            migrationBuilder.AlterColumn<bool>(
                name: "Selected",
                table: "CartItems",
                type: "boolean",
                nullable: true,
                comment: "是否选中",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "标记该商品当前是否在结算清单中处于勾选/激活状态");

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "CartItems",
                type: "integer",
                nullable: true,
                comment: "数量",
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true,
                oldComment: "用户预备购买的商品件数");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductId",
                table: "CartItems",
                type: "uuid",
                nullable: true,
                comment: "商品外键",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "关联的商品 ID");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "CartItems",
                type: "timestamp with time zone",
                nullable: true,
                comment: "修改时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录最后修改时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "ModifiedBy",
                table: "CartItems",
                type: "uuid",
                nullable: true,
                comment: "修改者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录最后修改者ID");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "CartItems",
                type: "boolean",
                nullable: true,
                comment: "是否删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "软删除标记：true表示已删除");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "CartItems",
                type: "timestamp with time zone",
                nullable: true,
                comment: "删除时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录删除时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeletedBy",
                table: "CartItems",
                type: "uuid",
                nullable: true,
                comment: "删除者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录删除者ID");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "CartItems",
                type: "timestamp with time zone",
                nullable: true,
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录创建时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "CartItems",
                type: "uuid",
                nullable: true,
                comment: "创建者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录创建者ID");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "CartItems",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuidv7()",
                comment: "主键",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "uuidv7()",
                oldComment: "购物车记录唯一标识");

            migrationBuilder.AlterColumn<string>(
                name: "Path",
                table: "Attachments",
                type: "text",
                nullable: true,
                comment: "附件路径",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "存储路径：附件在服务器或云存储上的路径");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "Attachments",
                type: "timestamp with time zone",
                nullable: true,
                comment: "修改时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录最后修改时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "ModifiedBy",
                table: "Attachments",
                type: "uuid",
                nullable: true,
                comment: "修改者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录最后修改者ID");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Attachments",
                type: "boolean",
                nullable: true,
                comment: "是否删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "软删除标记：true表示已删除");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "Attachments",
                type: "timestamp with time zone",
                nullable: true,
                comment: "删除时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录删除时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeletedBy",
                table: "Attachments",
                type: "uuid",
                nullable: true,
                comment: "删除者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录删除者ID");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "Attachments",
                type: "timestamp with time zone",
                nullable: true,
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录创建时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "Attachments",
                type: "uuid",
                nullable: true,
                comment: "创建者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录创建者ID");

            migrationBuilder.AlterColumn<int>(
                name: "Kind",
                table: "Attachments",
                type: "integer",
                nullable: false,
                comment: "附件类型：0：用户头像小图,；1：用户头像原图,；2：商品中图,；3：商品原图",
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "附件类型：0=用户头像小图, 1=用户头像原图, 2=商品中图, 3=商品原图");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Attachments",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuidv7()",
                comment: "主键",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "uuidv7()",
                oldComment: "主键ID：附件的唯一标识");

            migrationBuilder.AlterColumn<long>(
                name: "Pcode",
                table: "AreaCodes",
                type: "bigint",
                nullable: true,
                comment: "父级区划代码",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "父级行政区划代码 (自关联外键)");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "AreaCodes",
                type: "timestamp with time zone",
                nullable: true,
                comment: "修改时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录最后修改时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "ModifiedBy",
                table: "AreaCodes",
                type: "uuid",
                nullable: true,
                comment: "修改者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录最后修改者ID");

            migrationBuilder.AlterColumn<short>(
                name: "Level",
                table: "AreaCodes",
                type: "smallint",
                nullable: true,
                comment: "行政级别: 1：省；2：地级；3：县级；4：乡级； 5：村级",
                oldClrType: typeof(short),
                oldType: "smallint",
                oldNullable: true,
                oldComment: "行政级别 (1-5): 1=省级, 2=地级, 3=县级, 4=乡级, 5=村级");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "AreaCodes",
                type: "boolean",
                nullable: true,
                comment: "是否删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "软删除标记：true表示已删除");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "AreaCodes",
                type: "timestamp with time zone",
                nullable: true,
                comment: "删除时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录删除时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeletedBy",
                table: "AreaCodes",
                type: "uuid",
                nullable: true,
                comment: "删除者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录删除者ID");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "AreaCodes",
                type: "timestamp with time zone",
                nullable: true,
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录创建时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "AreaCodes",
                type: "uuid",
                nullable: true,
                comment: "创建者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录创建者ID");

            migrationBuilder.AlterColumn<long>(
                name: "Code",
                table: "AreaCodes",
                type: "bigint",
                nullable: false,
                comment: "主键",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "行政区划代码 (主键)，遵循国家标准 (如 GB/T 2260)");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Addresses",
                type: "uuid",
                nullable: true,
                comment: "用户外键",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "关联的用户 ID");

            migrationBuilder.AlterColumn<string>(
                name: "StreetAddress",
                table: "Addresses",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true,
                comment: "详细地址",
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256,
                oldNullable: true,
                oldComment: "详细街道/门牌地址描述");

            migrationBuilder.AlterColumn<string>(
                name: "RecipientName",
                table: "Addresses",
                type: "character varying(64)",
                maxLength: 64,
                nullable: true,
                comment: "收货人姓名",
                oldClrType: typeof(string),
                oldType: "character varying(64)",
                oldMaxLength: 64,
                oldNullable: true,
                oldComment: "收货联系人姓名");

            migrationBuilder.AlterColumn<string>(
                name: "Province",
                table: "Addresses",
                type: "character varying(32)",
                maxLength: 32,
                nullable: true,
                comment: "省",
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32,
                oldNullable: true,
                oldComment: "一级行政区划分 (省/自治区/直辖市)");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Addresses",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                comment: "收货联系号码",
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true,
                oldComment: "收货联系人电话号码");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "Addresses",
                type: "timestamp with time zone",
                nullable: true,
                comment: "修改时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录最后修改时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "ModifiedBy",
                table: "Addresses",
                type: "uuid",
                nullable: true,
                comment: "修改者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录最后修改者ID");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Addresses",
                type: "boolean",
                nullable: true,
                comment: "是否删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "软删除标记：true表示已删除");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDefault",
                table: "Addresses",
                type: "boolean",
                nullable: true,
                comment: "默认地址",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "是否设为该用户的默认首选收货地址");

            migrationBuilder.AlterColumn<string>(
                name: "District",
                table: "Addresses",
                type: "character varying(32)",
                maxLength: 32,
                nullable: true,
                comment: "区",
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32,
                oldNullable: true,
                oldComment: "三级行政区划分 (区/县)");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "Addresses",
                type: "timestamp with time zone",
                nullable: true,
                comment: "删除时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录删除时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeletedBy",
                table: "Addresses",
                type: "uuid",
                nullable: true,
                comment: "删除者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录删除者ID");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "Addresses",
                type: "timestamp with time zone",
                nullable: true,
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "记录创建时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "Addresses",
                type: "uuid",
                nullable: true,
                comment: "创建者",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "记录创建者ID");

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "Addresses",
                type: "character varying(32)",
                maxLength: 32,
                nullable: true,
                comment: "市",
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32,
                oldNullable: true,
                oldComment: "二级行政区划分 (城市)");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Addresses",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuidv7()",
                comment: "主键",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "uuidv7()",
                oldComment: "地址记录唯一标识");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterTable(
                name: "WebAuthnCredentials",
                comment: "WebAuthn凭据表：存储用户的 WebAuthn (FIDO2) 认证凭证，用于实现无密码登录。",
                oldComment: "凭据表");

            migrationBuilder.AlterTable(
                name: "UserTokens",
                comment: "用户令牌表：存储用户的身份验证令牌、刷新令牌或第三方登录令牌。",
                oldComment: "用户令牌表");

            migrationBuilder.AlterTable(
                name: "UserSessions",
                comment: "用户会话表：记录用户的登录会话信息，用于设备管理、安全审计和推送通知。",
                oldComment: "用户会话表");

            migrationBuilder.AlterTable(
                name: "Users",
                comment: "用户核心表：存储系统用户的账户信息、个人资料及安全凭证。",
                oldComment: "用户表");

            migrationBuilder.AlterTable(
                name: "UserRoles",
                comment: "用户角色关联表：用于实现用户与角色的多对多关系。",
                oldComment: "用户角色表");

            migrationBuilder.AlterTable(
                name: "UserLogins",
                comment: "用户外部登录表：存储用户关联的第三方登录提供商信息（如 Google, Microsoft, Facebook 等）。",
                oldComment: "用户登录表");

            migrationBuilder.AlterTable(
                name: "UserClaims",
                comment: "用户声明表：存储用户特定的声明数据（如权限、自定义属性等。",
                oldComment: "用户声明表");

            migrationBuilder.AlterTable(
                name: "SystemPrompts",
                comment: "系统提示词表：存储 AI 系统提示词及其版本配置的主表",
                oldComment: "系统提示词表");

            migrationBuilder.AlterTable(
                name: "Roles",
                comment: "角色表：用于系统权限管理的角色定义",
                oldComment: "角色表");

            migrationBuilder.AlterTable(
                name: "RoleClaims",
                comment: "角色声明表：存储角色关联的声明数据（如权限标识、自定义属性等）。",
                oldComment: "角色声明表");

            migrationBuilder.AlterTable(
                name: "PushNotificationSubscriptions",
                comment: "推送通知订阅表：存储用户设备接收推送通知所需的凭证（如 Endpoint, P256dh, Auth）和订阅状态。",
                oldComment: "推送通知订阅表");

            migrationBuilder.AlterTable(
                name: "Products",
                comment: "产品核心表：存储电商商品的详细信息、价格策略、多格式描述及用于 AI 语义搜索的向量数据。",
                oldComment: "商品表");

            migrationBuilder.AlterTable(
                name: "ProductImages",
                comment: "商品图片关联表：存储商品的多媒体展示资源，支持多图展示、主图标记及排序展示。",
                oldComment: "商品图片表");

            migrationBuilder.AlterTable(
                name: "Payments",
                comment: "支付流水表：记录用户对订单进行的每一笔支付尝试及其最终状态。",
                oldComment: "支付流水表");

            migrationBuilder.AlterTable(
                name: "Orders",
                comment: "订单主表：电商交易的核心记录，维护订单生命周期状态及金额明细。",
                oldComment: "订单表");

            migrationBuilder.AlterTable(
                name: "OrderItems",
                comment: "订单明细表：记录订单中每一项商品的详细快照及购买数量。",
                oldComment: "订单子表");

            migrationBuilder.AlterTable(
                name: "Inventories",
                comment: "商品库存表：维护商品实时库存、占用库存及库存报警阈值。",
                oldComment: "商品库存表");

            migrationBuilder.AlterTable(
                name: "DataProtectionKeys",
                comment: "数据保护系统的密钥存储表：存储 ASP.NET Core DataProtection 的密钥环，用于在服务器重启或集群环境下保持 Cookie 和 Token 有效性",
                oldComment: "密钥存储表");

            migrationBuilder.AlterTable(
                name: "Categories",
                comment: "商品分类表：用于管理商品的类别和标签",
                oldComment: "商品分类表");

            migrationBuilder.AlterTable(
                name: "CartItems",
                comment: "购物车明细表：记录用户添加到结算清单的商品、数量及勾选状态。",
                oldComment: "购物车");

            migrationBuilder.AlterTable(
                name: "Attachments",
                comment: "附件表：存储系统中的文件引用信息",
                oldComment: "附件表");

            migrationBuilder.AlterTable(
                name: "Addresses",
                comment: "用户收货地址表：存储用户的收货联系人、电话及多级行政区划详细地址。",
                oldComment: "用户收货地址表");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "WebAuthnCredentials",
                type: "uuid",
                nullable: true,
                comment: "关联用户的唯一标识符",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "用户外键");

            migrationBuilder.AlterColumn<int[]>(
                name: "Transports",
                table: "WebAuthnCredentials",
                type: "integer[]",
                nullable: true,
                comment: "认证器支持的传输方式 (USB, NFC, BLE 等)",
                oldClrType: typeof(int[]),
                oldType: "integer[]",
                oldNullable: true,
                oldComment: "传输方式 (0：usb，1：nfc，2：ble，3：smart-card，4：hybrid，5，internal");

            migrationBuilder.AlterColumn<long>(
                name: "SignCount",
                table: "WebAuthnCredentials",
                type: "bigint",
                nullable: true,
                comment: "签名计数器，用于防止重放攻击",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "签名计数器");

            migrationBuilder.AlterColumn<byte[]>(
                name: "PublicKey",
                table: "WebAuthnCredentials",
                type: "bytea",
                nullable: true,
                comment: "用户的公钥 (COSE Key 格式)，用于验证签名",
                oldClrType: typeof(byte[]),
                oldType: "bytea",
                oldNullable: true,
                oldComment: "公钥");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "WebAuthnCredentials",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录最后修改时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "修改时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "ModifiedBy",
                table: "WebAuthnCredentials",
                type: "uuid",
                nullable: true,
                comment: "记录最后修改者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "修改者");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "WebAuthnCredentials",
                type: "boolean",
                nullable: true,
                comment: "软删除标记：true表示已删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "是否删除");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "WebAuthnCredentials",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录删除时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "删除时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeletedBy",
                table: "WebAuthnCredentials",
                type: "uuid",
                nullable: true,
                comment: "记录删除者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "删除者");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "WebAuthnCredentials",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "WebAuthnCredentials",
                type: "uuid",
                nullable: true,
                comment: "记录创建者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "创建者");

            migrationBuilder.AlterColumn<byte[]>(
                name: "Id",
                table: "WebAuthnCredentials",
                type: "bytea",
                nullable: false,
                comment: "凭证的唯一标识符 (Credential ID)",
                oldClrType: typeof(byte[]),
                oldType: "bytea",
                oldComment: "主键");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "UserTokens",
                type: "text",
                nullable: true,
                comment: "令牌的具体值（敏感数据，通常经过哈希处理或加密）",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "令牌值");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "UserTokens",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录最后修改时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "修改时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "ModifiedBy",
                table: "UserTokens",
                type: "uuid",
                nullable: true,
                comment: "记录最后修改者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "修改者");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "UserTokens",
                type: "boolean",
                nullable: true,
                comment: "软删除标记：true表示已删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "软删除");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "UserTokens",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录删除时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "删除时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeletedBy",
                table: "UserTokens",
                type: "uuid",
                nullable: true,
                comment: "记录删除者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "删除者");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "UserTokens",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "UserTokens",
                type: "uuid",
                nullable: true,
                comment: "记录创建者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "创建者");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "UserTokens",
                type: "text",
                nullable: false,
                comment: "令牌名称（主键的一部分）：例如 'SecurityStamp' 或 'AccessToken'",
                oldClrType: typeof(string),
                oldType: "text",
                oldComment: "名称（主键）：例如 'SecurityStamp' 或 'AccessToken'");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "UserTokens",
                type: "text",
                nullable: false,
                comment: "令牌提供商名称（主键的一部分）：例如 'AspNetCore.Identity' 或 'Google'",
                oldClrType: typeof(string),
                oldType: "text",
                oldComment: "提供商（主键）：例如 'AspNetCore.Identity' 或 'Google'");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "UserTokens",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuidv7()",
                comment: "用户ID（主键的一部分）：关联到 Users 表",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "uuidv7()",
                oldComment: "用户外键");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "UserSessions",
                type: "uuid",
                nullable: true,
                comment: "关联用户的 ID (外键)",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "用话外键");

            migrationBuilder.AlterColumn<long>(
                name: "StartedOn",
                table: "UserSessions",
                type: "bigint",
                nullable: true,
                comment: "会话开始时间 (Unix 时间戳，单位：秒)",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "开始时间");

            migrationBuilder.AlterColumn<string>(
                name: "SignalRConnectionId",
                table: "UserSessions",
                type: "text",
                nullable: true,
                comment: "SignalR 连接 ID，用于实时消息推送",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "连接ID");

            migrationBuilder.AlterColumn<long>(
                name: "RenewedOn",
                table: "UserSessions",
                type: "bigint",
                nullable: true,
                comment: "会话最后续期时间 (Unix 时间戳，单位：秒)",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "续期时间");

            migrationBuilder.AlterColumn<bool>(
                name: "Privileged",
                table: "UserSessions",
                type: "boolean",
                nullable: true,
                comment: "特权访问标记：指示该会话是否拥有高权限（如管理员操作）",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "访问标记");

            migrationBuilder.AlterColumn<int>(
                name: "PlatformType",
                table: "UserSessions",
                type: "integer",
                nullable: true,
                comment: "客户端应用平台类型（如：Web, iOS, Android, Windows）",
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true,
                oldComment: "客户端应用平台类型（如：0：Web, 1：Ios,2：MacOS,3：Linux, 4：Android,5：Windows）");

            migrationBuilder.AlterColumn<int>(
                name: "NotificationStatus",
                table: "UserSessions",
                type: "integer",
                nullable: true,
                comment: "推送通知状态：0=未配置, 1=允许, 2=静音",
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true,
                oldComment: "推送通知状态，0：未配置；1：允许；2：静音");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "UserSessions",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录最后修改时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "修改时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "ModifiedBy",
                table: "UserSessions",
                type: "uuid",
                nullable: true,
                comment: "记录最后修改者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "修改者");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "UserSessions",
                type: "boolean",
                nullable: true,
                comment: "软删除标记：true表示已删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "是否删除");

            migrationBuilder.AlterColumn<string>(
                name: "IP",
                table: "UserSessions",
                type: "text",
                nullable: true,
                comment: "用户会话的 IP 地址",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "IP");

            migrationBuilder.AlterColumn<string>(
                name: "DeviceInfo",
                table: "UserSessions",
                type: "text",
                nullable: true,
                comment: "设备详细信息（浏览器、操作系统、设备型号等）",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "设备详细信息");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "UserSessions",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录删除时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "删除时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeletedBy",
                table: "UserSessions",
                type: "uuid",
                nullable: true,
                comment: "记录删除者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "删除者");

            migrationBuilder.AlterColumn<string>(
                name: "CultureName",
                table: "UserSessions",
                type: "text",
                nullable: true,
                comment: "用户在该会话中选择的语言文化代码 (如：zh-CN, en-US)",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "当前语言");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "UserSessions",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "UserSessions",
                type: "uuid",
                nullable: true,
                comment: "记录创建者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "创建者");

            migrationBuilder.AlterColumn<string>(
                name: "AppVersion",
                table: "UserSessions",
                type: "text",
                nullable: true,
                comment: "客户端应用程序的版本号",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "版本号");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "UserSessions",
                type: "text",
                nullable: true,
                comment: "基于 IP 地址解析的地理位置信息",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "地理位置信息");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "UserSessions",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuidv7()",
                comment: "会话的唯一标识符 (主键)",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "uuidv7()",
                oldComment: "主键");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "TwoFactorTokenRequestedOn",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true,
                comment: "双因素认证 (2FA) 令牌的最后请求时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "双因素认证 (2FA) 时间");

            migrationBuilder.AlterColumn<string>(
                name: "SecurityStamp",
                table: "Users",
                type: "text",
                nullable: true,
                comment: "安全戳：当用户凭据变更（如改密、删登录）时更改，用于使旧令牌失效",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "安全戳");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ResetPasswordTokenRequestedOn",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true,
                comment: "重置密码令牌的最后请求时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "重置密码时间");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "PhoneNumberTokenRequestedOn",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true,
                comment: "手机号验证令牌的最后请求时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "手机号时间");

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "Users",
                type: "text",
                nullable: true,
                comment: "密码的加盐哈希值",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "密码");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "OtpRequestedOn",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true,
                comment: "一次性密码 (OTP) 的最后请求时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "一次性密码 (OTP) 的请求时间");

            migrationBuilder.AlterColumn<string>(
                name: "NormalizedUserName",
                table: "Users",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true,
                comment: "标准化用户名 (用于索引和查找)",
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256,
                oldNullable: true,
                oldComment: "标准化用户名");

            migrationBuilder.AlterColumn<string>(
                name: "NormalizedEmail",
                table: "Users",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true,
                comment: "标准化电子邮件地址",
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256,
                oldNullable: true,
                oldComment: "邮件地址");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录最后修改时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "修改时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "ModifiedBy",
                table: "Users",
                type: "uuid",
                nullable: true,
                comment: "记录最后修改者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "修改者");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "LockoutEnd",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true,
                comment: "锁定期结束时间 (UTC)。如果为过去时间或空，表示未锁定",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "锁定期结束时间");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Users",
                type: "boolean",
                nullable: true,
                comment: "软删除标记：true表示已删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "软删除");

            migrationBuilder.AlterColumn<bool>(
                name: "HasProfilePicture",
                table: "Users",
                type: "boolean",
                nullable: true,
                comment: "是否拥有头像标记",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "是否有头像");

            migrationBuilder.AlterColumn<int>(
                name: "Gender",
                table: "Users",
                type: "integer",
                nullable: true,
                comment: "用户性别",
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true,
                oldComment: "性别");

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "Users",
                type: "text",
                nullable: true,
                comment: "用户的全名",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "全名");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "EmailTokenRequestedOn",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true,
                comment: "邮箱验证/修改令牌的最后请求时间，用于安全校验",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "邮箱验证时间");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true,
                comment: "电子邮件地址",
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256,
                oldNullable: true,
                oldComment: "邮件地址");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ElevatedAccessTokenRequestedOn",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true,
                comment: "高权限访问令牌 (Elevated Access) 的最后请求时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "Elevated Access请求时间");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录删除时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "除时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeletedBy",
                table: "Users",
                type: "uuid",
                nullable: true,
                comment: "记录删除者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "删除者");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "Users",
                type: "uuid",
                nullable: true,
                comment: "记录创建者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "创建者");

            migrationBuilder.AlterColumn<string>(
                name: "ConcurrencyStamp",
                table: "Users",
                type: "text",
                nullable: true,
                comment: "并发戳：用于乐观并发控制，每次持久化到数据库时更改",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "并发戳");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "BirthDate",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true,
                comment: "用户出生日期",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "出生日期");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "UserRoles",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录最后修改时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "修改时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "ModifiedBy",
                table: "UserRoles",
                type: "uuid",
                nullable: true,
                comment: "记录最后修改者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "修改者");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "UserRoles",
                type: "boolean",
                nullable: true,
                comment: "软删除标记：true表示已删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "软删除");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "UserRoles",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录删除时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "删除时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeletedBy",
                table: "UserRoles",
                type: "uuid",
                nullable: true,
                comment: "记录删除者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "删除者");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "UserRoles",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "UserRoles",
                type: "uuid",
                nullable: true,
                comment: "记录创建者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "创建者");

            migrationBuilder.AlterColumn<Guid>(
                name: "RoleId",
                table: "UserRoles",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuidv7()",
                comment: "角色ID（主键的一部分）：关联到 Roles 表",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "uuidv7()",
                oldComment: "角色外键");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "UserRoles",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuidv7()",
                comment: "用户ID（主键的一部分）：关联到 Users 表",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "uuidv7()",
                oldComment: "用户外键");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "UserLogins",
                type: "uuid",
                nullable: false,
                comment: "用户表主键Id：关联Users表Id字段",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "用户外键");

            migrationBuilder.AlterColumn<string>(
                name: "ProviderDisplayName",
                table: "UserLogins",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true,
                comment: "登录提供商的显示名称",
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128,
                oldNullable: true,
                oldComment: "提供商名称");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "UserLogins",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录最后修改时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "修改时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "ModifiedBy",
                table: "UserLogins",
                type: "uuid",
                nullable: true,
                comment: "记录最后修改者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "修改者");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "UserLogins",
                type: "boolean",
                nullable: true,
                comment: "软删除标记：true表示已删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "软删除");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "UserLogins",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录删除时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "删除时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeletedBy",
                table: "UserLogins",
                type: "uuid",
                nullable: true,
                comment: "记录删除者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "删除者");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "UserLogins",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "UserLogins",
                type: "uuid",
                nullable: true,
                comment: "记录创建者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "创建者");

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "UserLogins",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                comment: "提供商端的用户唯一标识符（Provider User ID）",
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128,
                oldComment: "提供商");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "UserLogins",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                comment: "登录提供商名称（例如：'Google', 'Facebook', 'Microsoft'）",
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128,
                oldComment: "提供商名称（例如：'Google', 'Facebook', 'Microsoft'）");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "UserClaims",
                type: "uuid",
                nullable: false,
                comment: "用户表主键Id：关联Users表Id字段",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "用户外键");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "UserClaims",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录最后修改时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "修改时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "ModifiedBy",
                table: "UserClaims",
                type: "uuid",
                nullable: true,
                comment: "记录最后修改者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "修改者");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "UserClaims",
                type: "boolean",
                nullable: true,
                comment: "软删除标记：true表示已删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "软删除");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "UserClaims",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录删除时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "删除时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeletedBy",
                table: "UserClaims",
                type: "uuid",
                nullable: true,
                comment: "记录删除者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "删除者");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "UserClaims",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "UserClaims",
                type: "uuid",
                nullable: true,
                comment: "记录创建者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "创建者");

            migrationBuilder.AlterColumn<string>(
                name: "ClaimValue",
                table: "UserClaims",
                type: "character varying(1024)",
                maxLength: 1024,
                nullable: true,
                comment: "声明的具体值（例如：'Admin', 'HR', 'John Doe'）",
                oldClrType: typeof(string),
                oldType: "character varying(1024)",
                oldMaxLength: 1024,
                oldNullable: true,
                oldComment: "声明值");

            migrationBuilder.AlterColumn<string>(
                name: "ClaimType",
                table: "UserClaims",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true,
                comment: "声明的类型（例如：'Permission.Read', 'Department', 'FullName'）",
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256,
                oldNullable: true,
                oldComment: "声明类型");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "TodoItems",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录最后修改时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "修改时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "ModifiedBy",
                table: "TodoItems",
                type: "uuid",
                nullable: true,
                comment: "记录最后修改者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "修改者");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "TodoItems",
                type: "boolean",
                nullable: true,
                comment: "软删除标记：true表示已删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "是否删除");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "TodoItems",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录删除时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "删除时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeletedBy",
                table: "TodoItems",
                type: "uuid",
                nullable: true,
                comment: "记录删除者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "删除者");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "TodoItems",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "TodoItems",
                type: "uuid",
                nullable: true,
                comment: "记录创建者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "创建者");

            migrationBuilder.AlterColumn<int>(
                name: "PromptKind",
                table: "SystemPrompts",
                type: "integer",
                nullable: true,
                comment: "提示词的类型枚举 (PromptKind)",
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true,
                oldComment: "类别，0：Support");

            migrationBuilder.AlterColumn<string>(
                name: "Markdown",
                table: "SystemPrompts",
                type: "text",
                nullable: true,
                comment: "提示词的内容 (Markdown 格式)",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "内容");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "SystemPrompts",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuidv7()",
                comment: "系统提示词的唯一标识符 (GUID)",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "uuidv7()",
                oldComment: "主键");

            migrationBuilder.AlterColumn<string>(
                name: "NormalizedName",
                table: "Roles",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                comment: "标准化名称：用于数据库查询的大写名称",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true,
                oldComment: "标准化名称");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Roles",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                comment: "角色名称：如 SuperAdmin, Demo 等",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true,
                oldComment: "名称");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "Roles",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录最后修改时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "修改时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "ModifiedBy",
                table: "Roles",
                type: "uuid",
                nullable: true,
                comment: "记录最后修改者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "修改者");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Roles",
                type: "boolean",
                nullable: true,
                comment: "软删除标记：true表示已删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "软删除");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "Roles",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录删除时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "删除时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeletedBy",
                table: "Roles",
                type: "uuid",
                nullable: true,
                comment: "记录删除者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "删除者");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "Roles",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "Roles",
                type: "uuid",
                nullable: true,
                comment: "记录创建者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "创建者");

            migrationBuilder.AlterColumn<string>(
                name: "ConcurrencyStamp",
                table: "Roles",
                type: "text",
                nullable: true,
                comment: "并发标记：用于乐观锁控制",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "并发标记");

            migrationBuilder.AlterColumn<Guid>(
                name: "RoleId",
                table: "RoleClaims",
                type: "uuid",
                nullable: false,
                comment: "关联的角色 ID：外键，指向该声明所属的角色",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "角色外键");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "RoleClaims",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录最后修改时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "修改时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "ModifiedBy",
                table: "RoleClaims",
                type: "uuid",
                nullable: true,
                comment: "记录最后修改者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "修改者");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "RoleClaims",
                type: "boolean",
                nullable: true,
                comment: "软删除标记：true表示已删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "软删除");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "RoleClaims",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录删除时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "删除时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeletedBy",
                table: "RoleClaims",
                type: "uuid",
                nullable: true,
                comment: "记录删除者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "删除者");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "RoleClaims",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "RoleClaims",
                type: "uuid",
                nullable: true,
                comment: "记录创建者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "创建者");

            migrationBuilder.AlterColumn<string>(
                name: "ClaimValue",
                table: "RoleClaims",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true,
                comment: "声明值：权限的具体数值或标识，例如 '-1' 代表无限制，或具体的功能代码",
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255,
                oldNullable: true,
                oldComment: "声明值");

            migrationBuilder.AlterColumn<string>(
                name: "ClaimType",
                table: "RoleClaims",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true,
                comment: "声明类型：定义权限的种类，例如 'MaxPrivilegedSessions' 或 'Features'",
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255,
                oldNullable: true,
                oldComment: "声明类型");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserSessionId",
                table: "PushNotificationSubscriptions",
                type: "uuid",
                nullable: true,
                comment: "关联的用户会话 ID (外键)，用于追踪订阅来源设备",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "用户会话外键");

            migrationBuilder.AlterColumn<string[]>(
                name: "Tags",
                table: "PushNotificationSubscriptions",
                type: "text[]",
                nullable: true,
                comment: "订阅标签数组 (JSON)，用于按主题过滤推送消息",
                oldClrType: typeof(string[]),
                oldType: "text[]",
                oldNullable: true,
                oldComment: "订阅标签数组[\"news\", \"alerts\"]");

            migrationBuilder.AlterColumn<long>(
                name: "RenewedOn",
                table: "PushNotificationSubscriptions",
                type: "bigint",
                nullable: true,
                comment: "订阅最后续期时间 (Unix 时间戳，单位：秒)",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "订阅续期时间 （秒）");

            migrationBuilder.AlterColumn<string>(
                name: "PushChannel",
                table: "PushNotificationSubscriptions",
                type: "text",
                nullable: true,
                comment: "推送服务提供的订阅 URL (Endpoint) 或渠道标识",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "推送渠道");

            migrationBuilder.AlterColumn<string>(
                name: "P256dh",
                table: "PushNotificationSubscriptions",
                type: "text",
                nullable: true,
                comment: "Web Push 用户代理公钥 (P-256dh)，用于消息加密",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "加密公钥");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "PushNotificationSubscriptions",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录最后修改时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "修改时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "ModifiedBy",
                table: "PushNotificationSubscriptions",
                type: "uuid",
                nullable: true,
                comment: "记录最后修改者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "修改者");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "PushNotificationSubscriptions",
                type: "boolean",
                nullable: true,
                comment: "软删除标记：true表示已删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "是否删除");

            migrationBuilder.AlterColumn<long>(
                name: "ExpirationTime",
                table: "PushNotificationSubscriptions",
                type: "bigint",
                nullable: true,
                comment: "订阅过期时间 (Unix 时间戳，单位：秒)",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "订阅过期时间（秒）");

            migrationBuilder.AlterColumn<string>(
                name: "Endpoint",
                table: "PushNotificationSubscriptions",
                type: "text",
                nullable: true,
                comment: "完整的推送消息发送 Endpoint URL",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "推送地址");

            migrationBuilder.AlterColumn<string>(
                name: "DeviceId",
                table: "PushNotificationSubscriptions",
                type: "text",
                nullable: true,
                comment: "设备的唯一标识符 (Device ID)",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "设备标识符");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "PushNotificationSubscriptions",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录删除时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "删除时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeletedBy",
                table: "PushNotificationSubscriptions",
                type: "uuid",
                nullable: true,
                comment: "记录删除者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "删除者");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "PushNotificationSubscriptions",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "PushNotificationSubscriptions",
                type: "uuid",
                nullable: true,
                comment: "记录创建者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "创建者");

            migrationBuilder.AlterColumn<string>(
                name: "Auth",
                table: "PushNotificationSubscriptions",
                type: "text",
                nullable: true,
                comment: "Web Push 认证密钥 (Auth Secret)，用于生成加密盐",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "认证密钥");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "PushNotificationSubscriptions",
                type: "integer",
                nullable: false,
                comment: "订阅记录的主键 ID",
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "主键")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<uint>(
                name: "xmin",
                table: "Products",
                type: "xid",
                rowVersion: true,
                nullable: false,
                comment: "行版本号 (用于乐观并发控制)",
                oldClrType: typeof(uint),
                oldType: "xid",
                oldRowVersion: true,
                oldComment: "版本号");

            migrationBuilder.AlterColumn<int>(
                name: "ShortId",
                table: "Products",
                type: "integer",
                nullable: false,
                comment: "用于生成友好 URL 的短整型 ID",
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "商品短ID");

            migrationBuilder.AlterColumn<string>(
                name: "PrimaryImageAltText",
                table: "Products",
                type: "text",
                nullable: true,
                comment: "主图片的替代文本 (Alt Text)",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "主图片 Alt Text");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Products",
                type: "numeric(18,3)",
                precision: 18,
                scale: 3,
                nullable: true,
                comment: "产品价格 (十进制)",
                oldClrType: typeof(decimal),
                oldType: "numeric(18,3)",
                oldPrecision: 18,
                oldScale: 3,
                oldNullable: true,
                oldComment: "价格");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Products",
                type: "character varying(64)",
                maxLength: 64,
                nullable: true,
                comment: "产品名称 (最大长度 64 字符)",
                oldClrType: typeof(string),
                oldType: "character varying(64)",
                oldMaxLength: 64,
                oldNullable: true,
                oldComment: "名称");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "Products",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录最后修改时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "修改时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "ModifiedBy",
                table: "Products",
                type: "uuid",
                nullable: true,
                comment: "记录最后修改者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "修改者");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Products",
                type: "boolean",
                nullable: true,
                comment: "软删除标记：true表示已删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "是否删除");

            migrationBuilder.AlterColumn<bool>(
                name: "HasPrimaryImage",
                table: "Products",
                type: "boolean",
                nullable: true,
                comment: "标记该产品是否拥有主图",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "拥有主图片");

            migrationBuilder.AlterColumn<Vector>(
                name: "Embedding",
                table: "Products",
                type: "vector(768)",
                nullable: true,
                comment: "用于语义搜索的向量嵌入 (Pgvector)",
                oldClrType: typeof(Vector),
                oldType: "vector(768)",
                oldNullable: true,
                oldComment: "向量");

            migrationBuilder.AlterColumn<string>(
                name: "DescriptionText",
                table: "Products",
                type: "character varying(4096)",
                maxLength: 4096,
                nullable: true,
                comment: "产品的纯文本描述，用于搜索或摘要",
                oldClrType: typeof(string),
                oldType: "character varying(4096)",
                oldMaxLength: 4096,
                oldNullable: true,
                oldComment: "纯文本描述");

            migrationBuilder.AlterColumn<string>(
                name: "DescriptionHTML",
                table: "Products",
                type: "character varying(4096)",
                maxLength: 4096,
                nullable: true,
                comment: "产品的 HTML 格式描述",
                oldClrType: typeof(string),
                oldType: "character varying(4096)",
                oldMaxLength: 4096,
                oldNullable: true,
                oldComment: "HTML描述");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "Products",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录删除时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "删除时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeletedBy",
                table: "Products",
                type: "uuid",
                nullable: true,
                comment: "记录删除者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "删除者");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "Products",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "Products",
                type: "uuid",
                nullable: true,
                comment: "记录创建者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "创建者");

            migrationBuilder.AlterColumn<Guid>(
                name: "CategoryId",
                table: "Products",
                type: "uuid",
                nullable: true,
                comment: "关联的分类 ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "类别外键");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Products",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuidv7()",
                comment: "产品的唯一标识符 (GUID)",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "uuidv7()",
                oldComment: "主键");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "ProductReviews",
                type: "uuid",
                nullable: true,
                comment: "发表评论的用户 ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "用户外键");

            migrationBuilder.AlterColumn<short>(
                name: "Rating",
                table: "ProductReviews",
                type: "smallint",
                nullable: true,
                comment: "商品评分 (取值范围 1-5)",
                oldClrType: typeof(short),
                oldType: "smallint",
                oldNullable: true,
                oldComment: "评分 (取值范围 1-5)");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductId",
                table: "ProductReviews",
                type: "uuid",
                nullable: true,
                comment: "被评价的商品 ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "商品外键");

            migrationBuilder.AlterColumn<Guid>(
                name: "OrderId",
                table: "ProductReviews",
                type: "uuid",
                nullable: true,
                comment: "关联的订单 ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "订单外键");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "ProductReviews",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录最后修改时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "修改时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "ModifiedBy",
                table: "ProductReviews",
                type: "uuid",
                nullable: true,
                comment: "记录最后修改者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "修改者");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "ProductReviews",
                type: "boolean",
                nullable: true,
                comment: "软删除标记：true表示已删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "是否删除");

            migrationBuilder.AlterColumn<bool>(
                name: "IsAnonymous",
                table: "ProductReviews",
                type: "boolean",
                nullable: true,
                comment: "是否启用匿名方式显示评价",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "是否匿名评论");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "ProductReviews",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录删除时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "删除时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeletedBy",
                table: "ProductReviews",
                type: "uuid",
                nullable: true,
                comment: "记录删除者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "删除者");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "ProductReviews",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "ProductReviews",
                type: "uuid",
                nullable: true,
                comment: "记录创建者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "创建者");

            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "ProductReviews",
                type: "character varying(1024)",
                maxLength: 1024,
                nullable: true,
                comment: "用户发表的评价正文内容",
                oldClrType: typeof(string),
                oldType: "character varying(1024)",
                oldMaxLength: 1024,
                oldNullable: true,
                oldComment: "评论内容");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "ProductReviews",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuidv7()",
                comment: "评价记录唯一标识",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "uuidv7()",
                oldComment: "主键");

            migrationBuilder.AlterColumn<int>(
                name: "SortOrder",
                table: "ProductImages",
                type: "integer",
                nullable: true,
                comment: "图片在展示列表中的排序权重 (升序排列)",
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true,
                oldComment: "排序");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductId",
                table: "ProductImages",
                type: "uuid",
                nullable: true,
                comment: "关联的商品 ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "商品主键");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "ProductImages",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录最后修改时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "修改时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "ModifiedBy",
                table: "ProductImages",
                type: "uuid",
                nullable: true,
                comment: "记录最后修改者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "修改者");

            migrationBuilder.AlterColumn<bool>(
                name: "IsPrimary",
                table: "ProductImages",
                type: "boolean",
                nullable: true,
                comment: "是否将此图片设为商品封面主图",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "是否封面主图");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "ProductImages",
                type: "boolean",
                nullable: true,
                comment: "软删除标记：true表示已删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "是否删除");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "ProductImages",
                type: "character varying(512)",
                maxLength: 512,
                nullable: true,
                comment: "图片素材的存储 URL 路径",
                oldClrType: typeof(string),
                oldType: "character varying(512)",
                oldMaxLength: 512,
                oldNullable: true,
                oldComment: "图片路径");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "ProductImages",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录删除时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "删除时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeletedBy",
                table: "ProductImages",
                type: "uuid",
                nullable: true,
                comment: "记录删除者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "删除者");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "ProductImages",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "ProductImages",
                type: "uuid",
                nullable: true,
                comment: "记录创建者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "创建者");

            migrationBuilder.AlterColumn<string>(
                name: "AltText",
                table: "ProductImages",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true,
                comment: "图片替代文本 (Alt Text)，用于 SEO 和无障碍显示",
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128,
                oldNullable: true,
                oldComment: "Alt Text");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "ProductImages",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuidv7()",
                comment: "图片记录唯一标识",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "uuidv7()",
                oldComment: "主键");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Payments",
                type: "uuid",
                nullable: true,
                comment: "执行该支付流水记录的用户 ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "用户外键");

            migrationBuilder.AlterColumn<string>(
                name: "TransactionId",
                table: "Payments",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true,
                comment: "三方支付机构返回的全局唯一交易参考单号",
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128,
                oldNullable: true,
                oldComment: "交易单号");

            migrationBuilder.AlterColumn<short>(
                name: "Status",
                table: "Payments",
                type: "smallint",
                nullable: true,
                comment: "该笔支付流水当前的处理状态枚举",
                oldClrType: typeof(short),
                oldType: "smallint",
                oldNullable: true,
                oldComment: "支付状态（0:待支付, 1:成功, 2:失败, 3:已退款）");

            migrationBuilder.AlterColumn<string>(
                name: "PaymentMethod",
                table: "Payments",
                type: "character varying(32)",
                maxLength: 32,
                nullable: true,
                comment: "支付方式/渠道名称 (如：Alipay, WeChatPay)",
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32,
                oldNullable: true,
                oldComment: "支付方式 (如：Alipay, WeChatPay)");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "PaidOn",
                table: "Payments",
                type: "timestamp with time zone",
                nullable: true,
                comment: "支付状态被标记为成功的时间点",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "支付时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "OrderId",
                table: "Payments",
                type: "uuid",
                nullable: true,
                comment: "关联的所属订单 ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "订单外键");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "Payments",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录最后修改时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "修改时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "ModifiedBy",
                table: "Payments",
                type: "uuid",
                nullable: true,
                comment: "记录最后修改者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "修改者");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Payments",
                type: "boolean",
                nullable: true,
                comment: "软删除标记：true表示已删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "是否删除");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "Payments",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录删除时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "删除时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeletedBy",
                table: "Payments",
                type: "uuid",
                nullable: true,
                comment: "记录删除者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "删除者");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "Payments",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "Payments",
                type: "uuid",
                nullable: true,
                comment: "记录创建者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "创建者");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Payments",
                type: "numeric(18,3)",
                precision: 18,
                scale: 3,
                nullable: true,
                comment: "本次支付流水的实付金额",
                oldClrType: typeof(decimal),
                oldType: "numeric(18,3)",
                oldPrecision: 18,
                oldScale: 3,
                oldNullable: true,
                oldComment: "实付金额");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Payments",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuidv7()",
                comment: "支付记录唯一标识",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "uuidv7()",
                oldComment: "主键");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Orders",
                type: "uuid",
                nullable: true,
                comment: "关联的下单用户 ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "用户外键");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalAmount",
                table: "Orders",
                type: "numeric(18,3)",
                precision: 18,
                scale: 3,
                nullable: true,
                comment: "商品原始销售总价",
                oldClrType: typeof(decimal),
                oldType: "numeric(18,3)",
                oldPrecision: 18,
                oldScale: 3,
                oldNullable: true,
                oldComment: "总价");

            migrationBuilder.AlterColumn<short>(
                name: "Status",
                table: "Orders",
                type: "smallint",
                nullable: true,
                comment: "0:待付款, 1:已付款, 2:已发货, 3:已完成, 4:已取消, 5:退款中",
                oldClrType: typeof(short),
                oldType: "smallint",
                oldNullable: true,
                oldComment: "订单状态：0:待付款, 1:已付款, 2:已发货, 3:已完成, 4:已取消, 5:退款中");

            migrationBuilder.AlterColumn<decimal>(
                name: "ShippingFee",
                table: "Orders",
                type: "numeric(18,3)",
                precision: 18,
                scale: 3,
                nullable: true,
                comment: "订单物流配送费用",
                oldClrType: typeof(decimal),
                oldType: "numeric(18,3)",
                oldPrecision: 18,
                oldScale: 3,
                oldNullable: true,
                oldComment: "配送费用");

            migrationBuilder.AlterColumn<string>(
                name: "Remark",
                table: "Orders",
                type: "character varying(512)",
                maxLength: 512,
                nullable: true,
                comment: "用户提供的订单留言或特殊备注说明",
                oldClrType: typeof(string),
                oldType: "character varying(512)",
                oldMaxLength: 512,
                oldNullable: true,
                oldComment: "备注");

            migrationBuilder.AlterColumn<decimal>(
                name: "PayableAmount",
                table: "Orders",
                type: "numeric(18,3)",
                precision: 18,
                scale: 3,
                nullable: true,
                comment: "实付应结的总金额 (含运费并扣除优惠)",
                oldClrType: typeof(decimal),
                oldType: "numeric(18,3)",
                oldPrecision: 18,
                oldScale: 3,
                oldNullable: true,
                oldComment: "实际支付总金额");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "PaidOn",
                table: "Orders",
                type: "timestamp with time zone",
                nullable: true,
                comment: "该订单由于支付成功而被确认的时间戳",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "支付时间");

            migrationBuilder.AlterColumn<string>(
                name: "OrderNo",
                table: "Orders",
                type: "character varying(32)",
                maxLength: 32,
                nullable: true,
                comment: "全局唯一业务订单编号",
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32,
                oldNullable: true,
                oldComment: "订单编号");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "Orders",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录最后修改时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "修改时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "ModifiedBy",
                table: "Orders",
                type: "uuid",
                nullable: true,
                comment: "记录最后修改者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "修改者");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Orders",
                type: "boolean",
                nullable: true,
                comment: "软删除标记：true表示已删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "是否删除");

            migrationBuilder.AlterColumn<decimal>(
                name: "DiscountAmount",
                table: "Orders",
                type: "numeric(18,3)",
                precision: 18,
                scale: 3,
                nullable: true,
                comment: "优惠抵扣的总金额",
                oldClrType: typeof(decimal),
                oldType: "numeric(18,3)",
                oldPrecision: 18,
                oldScale: 3,
                oldNullable: true,
                oldComment: "抵扣总金额");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "Orders",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录删除时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "删除时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeletedBy",
                table: "Orders",
                type: "uuid",
                nullable: true,
                comment: "记录删除者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "删除者");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "Orders",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "Orders",
                type: "uuid",
                nullable: true,
                comment: "记录创建者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "创建者");

            migrationBuilder.AlterColumn<Guid>(
                name: "AddressId",
                table: "Orders",
                type: "uuid",
                nullable: true,
                comment: "关联的配送地址 ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "收货地址外键");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Orders",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuidv7()",
                comment: "订单记录唯一标识",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "uuidv7()",
                oldComment: "主键");

            migrationBuilder.AlterColumn<decimal>(
                name: "UnitPrice",
                table: "OrderItems",
                type: "numeric(18,3)",
                precision: 18,
                scale: 3,
                nullable: true,
                comment: "下单时刻存储的成交单价快照",
                oldClrType: typeof(decimal),
                oldType: "numeric(18,3)",
                oldPrecision: 18,
                oldScale: 3,
                oldNullable: true,
                oldComment: "单价");

            migrationBuilder.AlterColumn<decimal>(
                name: "SubTotal",
                table: "OrderItems",
                type: "numeric(18,3)",
                precision: 18,
                scale: 3,
                nullable: true,
                comment: "该订单项对应的合计金额小计",
                oldClrType: typeof(decimal),
                oldType: "numeric(18,3)",
                oldPrecision: 18,
                oldScale: 3,
                oldNullable: true,
                oldComment: "总金额");

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "OrderItems",
                type: "integer",
                nullable: true,
                comment: "用户购买商品的选择数量",
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true,
                oldComment: "数量");

            migrationBuilder.AlterColumn<string>(
                name: "ProductName",
                table: "OrderItems",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true,
                comment: "下单时刻存储的商品名称属性快照",
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128,
                oldNullable: true,
                oldComment: "商品名称");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductId",
                table: "OrderItems",
                type: "uuid",
                nullable: true,
                comment: "关联的商品 ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "商品外键");

            migrationBuilder.AlterColumn<string>(
                name: "PrimaryImageAltText",
                table: "OrderItems",
                type: "text",
                nullable: true,
                comment: "下单时刻存储的图片替代文本快照",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "图片地址");

            migrationBuilder.AlterColumn<Guid>(
                name: "OrderId",
                table: "OrderItems",
                type: "uuid",
                nullable: true,
                comment: "关联的主订单 ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "订单外键");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "OrderItems",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录最后修改时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "修改时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "ModifiedBy",
                table: "OrderItems",
                type: "uuid",
                nullable: true,
                comment: "记录最后修改者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "修改者");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "OrderItems",
                type: "boolean",
                nullable: true,
                comment: "软删除标记：true表示已删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "是否删除");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "OrderItems",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录删除时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "删除时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeletedBy",
                table: "OrderItems",
                type: "uuid",
                nullable: true,
                comment: "记录删除者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "删除者");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "OrderItems",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "OrderItems",
                type: "uuid",
                nullable: true,
                comment: "记录创建者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "创建者");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "OrderItems",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuidv7()",
                comment: "订单项记录唯一标识",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "uuidv7()",
                oldComment: "主键");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "KnowledgeDocuments",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录最后修改时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "修改时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "ModifiedBy",
                table: "KnowledgeDocuments",
                type: "uuid",
                nullable: true,
                comment: "记录最后修改者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "修改者");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "KnowledgeDocuments",
                type: "boolean",
                nullable: true,
                comment: "软删除标记：true表示已删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "是否删除");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "KnowledgeDocuments",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录删除时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "删除时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeletedBy",
                table: "KnowledgeDocuments",
                type: "uuid",
                nullable: true,
                comment: "记录删除者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "删除者");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "KnowledgeDocuments",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "KnowledgeDocuments",
                type: "uuid",
                nullable: true,
                comment: "记录创建者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "创建者");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "KnowledgeDocumentChunks",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录最后修改时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "修改时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "ModifiedBy",
                table: "KnowledgeDocumentChunks",
                type: "uuid",
                nullable: true,
                comment: "记录最后修改者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "修改者");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "KnowledgeDocumentChunks",
                type: "boolean",
                nullable: true,
                comment: "软删除标记：true表示已删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "是否删除");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "KnowledgeDocumentChunks",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录删除时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "删除时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeletedBy",
                table: "KnowledgeDocumentChunks",
                type: "uuid",
                nullable: true,
                comment: "记录删除者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "删除者");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "KnowledgeDocumentChunks",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "KnowledgeDocumentChunks",
                type: "uuid",
                nullable: true,
                comment: "记录创建者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "创建者");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "KnowledgeBases",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录最后修改时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "修改时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "ModifiedBy",
                table: "KnowledgeBases",
                type: "uuid",
                nullable: true,
                comment: "记录最后修改者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "修改者");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "KnowledgeBases",
                type: "boolean",
                nullable: true,
                comment: "软删除标记：true表示已删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "是否删除");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "KnowledgeBases",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录删除时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "删除时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeletedBy",
                table: "KnowledgeBases",
                type: "uuid",
                nullable: true,
                comment: "记录删除者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "删除者");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "KnowledgeBases",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "KnowledgeBases",
                type: "uuid",
                nullable: true,
                comment: "记录创建者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "创建者");

            migrationBuilder.AlterColumn<int>(
                name: "StockQuantity",
                table: "Inventories",
                type: "integer",
                nullable: true,
                comment: "当前真实的可用库存余量",
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true,
                oldComment: "库存余量");

            migrationBuilder.AlterColumn<int>(
                name: "ReservedQuantity",
                table: "Inventories",
                type: "integer",
                nullable: true,
                comment: "已下单但未支付的冻结/预占库存数量",
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true,
                oldComment: "预占库存数量");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductId",
                table: "Inventories",
                type: "uuid",
                nullable: true,
                comment: "关联的商品 ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "商品外键");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "Inventories",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录最后修改时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "修改时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "ModifiedBy",
                table: "Inventories",
                type: "uuid",
                nullable: true,
                comment: "记录最后修改者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "修改者");

            migrationBuilder.AlterColumn<int>(
                name: "LowStockThreshold",
                table: "Inventories",
                type: "integer",
                nullable: true,
                comment: "触发低库存自动提醒的阈值高度",
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true,
                oldComment: "库存阈值");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Inventories",
                type: "boolean",
                nullable: true,
                comment: "软删除标记：true表示已删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "是否删除");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "Inventories",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录删除时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "删除时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeletedBy",
                table: "Inventories",
                type: "uuid",
                nullable: true,
                comment: "记录删除者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "删除者");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "Inventories",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "Inventories",
                type: "uuid",
                nullable: true,
                comment: "记录创建者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "创建者");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Inventories",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuidv7()",
                comment: "库存记录唯一标识",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "uuidv7()",
                oldComment: "主键");

            migrationBuilder.AlterColumn<string>(
                name: "Xml",
                table: "DataProtectionKeys",
                type: "text",
                nullable: true,
                comment: "密钥的 XML 序列化数据，包含加密密钥材料",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "密钥");

            migrationBuilder.AlterColumn<string>(
                name: "FriendlyName",
                table: "DataProtectionKeys",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true,
                comment: "密钥的友好名称，用于标识密钥的用途或环境",
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255,
                oldNullable: true,
                oldComment: "密钥名称");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "DataProtectionKeys",
                type: "integer",
                nullable: false,
                comment: "主键标识符",
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "主键")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<uint>(
                name: "xmin",
                table: "Categories",
                type: "xid",
                rowVersion: true,
                nullable: false,
                comment: "版本号：用于乐观并发控制，每次更新自动递增",
                oldClrType: typeof(uint),
                oldType: "xid",
                oldRowVersion: true,
                oldComment: "版本号");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categories",
                type: "character varying(64)",
                maxLength: 64,
                nullable: true,
                comment: "分类名称：商品的类别名称，最大长度64字符",
                oldClrType: typeof(string),
                oldType: "character varying(64)",
                oldMaxLength: 64,
                oldNullable: true,
                oldComment: "名称");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "Categories",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录最后修改时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "修改时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "ModifiedBy",
                table: "Categories",
                type: "uuid",
                nullable: true,
                comment: "记录最后修改者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "修改者");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Categories",
                type: "boolean",
                nullable: true,
                comment: "软删除标记：true表示已删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "是否删除");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "Categories",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录删除时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "删除时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeletedBy",
                table: "Categories",
                type: "uuid",
                nullable: true,
                comment: "记录删除者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "删除者");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "Categories",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "Categories",
                type: "uuid",
                nullable: true,
                comment: "记录创建者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "创建者");

            migrationBuilder.AlterColumn<string>(
                name: "Color",
                table: "Categories",
                type: "text",
                nullable: true,
                comment: "颜色代码：用于前端展示分类标签的颜色，如 #FF5733",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "颜色如 #FF5733");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Categories",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuidv7()",
                comment: "主键ID：分类的唯一标识",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "uuidv7()",
                oldComment: "主键");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "CartItems",
                type: "uuid",
                nullable: true,
                comment: "关联的用户 ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "用户外键");

            migrationBuilder.AlterColumn<bool>(
                name: "Selected",
                table: "CartItems",
                type: "boolean",
                nullable: true,
                comment: "标记该商品当前是否在结算清单中处于勾选/激活状态",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "是否选中");

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "CartItems",
                type: "integer",
                nullable: true,
                comment: "用户预备购买的商品件数",
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true,
                oldComment: "数量");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductId",
                table: "CartItems",
                type: "uuid",
                nullable: true,
                comment: "关联的商品 ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "商品外键");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "CartItems",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录最后修改时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "修改时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "ModifiedBy",
                table: "CartItems",
                type: "uuid",
                nullable: true,
                comment: "记录最后修改者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "修改者");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "CartItems",
                type: "boolean",
                nullable: true,
                comment: "软删除标记：true表示已删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "是否删除");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "CartItems",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录删除时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "删除时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeletedBy",
                table: "CartItems",
                type: "uuid",
                nullable: true,
                comment: "记录删除者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "删除者");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "CartItems",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "CartItems",
                type: "uuid",
                nullable: true,
                comment: "记录创建者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "创建者");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "CartItems",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuidv7()",
                comment: "购物车记录唯一标识",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "uuidv7()",
                oldComment: "主键");

            migrationBuilder.AlterColumn<string>(
                name: "Path",
                table: "Attachments",
                type: "text",
                nullable: true,
                comment: "存储路径：附件在服务器或云存储上的路径",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "附件路径");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "Attachments",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录最后修改时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "修改时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "ModifiedBy",
                table: "Attachments",
                type: "uuid",
                nullable: true,
                comment: "记录最后修改者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "修改者");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Attachments",
                type: "boolean",
                nullable: true,
                comment: "软删除标记：true表示已删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "是否删除");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "Attachments",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录删除时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "删除时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeletedBy",
                table: "Attachments",
                type: "uuid",
                nullable: true,
                comment: "记录删除者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "删除者");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "Attachments",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "Attachments",
                type: "uuid",
                nullable: true,
                comment: "记录创建者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "创建者");

            migrationBuilder.AlterColumn<int>(
                name: "Kind",
                table: "Attachments",
                type: "integer",
                nullable: false,
                comment: "附件类型：0=用户头像小图, 1=用户头像原图, 2=商品中图, 3=商品原图",
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "附件类型：0：用户头像小图,；1：用户头像原图,；2：商品中图,；3：商品原图");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Attachments",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuidv7()",
                comment: "主键ID：附件的唯一标识",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "uuidv7()",
                oldComment: "主键");

            migrationBuilder.AlterColumn<long>(
                name: "Pcode",
                table: "AreaCodes",
                type: "bigint",
                nullable: true,
                comment: "父级行政区划代码 (自关联外键)",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "父级区划代码");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "AreaCodes",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录最后修改时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "修改时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "ModifiedBy",
                table: "AreaCodes",
                type: "uuid",
                nullable: true,
                comment: "记录最后修改者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "修改者");

            migrationBuilder.AlterColumn<short>(
                name: "Level",
                table: "AreaCodes",
                type: "smallint",
                nullable: true,
                comment: "行政级别 (1-5): 1=省级, 2=地级, 3=县级, 4=乡级, 5=村级",
                oldClrType: typeof(short),
                oldType: "smallint",
                oldNullable: true,
                oldComment: "行政级别: 1：省；2：地级；3：县级；4：乡级； 5：村级");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "AreaCodes",
                type: "boolean",
                nullable: true,
                comment: "软删除标记：true表示已删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "是否删除");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "AreaCodes",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录删除时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "删除时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeletedBy",
                table: "AreaCodes",
                type: "uuid",
                nullable: true,
                comment: "记录删除者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "删除者");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "AreaCodes",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "AreaCodes",
                type: "uuid",
                nullable: true,
                comment: "记录创建者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "创建者");

            migrationBuilder.AlterColumn<long>(
                name: "Code",
                table: "AreaCodes",
                type: "bigint",
                nullable: false,
                comment: "行政区划代码 (主键)，遵循国家标准 (如 GB/T 2260)",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "主键");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Addresses",
                type: "uuid",
                nullable: true,
                comment: "关联的用户 ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "用户外键");

            migrationBuilder.AlterColumn<string>(
                name: "StreetAddress",
                table: "Addresses",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true,
                comment: "详细街道/门牌地址描述",
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256,
                oldNullable: true,
                oldComment: "详细地址");

            migrationBuilder.AlterColumn<string>(
                name: "RecipientName",
                table: "Addresses",
                type: "character varying(64)",
                maxLength: 64,
                nullable: true,
                comment: "收货联系人姓名",
                oldClrType: typeof(string),
                oldType: "character varying(64)",
                oldMaxLength: 64,
                oldNullable: true,
                oldComment: "收货人姓名");

            migrationBuilder.AlterColumn<string>(
                name: "Province",
                table: "Addresses",
                type: "character varying(32)",
                maxLength: 32,
                nullable: true,
                comment: "一级行政区划分 (省/自治区/直辖市)",
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32,
                oldNullable: true,
                oldComment: "省");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Addresses",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                comment: "收货联系人电话号码",
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true,
                oldComment: "收货联系号码");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "Addresses",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录最后修改时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "修改时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "ModifiedBy",
                table: "Addresses",
                type: "uuid",
                nullable: true,
                comment: "记录最后修改者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "修改者");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Addresses",
                type: "boolean",
                nullable: true,
                comment: "软删除标记：true表示已删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "是否删除");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDefault",
                table: "Addresses",
                type: "boolean",
                nullable: true,
                comment: "是否设为该用户的默认首选收货地址",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "默认地址");

            migrationBuilder.AlterColumn<string>(
                name: "District",
                table: "Addresses",
                type: "character varying(32)",
                maxLength: 32,
                nullable: true,
                comment: "三级行政区划分 (区/县)",
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32,
                oldNullable: true,
                oldComment: "区");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "Addresses",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录删除时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "删除时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeletedBy",
                table: "Addresses",
                type: "uuid",
                nullable: true,
                comment: "记录删除者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "删除者");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "Addresses",
                type: "timestamp with time zone",
                nullable: true,
                comment: "记录创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "Addresses",
                type: "uuid",
                nullable: true,
                comment: "记录创建者ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "创建者");

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "Addresses",
                type: "character varying(32)",
                maxLength: 32,
                nullable: true,
                comment: "二级行政区划分 (城市)",
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32,
                oldNullable: true,
                oldComment: "市");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Addresses",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuidv7()",
                comment: "地址记录唯一标识",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "uuidv7()",
                oldComment: "主键");
        }
    }
}
