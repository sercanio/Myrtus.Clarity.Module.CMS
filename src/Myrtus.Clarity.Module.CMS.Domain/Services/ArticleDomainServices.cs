using Myrtus.Clarity.Module.CMS.Domain.Entities;

namespace Myrtus.Clarity.Module.CMS.Domain.Services;

public class ArticleDomainService
{
    public static void AssignTags(Article article, IEnumerable<Tag> tags)
    {
        IEnumerable<Guid> tagIds = tags.Select(tag => tag.Id);
        article.AddTags(tagIds);
    }
}
