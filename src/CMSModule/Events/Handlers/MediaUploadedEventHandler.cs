using MediatR;
using Myrtus.Clarity.Core.Application.Abstractions.Notification;

namespace CMSModule.Events.Handlers
{
    public class MediaUploadedEventHandler : INotificationHandler<MediaUploadedEvent>
    {
        private readonly INotificationService _notificationService;

        public MediaUploadedEventHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task Handle(MediaUploadedEvent notification, CancellationToken cancellationToken)
        {
            var message = $"Media '{notification.FileName}' has been uploaded by {notification.UploadedBy}.";
            await _notificationService.SendNotificationAsync(message);
        }
    }
}
