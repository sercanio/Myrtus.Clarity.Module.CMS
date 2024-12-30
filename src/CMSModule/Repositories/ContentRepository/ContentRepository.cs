using CMSModule.Models;
using MongoDB.Driver;

namespace CMSModule.Repositories.ContentRepository;

public class ContentRepository : IContentRepository
{
    private readonly IMongoCollection<Models.Content> _contents;

    public ContentRepository(IMongoDatabase database)
    {
        _contents = database.GetCollection<Models.Content>("Contents");
    }

    public async Task<Models.Content> GetByIdAsync(string id)
    {
        return await _contents.Find(c => c.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Models.Content> GetBySlugAsync(string slug)
    {
        return await _contents.Find(c => c.Slug == slug).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Models.Content>> GetAllAsync()
    {
        return await _contents.Find(_ => true).ToListAsync();
    }

    public async Task<IEnumerable<Models.Content>> QueryAsync(ContentQueryParameters parameters)
    {
        var filterBuilder = Builders<Models.Content>.Filter;
        var filter = filterBuilder.Empty;

        if (!string.IsNullOrEmpty(parameters.ContentType))
        {
            filter &= filterBuilder.Eq(c => c.ContentType, parameters.ContentType);
        }

        if (!string.IsNullOrEmpty(parameters.Tag))
        {
            filter &= filterBuilder.AnyEq(c => c.Tags, parameters.Tag);
        }

        if (!string.IsNullOrEmpty(parameters.Status))
        {
            if (Enum.TryParse<Models.ContentStatus>(parameters.Status, out var status))
            {
                filter &= filterBuilder.Eq(c => c.Status, status);
            }
        }

        if (!string.IsNullOrEmpty(parameters.Language))
        {
            filter &= filterBuilder.Eq(c => c.Language, parameters.Language);
        }

        // Add more filters as needed

        return await _contents.Find(filter).ToListAsync();
    }

    public async Task CreateAsync(Models.Content content)
    {
        await _contents.InsertOneAsync(content);
    }

    public async Task UpdateAsync(Models.Content content)
    {
        await _contents.ReplaceOneAsync(c => c.Id == content.Id, content);
    }

    public async Task DeleteAsync(string id)
    {
        await _contents.DeleteOneAsync(c => c.Id == id);
    }
}
