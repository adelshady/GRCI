using System;
using Volo.Abp.Domain.Entities;

namespace GRCi.Compliance.Workflow;

public class WorkflowActionHistory : Entity<Guid>
{
    public string EntityType { get; private set; } = null!;
    public Guid EntityId { get; private set; }
    public string Action { get; private set; } = null!;
    public string FromStatus { get; private set; } = null!;
    public string ToStatus { get; private set; } = null!;
    public Guid? PerformedByUserId { get; private set; }
    public string? PerformedByUserName { get; private set; }
    public DateTime PerformedAt { get; private set; }
    public string? Comment { get; private set; }

    // EF Core constructor
    protected WorkflowActionHistory() { }

    public WorkflowActionHistory(
        Guid id,
        string entityType,
        Guid entityId,
        string action,
        string fromStatus,
        string toStatus,
        Guid? performedByUserId,
        string? performedByUserName,
        DateTime performedAt,
        string? comment) : base(id)
    {
        EntityType = entityType;
        EntityId = entityId;
        Action = action;
        FromStatus = fromStatus;
        ToStatus = toStatus;
        PerformedByUserId = performedByUserId;
        PerformedByUserName = performedByUserName;
        PerformedAt = performedAt;
        Comment = comment;
    }
}
