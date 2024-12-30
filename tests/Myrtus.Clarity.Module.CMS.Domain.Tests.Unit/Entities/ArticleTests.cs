using Myrtus.Clarity.Module.CMS.Domain.Entities;
using Myrtus.Clarity.Module.CMS.Domain.ValueObjects;

namespace Myrtus.Clarity.Module.CMS.Domain.Tests.Unit.Entities;

public class ArticleTests
{
    [Fact]
    public void Constructor_ShouldInitializeArticle_WithCorrectValues()
    {
        // Arrange
        var id = Guid.NewGuid();
        var title = "Sample Title";
        var content = "Sample Content";
        var slug = new Slug("sample-title");
        var authorId = Guid.NewGuid();

        // Act
        var article = new Article(id, title, content, slug, authorId);

        // Assert
        Assert.Equal(id, article.Id);
        Assert.Equal(title, article.Title);
        Assert.Equal(content, article.Content);
        Assert.Equal(slug.Value, article.Slug.Value);
        Assert.Equal(authorId, article.AuthorId);
        Assert.True(article.PublishedOn != default);
    }

    [Fact]
    public void AddTag_ShouldAddTagToArticle_WhenTagIsNotAlreadyAdded()
    {
        // Arrange
        var article = new Article(Guid.NewGuid(), "Title", "Content", new Slug("slug"), Guid.NewGuid());
        var tagId = Guid.NewGuid();

        // Act
        article.AddTag(tagId);

        // Assert
        Assert.Contains(tagId, article.TagIds);
    }

    [Fact]
    public void AddTag_ShouldThrowException_WhenTagIsAlreadyAdded()
    {
        // Arrange
        var article = new Article(Guid.NewGuid(), "Title", "Content", new Slug("slug"), Guid.NewGuid());
        var tagId = Guid.NewGuid();
        article.AddTag(tagId);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => article.AddTag(tagId));
    }

    [Fact]
    public void Update_ShouldUpdateArticleValues()
    {
        // Arrange
        var article = new Article(Guid.NewGuid(), "Title", "Content", new Slug("slug"), Guid.NewGuid());
        var newTitle = "Updated Title";
        var newContent = "Updated Content";
        var newSlug = new Slug("updated-title");

        // Act
        article.Update(newTitle, newContent, newSlug);

        // Assert
        Assert.Equal(newTitle, article.Title);
        Assert.Equal(newContent, article.Content);
        Assert.Equal(newSlug.Value, article.Slug.Value);
    }
}
