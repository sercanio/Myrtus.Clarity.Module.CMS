using CMSModule.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using AppTemplate.Infrastructure.Repositories.NoSQL;

namespace CMSModule.Repositories.ContentRepository;

public class ContentRepository : NoSqlRepository<Content>, IContentRepository
{
    public ContentRepository(IMongoDatabase database)
        : base(database, "Contents")
    {
    }

    public async Task<bool> CheckIfContentExistsBySlugAsync(string slug, CancellationToken cancellationToken)
    {
        return await _collection.Find(c => c.Slug == slug).AnyAsync(cancellationToken);
    }
}
