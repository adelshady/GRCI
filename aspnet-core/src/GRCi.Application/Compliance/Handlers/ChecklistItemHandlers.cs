using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GRCi.Compliance.ChecklistTemplates;
using GRCi.Compliance.Commands;
using GRCi.Compliance.Dtos;
using GRCi.Compliance.Queries;
using GRCi.Compliance.Repositories;
using MediatR;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace GRCi.Compliance.Handlers;

public class CreateChecklistItemHandler : IRequestHandler<CreateChecklistItemCommand, ChecklistItemDto>
{
    private readonly IChecklistTemplateRepository _templateRepository;
    private readonly IGuidGenerator _guidGenerator;
    private readonly IMapper _mapper;

    public CreateChecklistItemHandler(
        IChecklistTemplateRepository templateRepository,
        IGuidGenerator guidGenerator,
        IMapper mapper)
    {
        _templateRepository = templateRepository;
        _guidGenerator = guidGenerator;
        _mapper = mapper;
    }

    public async Task<ChecklistItemDto> Handle(CreateChecklistItemCommand request, CancellationToken cancellationToken)
    {
        var template = await _templateRepository.GetWithItemsAsync(request.TemplateId, cancellationToken)
            ?? throw new BusinessException($"ChecklistTemplate with id {request.TemplateId} not found.");

        var item = template.AddItem(
            _guidGenerator.Create(),
            request.SectionTitle,
            request.RequirementTextEn,
            request.RequirementTextAr,
            request.Criticality,
            request.IsMandatory,
            request.NotesRequiredWhen,
            request.SortOrder);

        await _templateRepository.UpdateAsync(template, autoSave: true, cancellationToken: cancellationToken);

        return _mapper.Map<ChecklistItem, ChecklistItemDto>(item);
    }
}

public class UpdateChecklistItemHandler : IRequestHandler<UpdateChecklistItemCommand, ChecklistItemDto>
{
    private readonly IChecklistTemplateRepository _templateRepository;
    private readonly IRepository<ChecklistItem, Guid> _itemRepository;
    private readonly IMapper _mapper;

    public UpdateChecklistItemHandler(
        IChecklistTemplateRepository templateRepository,
        IRepository<ChecklistItem, Guid> itemRepository,
        IMapper mapper)
    {
        _templateRepository = templateRepository;
        _itemRepository = itemRepository;
        _mapper = mapper;
    }

    public async Task<ChecklistItemDto> Handle(UpdateChecklistItemCommand request, CancellationToken cancellationToken)
    {
        var item = await _itemRepository.GetAsync(request.ItemId, cancellationToken: cancellationToken);

        var template = await _templateRepository.GetWithItemsAsync(item.TemplateId, cancellationToken)
            ?? throw new BusinessException($"ChecklistTemplate with id {item.TemplateId} not found.");

        template.UpdateItem(
            request.ItemId,
            request.SectionTitle,
            request.RequirementTextEn,
            request.RequirementTextAr,
            request.Criticality,
            request.IsMandatory,
            request.NotesRequiredWhen,
            request.SortOrder);

        await _templateRepository.UpdateAsync(template, autoSave: true, cancellationToken: cancellationToken);

        var updatedItem = template.Items.First(i => i.Id == request.ItemId);
        return _mapper.Map<ChecklistItem, ChecklistItemDto>(updatedItem);
    }
}

public class DeleteChecklistItemHandler : IRequestHandler<DeleteChecklistItemCommand>
{
    private readonly IChecklistTemplateRepository _templateRepository;
    private readonly IRepository<ChecklistItem, Guid> _itemRepository;

    public DeleteChecklistItemHandler(
        IChecklistTemplateRepository templateRepository,
        IRepository<ChecklistItem, Guid> itemRepository)
    {
        _templateRepository = templateRepository;
        _itemRepository = itemRepository;
    }

    public async Task Handle(DeleteChecklistItemCommand request, CancellationToken cancellationToken)
    {
        var item = await _itemRepository.GetAsync(request.ItemId, cancellationToken: cancellationToken);

        var template = await _templateRepository.GetWithItemsAsync(item.TemplateId, cancellationToken)
            ?? throw new BusinessException($"ChecklistTemplate with id {item.TemplateId} not found.");

        template.RemoveItem(request.ItemId);

        await _templateRepository.UpdateAsync(template, autoSave: true, cancellationToken: cancellationToken);
    }
}

public class GetChecklistItemsByTemplateIdHandler : IRequestHandler<GetChecklistItemsByTemplateIdQuery, List<ChecklistItemDto>>
{
    private readonly IChecklistTemplateRepository _templateRepository;
    private readonly IMapper _mapper;

    public GetChecklistItemsByTemplateIdHandler(IChecklistTemplateRepository templateRepository, IMapper mapper)
    {
        _templateRepository = templateRepository;
        _mapper = mapper;
    }

    public async Task<List<ChecklistItemDto>> Handle(GetChecklistItemsByTemplateIdQuery request, CancellationToken cancellationToken)
    {
        var template = await _templateRepository.GetWithItemsAsync(request.TemplateId, cancellationToken)
            ?? throw new BusinessException($"ChecklistTemplate with id {request.TemplateId} not found.");

        var items = request.ActiveOnly
            ? template.Items.Where(i => i.IsActive).ToList()
            : template.Items.ToList();

        return _mapper.Map<List<ChecklistItem>, List<ChecklistItemDto>>(items);
    }
}
