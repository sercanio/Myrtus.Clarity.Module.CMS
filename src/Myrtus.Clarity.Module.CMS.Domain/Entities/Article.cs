using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.Clarity.Module.CMS.Domain.Events;
using Myrtus.Clarity.Module.CMS.Domain.ValueObjects;

namespace Myrtus.Clarity.Module.CMS.Domain.Entities;

public class Article : Entity, IAggregateRoot
{
    public string Title { get; private set; }
    public string Content { get; private set; }
    public Slug Slug { get; private set; }
    public Guid AuthorId { get; private set; }
    public List<Guid> CategoryIds { get; private set; } = new();
    public List<Guid> TagIds { get; private set; } = new();
    public DateTime PublishedOn { get; private set; }

    private Article() { }

    public Article(Guid id, string title, string content, Slug slug, Guid authorId) : base(id)
    {
        Title = title;
        Content = content;
        Slug = slug;
        AuthorId = authorId;
        PublishedOn = DateTime.UtcNow;

        RaiseDomainEvent(new ArticleCreatedEvent(id));
    }

    public void Update(string title, string content, Slug slug)
    {
        Title = title;
        Content = content;
        Slug = slug;

        MarkUpdated();
        RaiseDomainEvent(new ArticleUpdatedEvent(Id));
    }

    public void Delete()
    {
        MarkDeleted();
        RaiseDomainEvent(new ArticleDeletedEvent(Id));
    }

    public void AddTag(Guid tagId)
    {
        if (TagIds.Contains(tagId))
        {
            throw new InvalidOperationException($"The tag with ID '{tagId}' is already assigned to this article.");
        }

        TagIds.Add(tagId);
    }

    public void AddTags(IEnumerable<Guid> tagIds)
    {
        foreach (var tagId in tagIds)
        {
            AddTag(tagId);
        }
    }
}