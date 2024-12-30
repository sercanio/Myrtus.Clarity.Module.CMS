using Myrtus.Clarity.Module.CMS.Domain.Entities;
using Myrtus.Clarity.Module.CMS.Domain.Services;
using Myrtus.Clarity.Module.CMS.Domain.ValueObjects;

namespace Myrtus.Clarity.Module.CMS.Domain.Tests.Unit.Services;

public class ArticleDomainServiceTests
{
    [Fact]
    public void AssignTags_ShouldAddAllTagsToArticle()
    {
        // Arrange
        var article = new Article(Guid.NewGuid(), "Title", "Content", new Slug("slug"), Guid.NewGuid());
        var tags = new List<Tag>
            {
                new Tag(Guid.NewGuid(), "Tech", new Slug("tech")),
                new Tag(Guid.NewGuid(), "Science", new Slug("science"))
            };

        // Act
        ArticleDomainService.AssignTags(article, tags);

        // Assert
        Assert.Contains(tags[0].Id, article.TagIds);
        Assert.Contains(tags[1].Id, article.TagIds);
    }

    [Fact]
    public void AssignTags_ShouldNotAddDuplicateTags()
    {
        // Arrange
        var tagId = Guid.NewGuid();
        var article = new Article(Guid.NewGuid(), "Title", "Content", new Slug("slug"), Guid.NewGuid());
        var tags = new List<Tag>
            {
                new Tag(tagId, "Tech", new Slug("tech"))
            };
        article.AddTag(tagId);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => ArticleDomainService.AssignTags(article, tags));
    }
}
