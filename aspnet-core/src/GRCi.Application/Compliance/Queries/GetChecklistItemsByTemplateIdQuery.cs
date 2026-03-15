using System;
using System.Collections.Generic;
using GRCi.Compliance.Dtos;
using MediatR;

namespace GRCi.Compliance.Queries;

public class GetChecklistItemsByTemplateIdQuery : IRequest<List<ChecklistItemDto>>
{
    public Guid TemplateId { get; set; }
    public bool ActiveOnly { get; set; } = true;

    public GetChecklistItemsByTemplateIdQuery(Guid templateId, bool activeOnly = true)
    {
        TemplateId = templateId;
        ActiveOnly = activeOnly;
    }
}
