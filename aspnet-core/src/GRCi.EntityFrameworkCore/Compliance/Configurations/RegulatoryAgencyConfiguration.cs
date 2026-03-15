using GRCi.Compliance.RegulatoryAgencies;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace GRCi.EntityFrameworkCore.Compliance.Configurations;

public class RegulatoryAgencyConfiguration : IEntityTypeConfiguration<RegulatoryAgency>
{
    public void Configure(EntityTypeBuilder<RegulatoryAgency> builder)
    {
        builder.ToTable("AppRegulatoryAgencies");
        builder.ConfigureByConvention();

        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(256);
        builder.Property(x => x.NameAr).HasMaxLength(256);
        builder.Property(x => x.Code).HasMaxLength(64);
    }
}
