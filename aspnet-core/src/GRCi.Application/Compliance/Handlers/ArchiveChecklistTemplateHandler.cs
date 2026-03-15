using System.Threading;
using System.Threading.Tasks;
using GRCi.Compliance.Commands;
using GRCi.Compliance.Repositories;
using GRCi.Compliance.Services;
using MediatR;
using Volo.Abp;
using Volo.Abp.Users;

namespace GRCi.Compliance.Handlers;

public class ArchiveChecklistTemplateHandler : IRequestHandler<ArchiveChecklistTemplateCommand>
{
    private readonly IChecklistTemplateRepository _repository;
    private readonly IWorkflowManager _workflowManager;
    private readonly ICurrentUser _currentUser;

    public ArchiveChecklistTemplateHandler(
        IChecklistTemplateRepository repository,
        IWorkflowManager workflowManager,
        ICurrentUser currentUser)
    {
        _repository = repository;
        _workflowManager = workflowManager;
        _currentUser = currentUser;
    }

    public async Task Handle(ArchiveChecklistTemplateCommand request, CancellationToken cancellationToken)
    {
        var template = await _repository.GetAsync(request.TemplateId, cancellationToken: cancellationToken);

        var fromStatus = template.Status.ToString();
        template.Archive();

        await _repository.UpdateAsync(template, autoSave: false, cancellationToken: cancellationToken);

        await _workflowManager.RecordTransitionAsync(
            "ChecklistTemplate",
            template.Id,
            "Archive",
            fromStatus,
            template.Status.ToString(),
            _currentUser.Id,
            _currentUser.UserName);
    }
}
