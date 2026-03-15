using System;
using GRCi.Compliance.Dtos;
using MediatR;

namespace GRCi.Compliance.Commands;

public class UploadAttachmentCommand : IRequest<AttachmentDto>
{
    public string EntityType { get; set; } = null!;
    public Guid EntityId { get; set; }
    public string FileName { get; set; } = null!;
    public string ContentType { get; set; } = null!;
    public long Size { get; set; }
    public byte[] Content { get; set; } = Array.Empty<byte>();
}
