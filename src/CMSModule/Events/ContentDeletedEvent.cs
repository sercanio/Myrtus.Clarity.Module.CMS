using MediatR;

namespace CMSModule.Events
{
    public class ContentDeletedEvent : INotification
    {
        public string ContentId { get; }
        public string Title { get; }
        public string DeletedBy { get; }

        public ContentDeletedEvent(string contentId, string title, string deletedBy)
        {
            ContentId = contentId;
            Title = title;
            DeletedBy = deletedBy;
        }
    }
}
