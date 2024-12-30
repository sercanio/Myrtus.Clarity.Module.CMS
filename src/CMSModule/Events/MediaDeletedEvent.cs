using MediatR;

namespace CMSModule.Events;

public class MediaDeletedEvent : INotification
{
    public string MediaId { get; }
    public string FileName { get; }
    public string DeletedBy { get; }

    public MediaDeletedEvent(string mediaId, string fileName, string deletedBy)
    {
        MediaId = mediaId;
        FileName = fileName;
        DeletedBy = deletedBy;
    }
}
