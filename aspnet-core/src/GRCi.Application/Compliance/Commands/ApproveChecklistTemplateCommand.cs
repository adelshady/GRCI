using System;
using MediatR;

namespace GRCi.Compliance.Commands;

public class ApproveChecklistTemplateCommand : IRequest
{
    public Guid TemplateId { get; set; }

    public ApproveChecklistTemplateCommand(Guid templateId) => TemplateId = templateId;
}
