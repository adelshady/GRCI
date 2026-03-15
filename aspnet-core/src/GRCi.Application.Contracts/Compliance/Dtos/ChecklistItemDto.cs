using System;
using GRCi.Compliance.Enums;

namespace GRCi.Compliance.Dtos;

public class ChecklistItemDto
{
    public Guid Id { get; set; }
    public Guid TemplateId { get; set; }
    public string? SectionTitle { get; set; }
    public string RequirementTextEn { get; set; } = null!;
    public string? RequirementTextAr { get; set; }
    public ItemCriticality Criticality { get; set; }
    public int Weight { get; set; }
    public bool IsMandatory { get; set; }
    public NotesRequirement NotesRequiredWhen { get; set; }
    public int SortOrder { get; set; }
    public bool IsActive { get; set; }
}
