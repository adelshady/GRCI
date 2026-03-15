using System;
using MediatR;

namespace GRCi.Compliance.Commands;

public class DeleteChecklistTemplateCommand : IRequest
{
    public Guid Id { get; set; }

    public DeleteChecklistTemplateCommand(Guid id) => Id = id;
}
