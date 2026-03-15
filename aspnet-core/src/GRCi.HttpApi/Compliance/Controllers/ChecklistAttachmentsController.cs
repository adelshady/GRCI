using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using GRCi.Compliance.Commands;
using GRCi.Compliance.Dtos;
using GRCi.Compliance.Queries;
using GRCi.Permissions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace GRCi.Controllers.Compliance;

[ApiController]
[Route("api/compliance")]
[Authorize]
public class ChecklistAttachmentsController : AbpControllerBase
{
    private readonly IMediator _mediator;

    public ChecklistAttachmentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("items/{itemId:guid}/attachments")]
    [Authorize(GRCiPermissions.Compliance.Attachments.Manage)]
    [Consumes("multipart/form-data")]
    public async Task<AttachmentDto> UploadAsync(Guid itemId, IFormFile file)
    {
        if (file == null || file.Length == 0)
            throw new BadHttpRequestException("No file was uploaded.");

        using var ms = new MemoryStream();
        await file.CopyToAsync(ms);

        var command = new UploadAttachmentCommand
        {
            EntityType = "ChecklistItem",
            EntityId = itemId,
            FileName = file.FileName,
            ContentType = file.ContentType,
            Size = file.Length,
            Content = ms.ToArray()
        };

        return await _mediator.Send(command);
    }

    [HttpGet("items/{itemId:guid}/attachments")]
    [Authorize(GRCiPermissions.Compliance.Attachments.Manage)]
    public Task<List<AttachmentDto>> GetAttachmentsAsync(Guid itemId)
    {
        return _mediator.Send(new GetItemAttachmentsQuery("ChecklistItem", itemId));
    }

    [HttpGet("attachments/{fileId:guid}/download")]
    [Authorize(GRCiPermissions.Compliance.Attachments.Manage)]
    public async Task<IActionResult> DownloadAsync(Guid fileId)
    {
        var result = await _mediator.Send(new DownloadAttachmentQuery(fileId));
        return File(result.Content, result.ContentType, result.FileName);
    }

    [HttpDelete("attachments/{fileId:guid}")]
    [Authorize(GRCiPermissions.Compliance.Attachments.Manage)]
    public Task DeleteAsync(Guid fileId)
    {
        return _mediator.Send(new DeleteAttachmentCommand(fileId));
    }
}
