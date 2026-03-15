using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GRCi.Compliance.ChecklistTemplates;
using GRCi.Compliance.Dtos;
using GRCi.Compliance.Queries;
using GRCi.Compliance.Repositories;
using GRCi.Compliance.Workflow;
using MediatR;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace GRCi.Compliance.Handlers;

public class GetChecklistTemplateByIdHandler : IRequestHandler<GetChecklistTemplateByIdQuery, ChecklistTemplateDto>
{
    private readonly IChecklistTemplateRepository _templateRepository;
    private readonly IRepository<WorkflowActionHistory, Guid> _historyRepository;
    private readonly IMapper _mapper;

    public GetChecklistTemplateByIdHandler(
        IChecklistTemplateRepository templateRepository,
        IRepository<WorkflowActionHistory, Guid> historyRepository,
        IMapper mapper)
    {
        _templateRepository = templateRepository;
        _historyRepository = historyRepository;
        _mapper = mapper;
    }

    public async Task<ChecklistTemplateDto> Handle(GetChecklistTemplateByIdQuery request, CancellationToken cancellationToken)
    {
        var template = await _templateRepository.GetWithItemsAsync(request.Id, cancellationToken)
            ?? throw new BusinessException($"ChecklistTemplate with id {request.Id} not found.");

        var dto = _mapper.Map<ChecklistTemplate, ChecklistTemplateDto>(template);

        if (request.IncludeHistory)
        {
            var history = await _historyRepository.GetListAsync(
                h => h.EntityType == "ChecklistTemplate" && h.EntityId == request.Id,
                cancellationToken: cancellationToken);

            dto.WorkflowHistory = _mapper.Map<List<WorkflowActionHistory>, List<WorkflowActionHistoryDto>>(
                history.OrderBy(h => h.PerformedAt).ToList());
        }

        return dto;
    }
}

public class ListChecklistTemplatesHandler : IRequestHandler<ListChecklistTemplatesQuery, PagedResultDto<ChecklistTemplateDto>>
{
    private readonly IChecklistTemplateRepository _templateRepository;
    private readonly IMapper _mapper;

    public ListChecklistTemplatesHandler(IChecklistTemplateRepository templateRepository, IMapper mapper)
    {
        _templateRepository = templateRepository;
        _mapper = mapper;
    }

    public async Task<PagedResultDto<ChecklistTemplateDto>> Handle(ListChecklistTemplatesQuery request, CancellationToken cancellationToken)
    {
        var totalCount = await _templateRepository.GetCountAsync(
            request.Search, request.Status, request.RegulatoryAgencyId, cancellationToken);

        var items = await _templateRepository.GetListAsync(
            request.SkipCount, request.MaxResultCount,
            request.Search, request.Status, request.RegulatoryAgencyId, cancellationToken);

        return new PagedResultDto<ChecklistTemplateDto>(
            totalCount,
            _mapper.Map<List<ChecklistTemplate>, List<ChecklistTemplateDto>>(items));
    }
}

public class GetWorkflowHistoryHandler : IRequestHandler<GetWorkflowHistoryQuery, List<WorkflowActionHistoryDto>>
{
    private readonly IRepository<WorkflowActionHistory, Guid> _historyRepository;
    private readonly IMapper _mapper;

    public GetWorkflowHistoryHandler(
        IRepository<WorkflowActionHistory, Guid> historyRepository,
        IMapper mapper)
    {
        _historyRepository = historyRepository;
        _mapper = mapper;
    }

    public async Task<List<WorkflowActionHistoryDto>> Handle(GetWorkflowHistoryQuery request, CancellationToken cancellationToken)
    {
        var history = await _historyRepository.GetListAsync(
            h => h.EntityType == request.EntityType && h.EntityId == request.EntityId,
            cancellationToken: cancellationToken);

        return _mapper.Map<List<WorkflowActionHistory>, List<WorkflowActionHistoryDto>>(
            history.OrderBy(h => h.PerformedAt).ToList());
    }
}
