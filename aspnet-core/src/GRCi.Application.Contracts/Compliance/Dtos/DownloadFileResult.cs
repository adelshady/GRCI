using System;

namespace GRCi.Compliance.Dtos;

public class DownloadFileResult
{
    public byte[] Content { get; set; } = Array.Empty<byte>();
    public string FileName { get; set; } = null!;
    public string ContentType { get; set; } = null!;
}
