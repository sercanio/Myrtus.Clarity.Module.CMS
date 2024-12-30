// modules/cms/src/Events/Handlers/MediaDeletedEventHandler.cs
using MediatR;
using Myrtus.Clarity.Core.Application.Abstractions.Auditing;
using Myrtus.Clarity.Core.Domain.Abstractions;

namespace CMSModule.Events.Handlers;

public class MediaDeletedEventHandler : INotificationHandler<MediaDeletedEvent>
{
    private readonly IAuditLogService _auditLogService;

    public MediaDeletedEventHandler(IAuditLogService auditLogService)
    {
        _auditLogService = auditLogService;
    }

    public async Task Handle(MediaDeletedEvent notification, CancellationToken cancellationToken)
    {
        var log = new AuditLog
        {
            Action = "Media Deleted",
            EntityId = notification.MediaId,
            Entity = "Media",
            Timestamp = DateTime.UtcNow,
            CreatedBy = notification.DeletedBy,
            Details = $"Media '{notification.FileName}' was deleted by {notification.DeletedBy}."
        };
        await _auditLogService.LogAsync(log);
    }
}
