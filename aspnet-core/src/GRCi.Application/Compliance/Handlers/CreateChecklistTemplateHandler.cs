using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GRCi.Compliance.ChecklistTemplates;
using GRCi.Compliance.Commands;
using GRCi.Compliance.Dtos;
using GRCi.Compliance.Repositories;
using MediatR;
using Volo.Abp.Guids;

namespace GRCi.Compliance.Handlers;

public class CreateChecklistTemplateHandler : IRequestHandler<CreateChecklistTemplateCommand, ChecklistTemplateDto>
{
    private readonly IChecklistTemplateRepository _repository;
    private readonly IGuidGenerator _guidGenerator;
    private readonly IMapper _mapper;

    public CreateChecklistTemplateHandler(
        IChecklistTemplateRepository repository,
        IGuidGenerator guidGenerator,
        IMapper mapper)
    {
        _repository = repository;
        _guidGenerator = guidGenerator;
        _mapper = mapper;
    }

    public async Task<ChecklistTemplateDto> Handle(CreateChecklistTemplateCommand request, CancellationToken cancellationToken)
    {
        var maxNumber = await _repository.GetMaxCodeNumberAsync(cancellationToken);
        var code = $"CLT-{(maxNumber + 1):D4}";

        var template = new ChecklistTemplate(
            _guidGenerator.Create(),
            code,
            request.NameEn,
            request.NameAr,
            request.DescriptionEn,
            request.DescriptionAr,
            request.RegulatoryAgencyId);

        await _repository.InsertAsync(template, autoSave: true, cancellationToken: cancellationToken);

        return _mapper.Map<ChecklistTemplate, ChecklistTemplateDto>(template);
    }
}
