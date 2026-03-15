using System;
using Volo.Abp.Domain.Entities;

namespace GRCi.Compliance.Attachments;

public class StoredFile : Entity<Guid>
{
    public string FileName { get; private set; } = null!;
    public string ContentType { get; private set; } = null!;
    public long Size { get; private set; }
    public string RelativePath { get; private set; } = null!;
    public Guid? UploadedByUserId { get; private set; }
    public DateTime UploadedAt { get; private set; }

    // EF Core constructor
    protected StoredFile() { }

    public StoredFile(
        Guid id,
        string fileName,
        string contentType,
        long size,
        string relativePath,
        Guid? uploadedByUserId,
        DateTime uploadedAt) : base(id)
    {
        FileName = fileName;
        ContentType = contentType;
        Size = size;
        RelativePath = relativePath;
        UploadedByUserId = uploadedByUserId;
        UploadedAt = uploadedAt;
    }
}
