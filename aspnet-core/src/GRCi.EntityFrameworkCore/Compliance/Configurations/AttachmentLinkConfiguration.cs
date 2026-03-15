using GRCi.Compliance.Attachments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace GRCi.EntityFrameworkCore.Compliance.Configurations;

public class AttachmentLinkConfiguration : IEntityTypeConfiguration<AttachmentLink>
{
    public void Configure(EntityTypeBuilder<AttachmentLink> builder)
    {
        builder.ToTable(GRCiConsts.DbTablePrefix + "AttachmentLinks", GRCiConsts.DbSchema);
        builder.ConfigureByConvention();

        builder.HasKey(l => l.Id);

        builder.Property(l => l.EntityType)
            .IsRequired()
            .HasMaxLength(128);

        builder.HasIndex(l => new { l.EntityType, l.EntityId });
        builder.HasIndex(l => l.FileId);
    }
}
