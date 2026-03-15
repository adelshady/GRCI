using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace GRCi.Compliance.RegulatoryAgencies;

public class RegulatoryAgency : FullAuditedAggregateRoot<Guid>
{
    public string NameEn { get; private set; } = null!;
    public string? NameAr { get; private set; }
    public string? Code { get; private set; }
    public bool IsActive { get; private set; }

    protected RegulatoryAgency() { }

    public RegulatoryAgency(Guid id, string nameEn, string? nameAr, string? code) : base(id)
    {
        NameEn = Check.NotNullOrWhiteSpace(nameEn, nameof(nameEn), maxLength: 256);
        NameAr = nameAr?.Trim();
        Code = code?.Trim();
        IsActive = true;
    }

    public void Update(string nameEn, string? nameAr, string? code)
    {
        NameEn = Check.NotNullOrWhiteSpace(nameEn, nameof(nameEn), maxLength: 256);
        NameAr = nameAr?.Trim();
        Code = code?.Trim();
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
}
