using System;
using System.Threading.Tasks;
using GRCi.Compliance.Commands;
using GRCi.Compliance.Dtos;
using GRCi.Compliance.Queries;
using GRCi.Permissions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace GRCi.Controllers.Compliance;

[ApiController]
[Route("api/compliance/templates")]
[Authorize]
public class ChecklistTemplatesController : AbpControllerBase
{
    private readonly IMediator _mediator;

    public ChecklistTemplatesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize(GRCiPermissions.Compliance.Templates.Default)]
    public Task<PagedResultDto<ChecklistTemplateDto>> GetListAsync([FromQuery] ListChecklistTemplatesQuery query)
    {
        return _mediator.Send(query);
    }

    [HttpGet("{id:guid}")]
    [Authorize(GRCiPermissions.Compliance.Templates.Default)]
    public Task<ChecklistTemplateDto> GetAsync(Guid id)
    {
        return _mediator.Send(new GetChecklistTemplateByIdQuery(id));
    }

    [HttpPost]
    [Authorize(GRCiPermissions.Compliance.Templates.Create)]
    public Task<ChecklistTemplateDto> CreateAsync([FromBody] CreateChecklistTemplateCommand command)
    {
        return _mediator.Send(command);
    }

    [HttpPut("{id:guid}")]
    [Authorize(GRCiPermissions.Compliance.Templates.Edit)]
    public Task<ChecklistTemplateDto> UpdateAsync(Guid id, [FromBody] UpdateChecklistTemplateCommand command)
    {
        command.Id = id;
        return _mediator.Send(command);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(GRCiPermissions.Compliance.Templates.Delete)]
    public Task DeleteAsync(Guid id)
    {
        return _mediator.Send(new DeleteChecklistTemplateCommand(id));
    }

    [HttpGet("{templateId:guid}/items")]
    [Authorize(GRCiPermissions.Compliance.Templates.Default)]
    public Task<System.Collections.Generic.List<ChecklistItemDto>> GetItemsAsync(Guid templateId)
    {
        return _mediator.Send(new GetChecklistItemsByTemplateIdQuery(templateId));
    }
}
