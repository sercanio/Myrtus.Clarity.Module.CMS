using MediatR;
using Myrtus.Clarity.Core.Application.Abstractions.Notification;

namespace CMSModule.Events.Handlers;

public class ContentPublishedEventHandler : INotificationHandler<ContentPublishedEvent>
{
    private readonly INotificationService _notificationService;

    public ContentPublishedEventHandler(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public async Task Handle(ContentPublishedEvent notification, CancellationToken cancellationToken)
    {
        var message = $"Content '{notification.Title}' has been published at {notification.PublishedAt}.";
        await _notificationService.SendNotificationAsync(message);
    }
}
