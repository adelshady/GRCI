using System;
using GRCi.Compliance.Dtos;
using MediatR;

namespace GRCi.Compliance.Queries;

public class GetChecklistTemplateByIdQuery : IRequest<ChecklistTemplateDto>
{
    public Guid Id { get; set; }
    public bool IncludeHistory { get; set; } = true;

    public GetChecklistTemplateByIdQuery(Guid id, bool includeHistory = true)
    {
        Id = id;
        IncludeHistory = includeHistory;
    }
}
