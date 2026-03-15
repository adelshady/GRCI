using System.Threading;
using System.Threading.Tasks;
using GRCi.Compliance.Commands;
using GRCi.Compliance.Enums;
using GRCi.Compliance.Repositories;
using GRCi.Compliance.Services;
using MediatR;
using Volo.Abp;
using Volo.Abp.Users;

namespace GRCi.Compliance.Handlers;

public class SubmitChecklistTemplateHandler : IRequestHandler<SubmitChecklistTemplateCommand>
{
    private readonly IChecklistTemplateRepository _repository;
    private readonly IWorkflowManager _workflowManager;
    private readonly ICurrentUser _currentUser;

    public SubmitChecklistTemplateHandler(
        IChecklistTemplateRepository repository,
        IWorkflowManager workflowManager,
        ICurrentUser currentUser)
    {
        _repository = repository;
        _workflowManager = workflowManager;
        _currentUser = currentUser;
    }

    public async Task Handle(SubmitChecklistTemplateCommand request, CancellationToken cancellationToken)
    {
        var template = await _repository.GetWithItemsAsync(request.TemplateId, cancellationToken)
            ?? throw new BusinessException($"ChecklistTemplate with id {request.TemplateId} not found.");

        var fromStatus = template.Status.ToString();
        template.Submit();

        await _repository.UpdateAsync(template, autoSave: false, cancellationToken: cancellationToken);

        await _workflowManager.RecordTransitionAsync(
            "ChecklistTemplate",
            template.Id,
            "Submit",
            fromStatus,
            template.Status.ToString(),
            _currentUser.Id,
            _currentUser.UserName);
    }
}
