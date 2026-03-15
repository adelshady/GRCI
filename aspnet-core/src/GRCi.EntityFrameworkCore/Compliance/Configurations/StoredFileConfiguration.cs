using GRCi.Compliance.Attachments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace GRCi.EntityFrameworkCore.Compliance.Configurations;

public class StoredFileConfiguration : IEntityTypeConfiguration<StoredFile>
{
    public void Configure(EntityTypeBuilder<StoredFile> builder)
    {
        builder.ToTable(GRCiConsts.DbTablePrefix + "StoredFiles", GRCiConsts.DbSchema);
        builder.ConfigureByConvention();

        builder.HasKey(f => f.Id);

        builder.Property(f => f.FileName)
            .IsRequired()
            .HasMaxLength(512);

        builder.Property(f => f.ContentType)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(f => f.RelativePath)
            .IsRequired()
            .HasMaxLength(1024);

        builder.Property(f => f.Size)
            .IsRequired();
    }
}
