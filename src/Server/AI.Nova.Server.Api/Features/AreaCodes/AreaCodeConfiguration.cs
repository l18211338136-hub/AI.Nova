namespace AI.Nova.Server.Api.Features.AreaCodes;

public class AreaCodeConfiguration : IEntityTypeConfiguration<AreaCode>
{
    public void Configure(EntityTypeBuilder<AreaCode> builder)
    {
        builder.HasOne<AreaCode>()
               .WithMany()
               .HasForeignKey(a => a.Pcode)
               .HasConstraintName("FK_AreaCode_ParentCode");
    }
}
