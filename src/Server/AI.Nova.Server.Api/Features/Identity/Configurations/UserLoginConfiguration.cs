namespace AI.Nova.Server.Api.Features.Identity.Configurations;

using AI.Nova.Server.Api.Features.Identity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class UserLoginConfiguration : IEntityTypeConfiguration<UserLogin>
{
    public void Configure(EntityTypeBuilder<UserLogin> builder)
    {
        builder.Property(uc => uc.UserId).HasComment("用户表主键Id：关联Users表Id字段");

        builder.Property(ul => ul.LoginProvider)
            .HasMaxLength(128) 
            .HasComment("登录提供商名称（例如：'Google', 'Facebook', 'Microsoft'）");

        builder.Property(ul => ul.ProviderKey)
            .HasMaxLength(128)
            .HasComment("提供商端的用户唯一标识符（Provider User ID）");

        builder.Property(ul => ul.ProviderDisplayName)
            .HasMaxLength(128)
            .HasComment("登录提供商的显示名称");
    }
}
