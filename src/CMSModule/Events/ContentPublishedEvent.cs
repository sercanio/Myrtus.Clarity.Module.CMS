using MediatR;
using Myrtus.Clarity.Core.Application.Abstractions.Clock;
using Myrtus.Clarity.Core.Infrastructure.Clock;

public class ContentPublishedEvent : INotification
{
    private readonly IDateTimeProvider _dateTimeProvider;
    public string ContentId { get; }
    public string Title { get; }
    public DateTime PublishedAt { get; }
    public string PublishedBy { get; }

    public ContentPublishedEvent(IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
    }

    public ContentPublishedEvent(string contentId, string title, string publishedBy)
    {
        ContentId = contentId;
        Title = title;
        PublishedAt = _dateTimeProvider!.UtcNow;
        PublishedBy = publishedBy;
    }
}
