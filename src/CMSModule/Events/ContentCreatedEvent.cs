// modules/cms/src/Events/ContentCreatedEvent.cs
using MediatR;

namespace CMSModule.Events;

public class ContentCreatedEvent : INotification
{
    public string ContentId { get; }
    public string Title { get; }
    public string CreatedBy { get; }

    public ContentCreatedEvent(string contentId, string title, string createdBy)
    {
        ContentId = contentId;
        Title = title;
        CreatedBy = createdBy;
    }
}