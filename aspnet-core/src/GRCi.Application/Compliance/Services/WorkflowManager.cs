using System;
using System.Threading.Tasks;
using GRCi.Compliance.ChecklistTemplates;
using GRCi.Compliance.Workflow;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace GRCi.Compliance.Services;

public class WorkflowManager : IWorkflowManager
{
    private readonly IRepository<WorkflowActionHistory, Guid> _historyRepository;
    private readonly IGuidGenerator _guidGenerator;

    public WorkflowManager(
        IRepository<WorkflowActionHistory, Guid> historyRepository,
        IGuidGenerator guidGenerator)
    {
        _historyRepository = historyRepository;
        _guidGenerator = guidGenerator;
    }

    public async Task RecordTransitionAsync(
        string entityType,
        Guid entityId,
        string action,
        string fromStatus,
        string toStatus,
        Guid? performedByUserId,
        string? performedByUserName,
        string? comment = null)
    {
        var history = new WorkflowActionHistory(
            _guidGenerator.Create(),
            entityType,
            entityId,
            action,
            fromStatus,
            toStatus,
            performedByUserId,
            performedByUserName,
            DateTime.UtcNow,
            comment);

        await _historyRepository.InsertAsync(history);
    }
}
