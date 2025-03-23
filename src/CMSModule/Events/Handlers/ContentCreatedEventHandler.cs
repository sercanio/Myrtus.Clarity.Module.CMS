using MediatR;
using Myrtus.Clarity.Core.Application.Abstractions.Auditing;
using Myrtus.Clarity.Core.Domain.Abstractions;

namespace CMSModule.Events.Handlers;

public class ContentCreatedAuditHandler : INotificationHandler<ContentCreatedEvent>
{
    private readonly IAuditLogService _auditLogService;

    public ContentCreatedAuditHandler(IAuditLogService auditLogService)
    {
        _auditLogService = auditLogService;
    }

    public async Task Handle(ContentCreatedEvent notification, CancellationToken cancellationToken)
    {
        var log = new AuditLog
        {
            Action = "Content Created",
            EntityId = notification.ContentId,
            Entity = "Content",
            Timestamp = DateTime.UtcNow,
            CreatedBy = notification.CreatedBy,
            Details = $"Content '{notification.Title}' was created."
        };
        await _auditLogService.LogAsync(log);
    }
}
