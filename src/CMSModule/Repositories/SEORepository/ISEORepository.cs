using CMSModule.Models;

namespace CMSModule.Repositories.SEORepository;

public interface ISEORepository
{
    Task<SEOSettings> GetSEOSettingsAsync();
    Task SaveSEOSettingsAsync(SEOSettings settings);
}