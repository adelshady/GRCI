using System.Threading;
using System.Threading.Tasks;
using GRCi.Compliance.Commands;
using GRCi.Compliance.Repositories;
using MediatR;

namespace GRCi.Compliance.Handlers;

public class DeleteChecklistTemplateHandler : IRequestHandler<DeleteChecklistTemplateCommand>
{
    private readonly IChecklistTemplateRepository _repository;

    public DeleteChecklistTemplateHandler(IChecklistTemplateRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteChecklistTemplateCommand request, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(request.Id, autoSave: true, cancellationToken: cancellationToken);
    }
}
