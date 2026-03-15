using System;

namespace GRCi.Compliance.Dtos;

public class AttachmentDto
{
    public Guid FileId { get; set; }
    public Guid LinkId { get; set; }
    public string EntityType { get; set; } = null!;
    public Guid EntityId { get; set; }
    public string FileName { get; set; } = null!;
    public string ContentType { get; set; } = null!;
    public long Size { get; set; }
    public Guid? UploadedByUserId { get; set; }
    public DateTime UploadedAt { get; set; }
}
