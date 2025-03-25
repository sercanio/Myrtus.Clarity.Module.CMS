using MediatR;
using Myrtus.Clarity.Core.Application.Abstractions.Notification;

namespace CMSModule.Events.Handlers;

public class ContentRestoredEventHandler : INotificationHandler<ContentRestoredEvent>
{
    private readonly INotificationService _notificationService;

    public ContentRestoredEventHandler(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public async Task Handle(ContentRestoredEvent notification, CancellationToken cancellationToken)
    {
        var message = $"Content '{notification.Title}' has been restored by {notification.RestoredBy}.";
        await _notificationService.SendNotificationAsync(message);
    }
}
