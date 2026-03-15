using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GRCi.Compliance.Commands;
using GRCi.Compliance.Dtos;
using GRCi.Compliance.Queries;
using GRCi.Permissions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace GRCi.Controllers.Compliance;

[ApiController]
[Route("api/compliance/templates/{templateId:guid}")]
[Authorize]
public class ChecklistWorkflowController : AbpControllerBase
{
    private readonly IMediator _mediator;

    public ChecklistWorkflowController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("submit")]
    [Authorize(GRCiPermissions.Compliance.Templates.Submit)]
    public Task SubmitAsync(Guid templateId)
    {
        return _mediator.Send(new SubmitChecklistTemplateCommand(templateId));
    }

    [HttpPost("approve")]
    [Authorize(GRCiPermissions.Compliance.Templates.Approve)]
    public Task ApproveAsync(Guid templateId)
    {
        return _mediator.Send(new ApproveChecklistTemplateCommand(templateId));
    }

    [HttpPost("return")]
    [Authorize(GRCiPermissions.Compliance.Templates.Return)]
    public Task ReturnAsync(Guid templateId, [FromBody] ReturnChecklistTemplateCommand command)
    {
        command.TemplateId = templateId;
        return _mediator.Send(command);
    }

    [HttpPost("resubmit")]
    [Authorize(GRCiPermissions.Compliance.Templates.Submit)]
    public Task ResubmitAsync(Guid templateId)
    {
        return _mediator.Send(new ResubmitChecklistTemplateCommand(templateId));
    }

    [HttpPost("archive")]
    [Authorize(GRCiPermissions.Compliance.Templates.Archive)]
    public Task ArchiveAsync(Guid templateId)
    {
        return _mediator.Send(new ArchiveChecklistTemplateCommand(templateId));
    }

    [HttpGet("history")]
    [Authorize(GRCiPermissions.Compliance.Templates.Default)]
    public Task<List<WorkflowActionHistoryDto>> GetHistoryAsync(Guid templateId)
    {
        return _mediator.Send(new GetWorkflowHistoryQuery("ChecklistTemplate", templateId));
    }
}
