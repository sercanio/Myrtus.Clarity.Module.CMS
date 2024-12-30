using CMSModule.Models;

namespace CMSModule.Services.SEOService;

public interface ISEOService
{
    Task<SEOSettings> GetSEOSettingsAsync();
    Task SaveSEOSettingsAsync(SEOSettings settings);
}