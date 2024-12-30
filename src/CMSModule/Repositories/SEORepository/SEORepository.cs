using CMSModule.Models;
using MongoDB.Driver;

namespace CMSModule.Repositories.SEORepository;

public class SEORepository : ISEORepository
{
    private readonly IMongoCollection<SEOSettings> _seoSettings;

    public SEORepository(IMongoDatabase database)
    {
        _seoSettings = database.GetCollection<SEOSettings>("SEOSettings");
    }

    public async Task<SEOSettings> GetSEOSettingsAsync()
    {
        return await _seoSettings.Find(_ => true).FirstOrDefaultAsync();
    }

    public async Task SaveSEOSettingsAsync(SEOSettings settings)
    {
        var existing = await _seoSettings.Find(_ => true).FirstOrDefaultAsync();
        if (existing == null)
        {
            await _seoSettings.InsertOneAsync(settings);
        }
        else
        {
            settings.Id = existing.Id; // Ensure the same ID
            await _seoSettings.ReplaceOneAsync(s => s.Id == settings.Id, settings);
        }
    }
}