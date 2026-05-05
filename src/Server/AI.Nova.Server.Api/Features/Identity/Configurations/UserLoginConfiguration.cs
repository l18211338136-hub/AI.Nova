namespace AI.Nova.Server.Api.Features.Identity.Configurations;

using AI.Nova.Server.Api.Features.Identity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class UserLoginConfiguration : IEntityTypeConfiguration<UserLogin>
{
    public void Configure(EntityTypeBuilder<UserLogin> builder)
    {
        builder.Property(uc => uc.UserId).HasComment("用户外键");

        builder.Property(ul => ul.LoginProvider)
            .HasMaxLength(128) 
            .HasComment("提供商名称（例如：'Google', 'Facebook', 'Microsoft'）");

        builder.Property(ul => ul.ProviderKey)
            .HasMaxLength(128)
            .HasComment("提供商");

        builder.Property(ul => ul.ProviderDisplayName)
            .HasMaxLength(128)
            .HasComment("提供商名称");
    }
}
