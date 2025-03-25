// modules/cms/src/Events/Handlers/ContentDeletedEventHandler.cs
using System.Threading;
using System.Threading.Tasks;
using CMSModule.Events;
using MediatR;
using Myrtus.Clarity.Core.Application.Abstractions.Auditing;
using Myrtus.Clarity.Core.Domain.Abstractions;

namespace CMSModule.Events.Handlers;

public class ContentDeletedEventHandler : INotificationHandler<ContentDeletedEvent>
{
    private readonly IAuditLogService _auditLogService;

    public ContentDeletedEventHandler(IAuditLogService auditLogService)
    {
        _auditLogService = auditLogService;
    }

    public async Task Handle(ContentDeletedEvent notification, CancellationToken cancellationToken)
    {
        var log = new AuditLog
        {
            Action = "Content Deleted",
            EntityId = notification.ContentId,
            Entity = "Content",
            Timestamp = DateTime.UtcNow,
            CreatedBy = notification.DeletedBy,
            Details = $"Content '{notification.Title}' was deleted by {notification.DeletedBy}."
        };
        await _auditLogService.LogAsync(log);
    }
}
