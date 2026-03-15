using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GRCi.Compliance.Attachments;
using GRCi.Compliance.Commands;
using GRCi.Compliance.Dtos;
using GRCi.Compliance.Queries;
using GRCi.Compliance.Services;
using MediatR;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;

namespace GRCi.Compliance.Handlers;

public class UploadAttachmentHandler : IRequestHandler<UploadAttachmentCommand, AttachmentDto>
{
    private readonly IAttachmentService _attachmentService;
    private readonly ICurrentUser _currentUser;

    public UploadAttachmentHandler(IAttachmentService attachmentService, ICurrentUser currentUser)
    {
        _attachmentService = attachmentService;
        _currentUser = currentUser;
    }

    public Task<AttachmentDto> Handle(UploadAttachmentCommand request, CancellationToken cancellationToken)
    {
        return _attachmentService.UploadAsync(
            request.EntityType,
            request.EntityId,
            request.FileName,
            request.ContentType,
            request.Size,
            request.Content,
            _currentUser.Id);
    }
}

public class DeleteAttachmentHandler : IRequestHandler<DeleteAttachmentCommand>
{
    private readonly IAttachmentService _attachmentService;

    public DeleteAttachmentHandler(IAttachmentService attachmentService)
    {
        _attachmentService = attachmentService;
    }

    public Task Handle(DeleteAttachmentCommand request, CancellationToken cancellationToken)
    {
        return _attachmentService.DeleteAsync(request.FileId);
    }
}

public class DownloadAttachmentHandler : IRequestHandler<DownloadAttachmentQuery, DownloadFileResult>
{
    private readonly IAttachmentService _attachmentService;

    public DownloadAttachmentHandler(IAttachmentService attachmentService)
    {
        _attachmentService = attachmentService;
    }

    public Task<DownloadFileResult> Handle(DownloadAttachmentQuery request, CancellationToken cancellationToken)
    {
        return _attachmentService.DownloadAsync(request.FileId);
    }
}

public class GetItemAttachmentsHandler : IRequestHandler<GetItemAttachmentsQuery, List<AttachmentDto>>
{
    private readonly IRepository<AttachmentLink, Guid> _linkRepository;
    private readonly IRepository<StoredFile, Guid> _fileRepository;

    public GetItemAttachmentsHandler(
        IRepository<AttachmentLink, Guid> linkRepository,
        IRepository<StoredFile, Guid> fileRepository)
    {
        _linkRepository = linkRepository;
        _fileRepository = fileRepository;
    }

    public async Task<List<AttachmentDto>> Handle(GetItemAttachmentsQuery request, CancellationToken cancellationToken)
    {
        var links = await _linkRepository.GetListAsync(
            l => l.EntityType == request.EntityType && l.EntityId == request.EntityId,
            cancellationToken: cancellationToken);

        var fileIds = links.Select(l => l.FileId).ToList();
        var files = await _fileRepository.GetListAsync(f => fileIds.Contains(f.Id), cancellationToken: cancellationToken);

        return links.Select(link =>
        {
            var file = files.FirstOrDefault(f => f.Id == link.FileId);
            return new AttachmentDto
            {
                FileId = link.FileId,
                LinkId = link.Id,
                EntityType = link.EntityType,
                EntityId = link.EntityId,
                FileName = file?.FileName ?? string.Empty,
                ContentType = file?.ContentType ?? string.Empty,
                Size = file?.Size ?? 0,
                UploadedByUserId = file?.UploadedByUserId,
                UploadedAt = file?.UploadedAt ?? link.CreatedAt
            };
        }).ToList();
    }
}
