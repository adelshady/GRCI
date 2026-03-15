using System;
using System.Collections.Generic;
using GRCi.Compliance.Enums;

namespace GRCi.Compliance.Dtos;

public class ChecklistTemplateDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public string? NameAr { get; set; }
    public string? DescriptionEn { get; set; }
    public string? DescriptionAr { get; set; }
    public Guid RegulatoryAgencyId { get; set; }
    public ChecklistTemplateStatus Status { get; set; }
    public int Version { get; set; }
    public List<ChecklistItemDto> Items { get; set; } = new();
    public List<WorkflowActionHistoryDto>? WorkflowHistory { get; set; }
    public DateTime CreationTime { get; set; }
    public Guid? CreatorId { get; set; }
    public DateTime? LastModificationTime { get; set; }
}
