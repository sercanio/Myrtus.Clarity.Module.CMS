using CMSModule.Models;
using MongoDB.Driver;

namespace CMSModule.Repositories.MediaRepository;

public class MediaRepository : IMediaRepository
{
    private readonly IMongoCollection<Media> _media;

    public MediaRepository(IMongoDatabase database)
    {
        _media = database.GetCollection<Media>("Media");
    }

    public async Task<Media> GetByIdAsync(string id)
    {
        return await _media.Find(m => m.Id == id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Media>> GetAllAsync()
    {
        return await _media.Find(_ => true).ToListAsync();
    }

    public async Task CreateAsync(Media media)
    {
        await _media.InsertOneAsync(media);
    }

    public async Task DeleteAsync(string id)
    {
        await _media.DeleteOneAsync(m => m.Id == id);
    }
}