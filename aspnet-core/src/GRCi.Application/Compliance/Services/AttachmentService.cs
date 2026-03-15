using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GRCi.Compliance.Attachments;
using GRCi.Compliance.Dtos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace GRCi.Compliance.Services;

public class AttachmentService : IAttachmentService
{
    private static readonly string[] AllowedContentTypes =
    {
        "application/pdf",
        "image/png",
        "image/jpg",
        "image/jpeg",
        "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
    };

    private const long MaxFileSizeBytes = 10 * 1024 * 1024; // 10 MB

    private readonly IRepository<StoredFile, Guid> _fileRepository;
    private readonly IRepository<AttachmentLink, Guid> _linkRepository;
    private readonly IGuidGenerator _guidGenerator;
    private readonly IWebHostEnvironment _environment;
    private readonly IConfiguration _configuration;

    public AttachmentService(
        IRepository<StoredFile, Guid> fileRepository,
        IRepository<AttachmentLink, Guid> linkRepository,
        IGuidGenerator guidGenerator,
        IWebHostEnvironment environment,
        IConfiguration configuration)
    {
        _fileRepository = fileRepository;
        _linkRepository = linkRepository;
        _guidGenerator = guidGenerator;
        _environment = environment;
        _configuration = configuration;
    }

    public async Task<AttachmentDto> UploadAsync(
        string entityType,
        Guid entityId,
        string fileName,
        string contentType,
        long size,
        byte[] content,
        Guid? uploadedByUserId)
    {
        ValidateFile(contentType, size);

        var fileId = _guidGenerator.Create();
        var safeFileName = Path.GetFileName(fileName);
        var relativePath = Path.Combine("uploads", entityType, entityId.ToString(), $"{fileId}_{safeFileName}");
        var absolutePath = Path.Combine(GetWebRootPath(), relativePath);

        Directory.CreateDirectory(Path.GetDirectoryName(absolutePath)!);
        await File.WriteAllBytesAsync(absolutePath, content);

        var storedFile = new StoredFile(
            fileId,
            safeFileName,
            contentType,
            size,
            relativePath,
            uploadedByUserId,
            DateTime.UtcNow);

        await _fileRepository.InsertAsync(storedFile);

        var linkId = _guidGenerator.Create();
        var link = new AttachmentLink(linkId, entityType, entityId, fileId, DateTime.UtcNow);
        await _linkRepository.InsertAsync(link);

        return new AttachmentDto
        {
            FileId = fileId,
            LinkId = linkId,
            EntityType = entityType,
            EntityId = entityId,
            FileName = safeFileName,
            ContentType = contentType,
            Size = size,
            UploadedByUserId = uploadedByUserId,
            UploadedAt = storedFile.UploadedAt
        };
    }

    public async Task<DownloadFileResult> DownloadAsync(Guid fileId)
    {
        var storedFile = await _fileRepository.FindAsync(fileId)
            ?? throw new BusinessException($"File with id {fileId} not found.");

        var absolutePath = Path.Combine(GetWebRootPath(), storedFile.RelativePath);

        if (!File.Exists(absolutePath))
            throw new BusinessException($"Physical file for id {fileId} not found on disk.");

        var content = await File.ReadAllBytesAsync(absolutePath);

        return new DownloadFileResult
        {
            Content = content,
            FileName = storedFile.FileName,
            ContentType = storedFile.ContentType
        };
    }

    public async Task DeleteAsync(Guid fileId)
    {
        var storedFile = await _fileRepository.FindAsync(fileId)
            ?? throw new BusinessException($"File with id {fileId} not found.");

        var absolutePath = Path.Combine(GetWebRootPath(), storedFile.RelativePath);
        if (File.Exists(absolutePath))
            File.Delete(absolutePath);

        var links = await _linkRepository.GetListAsync(l => l.FileId == fileId);
        foreach (var link in links)
            await _linkRepository.DeleteAsync(link);

        await _fileRepository.DeleteAsync(storedFile);
    }

    private void ValidateFile(string contentType, long size)
    {
        var maxSize = _configuration.GetValue<long>("Attachments:MaxFileSizeBytes", MaxFileSizeBytes);
        var allowedTypes = _configuration.GetSection("Attachments:AllowedContentTypes").Get<string[]>() ?? AllowedContentTypes;

        if (size > maxSize)
            throw new BusinessException($"File size exceeds the maximum allowed size of {maxSize / 1024 / 1024} MB.");

        if (!allowedTypes.Contains(contentType, StringComparer.OrdinalIgnoreCase))
            throw new BusinessException($"File type '{contentType}' is not allowed. Allowed types: {string.Join(", ", allowedTypes)}");
    }

    private string GetWebRootPath()
    {
        return _environment.WebRootPath
            ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
    }
}
