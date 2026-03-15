using System;
using System.Threading.Tasks;

namespace GRCi.Compliance.Services;

public interface IWorkflowManager
{
    Task RecordTransitionAsync(
        string entityType,
        Guid entityId,
        string action,
        string fromStatus,
        string toStatus,
        Guid? performedByUserId,
        string? performedByUserName,
        string? comment = null);
}
