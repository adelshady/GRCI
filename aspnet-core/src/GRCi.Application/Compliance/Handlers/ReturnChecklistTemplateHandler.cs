using System.Threading;
using System.Threading.Tasks;
using GRCi.Compliance.Commands;
using GRCi.Compliance.Repositories;
using GRCi.Compliance.Services;
using MediatR;
using Volo.Abp;
using Volo.Abp.Users;

namespace GRCi.Compliance.Handlers;

public class ReturnChecklistTemplateHandler : IRequestHandler<ReturnChecklistTemplateCommand>
{
    private readonly IChecklistTemplateRepository _repository;
    private readonly IWorkflowManager _workflowManager;
    private readonly ICurrentUser _currentUser;

    public ReturnChecklistTemplateHandler(
        IChecklistTemplateRepository repository,
        IWorkflowManager workflowManager,
        ICurrentUser currentUser)
    {
        _repository = repository;
        _workflowManager = workflowManager;
        _currentUser = currentUser;
    }

    public async Task Handle(ReturnChecklistTemplateCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Comment))
            throw new BusinessException("A comment is required when returning a template.");

        var template = await _repository.GetAsync(request.TemplateId, cancellationToken: cancellationToken);

        var fromStatus = template.Status.ToString();
        template.Return();

        await _repository.UpdateAsync(template, autoSave: false, cancellationToken: cancellationToken);

        await _workflowManager.RecordTransitionAsync(
            "ChecklistTemplate",
            template.Id,
            "Return",
            fromStatus,
            template.Status.ToString(),
            _currentUser.Id,
            _currentUser.UserName,
            request.Comment);
    }
}
