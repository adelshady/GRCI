using System;

namespace GRCi.Compliance.Dtos;

public class RegulatoryAgencyDto
{
    public Guid Id { get; set; }
    public string NameEn { get; set; } = null!;
    public string? NameAr { get; set; }
    public string? Code { get; set; }
    public bool IsActive { get; set; }
}
