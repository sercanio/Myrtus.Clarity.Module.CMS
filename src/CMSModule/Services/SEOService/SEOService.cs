using CMSModule.Models;
using CMSModule.Repositories.SEORepository;

namespace CMSModule.Services.SEOService;

public class SEOService : ISEOService
{
    private readonly ISEORepository _seoRepository;

    public SEOService(ISEORepository seoRepository)
    {
        _seoRepository = seoRepository;
    }

    public async Task<SEOSettings> GetSEOSettingsAsync()
    {
        return await _seoRepository.GetSEOSettingsAsync();
    }

    public async Task SaveSEOSettingsAsync(SEOSettings settings)
    {
        await _seoRepository.SaveSEOSettingsAsync(settings);
    }
}