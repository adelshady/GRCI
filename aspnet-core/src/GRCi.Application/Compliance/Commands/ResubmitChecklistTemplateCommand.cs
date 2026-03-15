using System;
using MediatR;

namespace GRCi.Compliance.Commands;

public class ResubmitChecklistTemplateCommand : IRequest
{
    public Guid TemplateId { get; set; }

    public ResubmitChecklistTemplateCommand(Guid templateId) => TemplateId = templateId;
}
