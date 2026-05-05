using AI.Nova.Server.Api.Infrastructure.Data.Audit;
using Fido2NetLib.Objects;

namespace AI.Nova.Server.Api.Features.Identity.Models;

/// <summary>
/// This model is used by the Fido2 lib to store and retrieve the data of the browser credential api for `Web Authentication`.
/// <br />
/// More info: <see href="https://github.com/passwordless-lib/fido2-net-lib"/>
/// </summary>
/// <summary>
/// 存储用户的 WebAuthn (FIDO2) 认证凭证
/// </summary>
[Table("WebAuthnCredentials")]
[Comment("凭据表")] 
public class WebAuthnCredential : AuditEntity
{
    [Key]
    [Comment("主键")] 
    public required byte[] Id { get; set; }

    [Comment("用户外键")]
    public Guid? UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }

    [Comment("公钥")]
    public byte[]? PublicKey { get; set; }

    [Comment("签名计数器")]
    public uint? SignCount { get; set; }

    [Comment("传输方式 (0：usb，1：nfc，2：ble，3：smart-card，4：hybrid，5，internal")]
    public AuthenticatorTransport[]? Transports { get; set; }

    [Comment("指示该凭证是否具备备份资格")]
    public bool? IsBackupEligible { get; set; }

    [Comment("指示该凭证是否已被备份")]
    public bool? IsBackedUp { get; set; }

    [Comment("证明对象 (Attestation Object) 的原始 CBOR 数据")]
    public byte[]? AttestationObject { get; set; }

    [Comment("证明时的客户端数据 JSON 的原始二进制数据")]
    public byte[]? AttestationClientDataJson { get; set; }

    [Comment("关联到此凭证的用户的标识符")]
    public byte[]? UserHandle { get; set; }

    [Comment("证明数据的格式 (如 packed, tpm)")]
    public string? AttestationFormat { get; set; }

    [Comment("凭证注册的日期和时间")]
    public DateTimeOffset? RegDate { get; set; }

    [Comment("认证器的 AAGUID，用于识别型号")]
    public Guid? AaGuid { get; set; }
}

