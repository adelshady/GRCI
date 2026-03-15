using System;
using System.Threading.Tasks;
using GRCi.Compliance.Commands;
using GRCi.Compliance.Dtos;
using GRCi.Permissions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace GRCi.Controllers.Compliance;

[ApiController]
[Route("api/compliance")]
[Authorize]
public class ChecklistItemsController : AbpControllerBase
{
    private readonly IMediator _mediator;

    public ChecklistItemsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("templates/{templateId:guid}/items")]
    [Authorize(GRCiPermissions.Compliance.Items.Manage)]
    public Task<ChecklistItemDto> CreateItemAsync(Guid templateId, [FromBody] CreateChecklistItemCommand command)
    {
        command.TemplateId = templateId;
        return _mediator.Send(command);
    }

    [HttpPut("items/{id:guid}")]
    [Authorize(GRCiPermissions.Compliance.Items.Manage)]
    public Task<ChecklistItemDto> UpdateItemAsync(Guid id, [FromBody] UpdateChecklistItemCommand command)
    {
        command.ItemId = id;
        return _mediator.Send(command);
    }

    [HttpDelete("items/{id:guid}")]
    [Authorize(GRCiPermissions.Compliance.Items.Manage)]
    public Task DeleteItemAsync(Guid id)
    {
        return _mediator.Send(new DeleteChecklistItemCommand(id));
    }
}
