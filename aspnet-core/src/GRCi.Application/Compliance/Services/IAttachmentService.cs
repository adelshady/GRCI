using System;
using System.Threading.Tasks;
using GRCi.Compliance.Dtos;

namespace GRCi.Compliance.Services;

public interface IAttachmentService
{
    Task<AttachmentDto> UploadAsync(
        string entityType,
        Guid entityId,
        string fileName,
        string contentType,
        long size,
        byte[] content,
        Guid? uploadedByUserId);

    Task<DownloadFileResult> DownloadAsync(Guid fileId);

    Task DeleteAsync(Guid fileId);
}
