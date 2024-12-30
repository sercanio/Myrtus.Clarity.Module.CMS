using MediatR;

namespace CMSModule.Events;

public class ContentUpdatedEvent : INotification
{
    public string ContentId { get; }
    public string Title { get; }
    public string UpdatedBy { get; }

    public ContentUpdatedEvent(string contentId, string title, string updatedBy)
    {
        ContentId = contentId;
        Title = title;
        UpdatedBy = updatedBy;
    }
}
