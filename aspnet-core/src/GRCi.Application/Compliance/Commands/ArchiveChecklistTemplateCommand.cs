using System;
using MediatR;

namespace GRCi.Compliance.Commands;

public class ArchiveChecklistTemplateCommand : IRequest
{
    public Guid TemplateId { get; set; }

    public ArchiveChecklistTemplateCommand(Guid templateId) => TemplateId = templateId;
}
