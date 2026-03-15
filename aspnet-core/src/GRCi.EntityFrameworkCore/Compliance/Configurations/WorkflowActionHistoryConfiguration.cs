using GRCi.Compliance.Workflow;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace GRCi.EntityFrameworkCore.Compliance.Configurations;

public class WorkflowActionHistoryConfiguration : IEntityTypeConfiguration<WorkflowActionHistory>
{
    public void Configure(EntityTypeBuilder<WorkflowActionHistory> builder)
    {
        builder.ToTable(GRCiConsts.DbTablePrefix + "WorkflowActionHistories", GRCiConsts.DbSchema);
        builder.ConfigureByConvention();

        builder.HasKey(h => h.Id);

        builder.Property(h => h.EntityType)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(h => h.Action)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(h => h.FromStatus)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(h => h.ToStatus)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(h => h.PerformedByUserName)
            .HasMaxLength(256);

        builder.Property(h => h.Comment)
            .HasMaxLength(2000);

        builder.HasIndex(h => new { h.EntityType, h.EntityId, h.PerformedAt });
    }
}
