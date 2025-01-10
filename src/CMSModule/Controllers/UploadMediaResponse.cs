namespace CMSModule.Controllers.DTOs;

public sealed record UploadMediaResponse
{
    public string Id { get; set; }
    public string FileName { get; set; }
    public string BlobUri { get; set; }
    public string ContentType { get; set; }
    public long Size { get; set; }
    public DateTime UploadedAt { get; set; }
    public string UploadedBy { get; set; }
}

