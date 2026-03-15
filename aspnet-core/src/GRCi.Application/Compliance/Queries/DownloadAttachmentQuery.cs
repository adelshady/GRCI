using System;
using GRCi.Compliance.Dtos;
using MediatR;

namespace GRCi.Compliance.Queries;

public class DownloadAttachmentQuery : IRequest<DownloadFileResult>
{
    public Guid FileId { get; set; }

    public DownloadAttachmentQuery(Guid fileId) => FileId = fileId;
}
