using System;
using MediatR;

namespace GRCi.Compliance.Commands;

public class DeleteAttachmentCommand : IRequest
{
    public Guid FileId { get; set; }

    public DeleteAttachmentCommand(Guid fileId) => FileId = fileId;
}
