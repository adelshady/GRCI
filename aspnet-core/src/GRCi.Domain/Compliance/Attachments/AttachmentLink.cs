using System;
using Volo.Abp.Domain.Entities;

namespace GRCi.Compliance.Attachments;

public class AttachmentLink : Entity<Guid>
{
    public string EntityType { get; private set; } = null!;
    public Guid EntityId { get; private set; }
    public Guid FileId { get; private set; }
    public DateTime CreatedAt { get; private set; }

    // EF Core constructor
    protected AttachmentLink() { }

    public AttachmentLink(
        Guid id,
        string entityType,
        Guid entityId,
        Guid fileId,
        DateTime createdAt) : base(id)
    {
        EntityType = entityType;
        EntityId = entityId;
        FileId = fileId;
        CreatedAt = createdAt;
    }
}
