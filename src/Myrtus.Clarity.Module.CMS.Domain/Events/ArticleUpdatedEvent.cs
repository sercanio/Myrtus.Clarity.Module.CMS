using Myrtus.Clarity.Core.Domain.Abstractions;

namespace Myrtus.Clarity.Module.CMS.Domain.Events;

public record ArticleUpdatedEvent(Guid ArticleId) : IDomainEvent;
