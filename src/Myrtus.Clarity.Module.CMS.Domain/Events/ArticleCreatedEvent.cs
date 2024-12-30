using Myrtus.Clarity.Core.Domain.Abstractions;

namespace Myrtus.Clarity.Module.CMS.Domain.Events;

public record ArticleCreatedEvent(Guid ArticleId) : IDomainEvent;
