using System;
using System.ComponentModel.DataAnnotations;
using GRCi.Compliance.Dtos;
using GRCi.Compliance.Enums;
using MediatR;

namespace GRCi.Compliance.Commands;

public class UpdateChecklistItemCommand : IRequest<ChecklistItemDto>
{
    public Guid ItemId { get; set; }

    [MaxLength(256)]
    public string? SectionTitle { get; set; }

    [Required]
    [MaxLength(4000)]
    public string RequirementTextEn { get; set; } = null!;

    [MaxLength(4000)]
    public string? RequirementTextAr { get; set; }

    [Required]
    public ItemCriticality Criticality { get; set; }

    public bool IsMandatory { get; set; }

    public NotesRequirement NotesRequiredWhen { get; set; }

    public int SortOrder { get; set; }
}
