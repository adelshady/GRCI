using GRCi.Compliance.ChecklistTemplates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace GRCi.EntityFrameworkCore.Compliance.Configurations;

public class ChecklistItemConfiguration : IEntityTypeConfiguration<ChecklistItem>
{
    public void Configure(EntityTypeBuilder<ChecklistItem> builder)
    {
        builder.ToTable(GRCiConsts.DbTablePrefix + "ChecklistItems", GRCiConsts.DbSchema);
        builder.ConfigureByConvention();

        builder.HasKey(i => i.Id);

        builder.Property(i => i.SectionTitle)
            .HasMaxLength(256);

        builder.Property(i => i.RequirementTextEn)
            .IsRequired()
            .HasMaxLength(4000);

        builder.Property(i => i.RequirementTextAr)
            .HasMaxLength(4000);

        builder.Property(i => i.Criticality)
            .IsRequired();

        builder.Property(i => i.Weight)
            .IsRequired();

        builder.Property(i => i.IsMandatory)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(i => i.NotesRequiredWhen)
            .IsRequired();

        builder.Property(i => i.SortOrder)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(i => i.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.HasIndex(i => i.TemplateId);
        builder.HasIndex(i => new { i.TemplateId, i.IsActive });
    }
}
