using System;
using System.ComponentModel.DataAnnotations;
using GRCi.Compliance.Dtos;
using MediatR;

namespace GRCi.Compliance.Commands;

public class CreateChecklistTemplateCommand : IRequest<ChecklistTemplateDto>
{
    [Required]
    [MaxLength(256)]
    public string NameEn { get; set; } = null!;

    [MaxLength(256)]
    public string? NameAr { get; set; }

    [MaxLength(2000)]
    public string? DescriptionEn { get; set; }

    [MaxLength(2000)]
    public string? DescriptionAr { get; set; }

    [Required]
    public Guid RegulatoryAgencyId { get; set; }
}
