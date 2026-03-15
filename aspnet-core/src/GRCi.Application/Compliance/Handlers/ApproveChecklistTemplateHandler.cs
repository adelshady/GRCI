using System.Threading;
using System.Threading.Tasks;
using GRCi.Compliance.Commands;
using GRCi.Compliance.Repositories;
using GRCi.Compliance.Services;
using MediatR;
using Volo.Abp;
using Volo.Abp.Users;

namespace GRCi.Compliance.Handlers;

public class ApproveChecklistTemplateHandler : IRequestHandler<ApproveChecklistTemplateCommand>
{
    private readonly IChecklistTemplateRepository _repository;
    private readonly IWorkflowManager _workflowManager;
    private readonly ICurrentUser _currentUser;

    public ApproveChecklistTemplateHandler(
        IChecklistTemplateRepository repository,
        IWorkflowManager workflowManager,
        ICurrentUser currentUser)
    {
        _repository = repository;
        _workflowManager = workflowManager;
        _currentUser = currentUser;
    }

    public async Task Handle(ApproveChecklistTemplateCommand request, CancellationToken cancellationToken)
    {
        var template = await _repository.GetAsync(request.TemplateId, cancellationToken: cancellationToken);

        var fromStatus = template.Status.ToString();
        template.Approve();

        await _repository.UpdateAsync(template, autoSave: false, cancellationToken: cancellationToken);

        await _workflowManager.RecordTransitionAsync(
            "ChecklistTemplate",
            template.Id,
            "Approve",
            fromStatus,
            template.Status.ToString(),
            _currentUser.Id,
            _currentUser.UserName);
    }
}
