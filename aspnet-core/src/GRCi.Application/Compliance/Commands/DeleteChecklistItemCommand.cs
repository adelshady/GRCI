using System;
using MediatR;

namespace GRCi.Compliance.Commands;

public class DeleteChecklistItemCommand : IRequest
{
    public Guid ItemId { get; set; }

    public DeleteChecklistItemCommand(Guid itemId) => ItemId = itemId;
}
