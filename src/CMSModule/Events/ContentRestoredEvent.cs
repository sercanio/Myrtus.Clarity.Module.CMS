using MediatR;

namespace CMSModule.Events
{
    public class ContentRestoredEvent : INotification
    {
        public string ContentId { get; }
        public string Title { get; }
        public string RestoredBy { get; }

        public ContentRestoredEvent(string contentId, string title, string restoredBy)
        {
            ContentId = contentId;
            Title = title;
            RestoredBy = restoredBy;
        }
    }
}
