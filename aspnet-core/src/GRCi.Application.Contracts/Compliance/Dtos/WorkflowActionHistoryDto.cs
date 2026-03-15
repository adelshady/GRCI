using System;

namespace GRCi.Compliance.Dtos;

public class WorkflowActionHistoryDto
{
    public Guid Id { get; set; }
    public string EntityType { get; set; } = null!;
    public Guid EntityId { get; set; }
    public string Action { get; set; } = null!;
    public string FromStatus { get; set; } = null!;
    public string ToStatus { get; set; } = null!;
    public Guid? PerformedByUserId { get; set; }
    public string? PerformedByUserName { get; set; }
    public DateTime PerformedAt { get; set; }
    public string? Comment { get; set; }
}
