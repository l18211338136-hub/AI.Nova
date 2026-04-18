namespace AI.Nova.Shared.Features.Attachments;

/// <summary>
/// 附件类型枚举
/// 用于区分不同业务场景下的文件存储类型
/// </summary>
public enum AttachmentKind
{
    /// <summary>
    /// 用户头像（小图）
    /// 尺寸：256*256px
    /// </summary>
    UserProfileImageSmall = 0,

    /// <summary>
    /// 用户头像（原图）
    /// 用于高清展示或重新裁剪
    /// </summary>
    UserProfileImageOriginal = 1,

    /// <summary>
    /// 商品主图（中图）
    /// 尺寸：512*512px (注意：原注释写的是515，建议确认是否为笔误)
    /// </summary>
    ProductPrimaryImageMedium = 2,

    /// <summary>
    /// 商品主图（原图）
    /// 用于高清展示
    /// </summary>
    ProductPrimaryImageOriginal = 3
}
