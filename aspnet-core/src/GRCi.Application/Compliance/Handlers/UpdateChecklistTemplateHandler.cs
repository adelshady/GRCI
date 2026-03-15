using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GRCi.Compliance.ChecklistTemplates;
using GRCi.Compliance.Commands;
using GRCi.Compliance.Dtos;
using GRCi.Compliance.Repositories;
using MediatR;
using Volo.Abp;

namespace GRCi.Compliance.Handlers;

public class UpdateChecklistTemplateHandler : IRequestHandler<UpdateChecklistTemplateCommand, ChecklistTemplateDto>
{
    private readonly IChecklistTemplateRepository _repository;
    private readonly IMapper _mapper;

    public UpdateChecklistTemplateHandler(IChecklistTemplateRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ChecklistTemplateDto> Handle(UpdateChecklistTemplateCommand request, CancellationToken cancellationToken)
    {
        var template = await _repository.GetAsync(request.Id, cancellationToken: cancellationToken);

        template.Update(
            request.NameEn,
            request.NameAr,
            request.DescriptionEn,
            request.DescriptionAr,
            request.RegulatoryAgencyId);

        await _repository.UpdateAsync(template, autoSave: true, cancellationToken: cancellationToken);

        return _mapper.Map<ChecklistTemplate, ChecklistTemplateDto>(template);
    }
}
