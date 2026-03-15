using System;
using System.Collections.Generic;
using System.Linq;
using GRCi.Compliance.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace GRCi.Compliance.ChecklistTemplates;

public class ChecklistTemplate : FullAuditedAggregateRoot<Guid>
{
    public string Code { get; private set; } = null!;
    public string NameEn { get; private set; } = null!;
    public string? NameAr { get; private set; }
    public string? DescriptionEn { get; private set; }
    public string? DescriptionAr { get; private set; }
    public Guid RegulatoryAgencyId { get; private set; }
    public ChecklistTemplateStatus Status { get; private set; }
    public int Version { get; private set; }

    public List<ChecklistItem> Items { get; private set; } = new();

    // EF Core constructor
    protected ChecklistTemplate() { }

    public ChecklistTemplate(
        Guid id,
        string code,
        string nameEn,
        string? nameAr,
        string? descriptionEn,
        string? descriptionAr,
        Guid regulatoryAgencyId) : base(id)
    {
        Code = Check.NotNullOrWhiteSpace(code, nameof(code));
        SetNameEn(nameEn);
        NameAr = nameAr?.Trim();
        DescriptionEn = descriptionEn?.Trim();
        DescriptionAr = descriptionAr?.Trim();
        RegulatoryAgencyId = regulatoryAgencyId != Guid.Empty
            ? regulatoryAgencyId
            : throw new ArgumentException("RegulatoryAgencyId must not be empty.", nameof(regulatoryAgencyId));
        Status = ChecklistTemplateStatus.Draft;
        Version = 1;
    }

    public void Update(
        string nameEn,
        string? nameAr,
        string? descriptionEn,
        string? descriptionAr,
        Guid regulatoryAgencyId)
    {
        GuardEditableStatus();
        SetNameEn(nameEn);
        NameAr = nameAr?.Trim();
        DescriptionEn = descriptionEn?.Trim();
        DescriptionAr = descriptionAr?.Trim();
        RegulatoryAgencyId = regulatoryAgencyId != Guid.Empty
            ? regulatoryAgencyId
            : throw new ArgumentException("RegulatoryAgencyId must not be empty.", nameof(regulatoryAgencyId));
    }

    public ChecklistItem AddItem(
        Guid itemId,
        string? sectionTitle,
        string requirementTextEn,
        string? requirementTextAr,
        ItemCriticality criticality,
        bool isMandatory,
        NotesRequirement notesRequiredWhen,
        int sortOrder)
    {
        GuardEditableStatus();
        var item = new ChecklistItem(itemId, Id, sectionTitle, requirementTextEn, requirementTextAr, criticality, isMandatory, notesRequiredWhen, sortOrder);
        Items.Add(item);
        return item;
    }

    public void UpdateItem(
        Guid itemId,
        string? sectionTitle,
        string requirementTextEn,
        string? requirementTextAr,
        ItemCriticality criticality,
        bool isMandatory,
        NotesRequirement notesRequiredWhen,
        int sortOrder)
    {
        GuardEditableStatus();
        var item = Items.FirstOrDefault(i => i.Id == itemId)
            ?? throw new BusinessException($"ChecklistItem with id {itemId} not found in this template.");
        item.Update(sectionTitle, requirementTextEn, requirementTextAr, criticality, isMandatory, notesRequiredWhen, sortOrder);
    }

    public void RemoveItem(Guid itemId)
    {
        GuardEditableStatus();
        var item = Items.FirstOrDefault(i => i.Id == itemId)
            ?? throw new BusinessException($"ChecklistItem with id {itemId} not found in this template.");
        item.Deactivate();
    }

    // Workflow transitions — called by WorkflowManager
    public void Submit()
    {
        if (Status != ChecklistTemplateStatus.Draft && Status != ChecklistTemplateStatus.Returned)
            throw new BusinessException($"Cannot submit template in status '{Status}'. Only Draft or Returned templates can be submitted.");

        if (!Items.Any(i => i.IsActive))
            throw new BusinessException("Template must have at least one active item before it can be submitted.");

        Status = ChecklistTemplateStatus.PendingApproval;
    }

    public void Approve()
    {
        if (Status != ChecklistTemplateStatus.PendingApproval)
            throw new BusinessException($"Cannot approve template in status '{Status}'. Only PendingApproval templates can be approved.");

        Status = ChecklistTemplateStatus.Approved;
    }

    public void Return()
    {
        if (Status != ChecklistTemplateStatus.PendingApproval)
            throw new BusinessException($"Cannot return template in status '{Status}'. Only PendingApproval templates can be returned.");

        Status = ChecklistTemplateStatus.Returned;
    }

    public void Resubmit()
    {
        if (Status != ChecklistTemplateStatus.Returned)
            throw new BusinessException($"Cannot resubmit template in status '{Status}'. Only Returned templates can be resubmitted.");

        if (!Items.Any(i => i.IsActive))
            throw new BusinessException("Template must have at least one active item before it can be resubmitted.");

        Version++;
        Status = ChecklistTemplateStatus.Draft;
    }

    public void Archive()
    {
        if (Status != ChecklistTemplateStatus.Approved)
            throw new BusinessException($"Cannot archive template in status '{Status}'. Only Approved templates can be archived.");

        Status = ChecklistTemplateStatus.Archived;
    }

    private void SetNameEn(string nameEn)
    {
        NameEn = Check.NotNullOrWhiteSpace(nameEn, nameof(nameEn), maxLength: 256);
    }

    private void GuardEditableStatus()
    {
        if (Status != ChecklistTemplateStatus.Draft && Status != ChecklistTemplateStatus.Returned)
            throw new BusinessException($"Template items cannot be modified when the template is in '{Status}' status. Only Draft or Returned templates can be edited.");
    }
}
