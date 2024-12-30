using MediatR;
using Myrtus.Clarity.Core.Application.Abstractions.Notification;

namespace CMSModule.Events.Handlers;

public class ContentUpdatedEventHandler : INotificationHandler<ContentUpdatedEvent>
{
    private readonly INotificationService _notificationService;

    public ContentUpdatedEventHandler(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public async Task Handle(ContentUpdatedEvent notification, CancellationToken cancellationToken)
    {
        var message = $"Content '{notification.Title}' has been updated by {notification.UpdatedBy}.";
        await _notificationService.SendNotificationAsync(message);
    }
}
