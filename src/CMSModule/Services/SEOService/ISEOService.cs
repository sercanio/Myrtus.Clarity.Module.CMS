using Ardalis.Result;
using CMSModule.Models;

namespace CMSModule.Services.SEOService
{
    public interface ISEOService
    {

        Task<Result<SEOSettings>> GetSEOSettingsAsync(CancellationToken cancellationToken);
        Task<Result> SaveSEOSettingsAsync(SEOSettings settings, CancellationToken cancellationToken);
    }
}
