using System;
using System.Collections.Generic;
using GRCi.Compliance.Dtos;
using MediatR;

namespace GRCi.Compliance.Queries;

public class GetItemAttachmentsQuery : IRequest<List<AttachmentDto>>
{
    public string EntityType { get; set; }
    public Guid EntityId { get; set; }

    public GetItemAttachmentsQuery(string entityType, Guid entityId)
    {
        EntityType = entityType;
        EntityId = entityId;
    }
}
