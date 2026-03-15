using System;
using GRCi.Compliance.Dtos;
using GRCi.Compliance.Enums;
using MediatR;
using Volo.Abp.Application.Dtos;

namespace GRCi.Compliance.Queries;

public class ListChecklistTemplatesQuery : IRequest<PagedResultDto<ChecklistTemplateDto>>
{
    public int SkipCount { get; set; } = 0;
    public int MaxResultCount { get; set; } = 10;
    public string? Search { get; set; }
    public ChecklistTemplateStatus? Status { get; set; }
    public Guid? RegulatoryAgencyId { get; set; }
}
