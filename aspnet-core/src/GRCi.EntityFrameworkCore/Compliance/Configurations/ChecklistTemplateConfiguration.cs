using GRCi.Compliance.ChecklistTemplates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace GRCi.EntityFrameworkCore.Compliance.Configurations;

public class ChecklistTemplateConfiguration : IEntityTypeConfiguration<ChecklistTemplate>
{
    public void Configure(EntityTypeBuilder<ChecklistTemplate> builder)
    {
        builder.ToTable(GRCiConsts.DbTablePrefix + "ChecklistTemplates", GRCiConsts.DbSchema);
        builder.ConfigureByConvention();

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Code)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(t => t.Code)
            .IsUnique();

        builder.Property(t => t.NameEn)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(t => t.NameAr)
            .HasMaxLength(256);

        builder.Property(t => t.DescriptionEn)
            .HasMaxLength(2000);

        builder.Property(t => t.DescriptionAr)
            .HasMaxLength(2000);

        builder.Property(t => t.Status)
            .IsRequired();

        builder.Property(t => t.Version)
            .IsRequired()
            .HasDefaultValue(1);

        builder.HasMany(t => t.Items)
            .WithOne()
            .HasForeignKey(i => i.TemplateId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
