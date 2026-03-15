using System;
using System.Collections.Generic;
using GRCi.Compliance.Dtos;
using MediatR;

namespace GRCi.Compliance.Queries;

public class GetWorkflowHistoryQuery : IRequest<List<WorkflowActionHistoryDto>>
{
    public string EntityType { get; set; }
    public Guid EntityId { get; set; }

    public GetWorkflowHistoryQuery(string entityType, Guid entityId)
    {
        EntityType = entityType;
        EntityId = entityId;
    }
}
