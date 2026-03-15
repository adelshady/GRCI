using System;
using GRCi.Compliance.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace GRCi.Compliance.ChecklistTemplates;

public class ChecklistItem : Entity<Guid>
{
    public Guid TemplateId { get; private set; }
    public string? SectionTitle { get; private set; }
    public string RequirementTextEn { get; private set; } = null!;
    public string? RequirementTextAr { get; private set; }
    public ItemCriticality Criticality { get; private set; }
    public int Weight { get; private set; }
    public bool IsMandatory { get; private set; }
    public NotesRequirement NotesRequiredWhen { get; private set; }
    public int SortOrder { get; private set; }
    public bool IsActive { get; private set; }

    // EF Core constructor
    protected ChecklistItem() { }

    internal ChecklistItem(
        Guid id,
        Guid templateId,
        string? sectionTitle,
        string requirementTextEn,
        string? requirementTextAr,
        ItemCriticality criticality,
        bool isMandatory,
        NotesRequirement notesRequiredWhen,
        int sortOrder) : base(id)
    {
        TemplateId = templateId;
        SectionTitle = sectionTitle?.Trim();
        SetRequirementTextEn(requirementTextEn);
        RequirementTextAr = requirementTextAr?.Trim();
        Criticality = criticality;
        Weight = DeriveWeight(criticality);
        IsMandatory = isMandatory;
        NotesRequiredWhen = notesRequiredWhen;
        SortOrder = sortOrder;
        IsActive = true;
    }

    internal void Update(
        string? sectionTitle,
        string requirementTextEn,
        string? requirementTextAr,
        ItemCriticality criticality,
        bool isMandatory,
        NotesRequirement notesRequiredWhen,
        int sortOrder)
    {
        SectionTitle = sectionTitle?.Trim();
        SetRequirementTextEn(requirementTextEn);
        RequirementTextAr = requirementTextAr?.Trim();
        Criticality = criticality;
        Weight = DeriveWeight(criticality);
        IsMandatory = isMandatory;
        NotesRequiredWhen = notesRequiredWhen;
        SortOrder = sortOrder;
    }

    internal void Deactivate()
    {
        IsActive = false;
    }

    private void SetRequirementTextEn(string text)
    {
        RequirementTextEn = Check.NotNullOrWhiteSpace(text, nameof(RequirementTextEn), maxLength: 4000);
    }

    private static int DeriveWeight(ItemCriticality criticality) => criticality switch
    {
        ItemCriticality.Minor => 1,
        ItemCriticality.Medium => 2,
        ItemCriticality.Critical => 3,
        _ => throw new ArgumentOutOfRangeException(nameof(criticality), criticality, null)
    };
}
