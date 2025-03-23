using MediatR;

namespace CMSModule.Events;

public class MediaUploadedEvent : INotification
{
    public string MediaId { get; }
    public string FileName { get; }
    public string UploadedBy { get; }

    public MediaUploadedEvent(string mediaId, string fileName, string uploadedBy)
    {
        MediaId = mediaId;
        FileName = fileName;
        UploadedBy = uploadedBy;
    }
}
