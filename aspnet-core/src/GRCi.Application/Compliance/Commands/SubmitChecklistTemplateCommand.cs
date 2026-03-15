using System;
using MediatR;

namespace GRCi.Compliance.Commands;

public class SubmitChecklistTemplateCommand : IRequest
{
    public Guid TemplateId { get; set; }

    public SubmitChecklistTemplateCommand(Guid templateId) => TemplateId = templateId;
}
