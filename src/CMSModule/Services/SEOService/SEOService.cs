// src/modules/cms/services/SEOService/SEOService.cs

using Ardalis.Result;
using CMSModule.Models;
using CMSModule.Repositories.SeoRepository;
using MongoDB.Bson;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CMSModule.Services.SEOService
{
    public class SEOService : ISEOService
    {
        private readonly ISeoRepository _seoRepository;

        public SEOService(ISeoRepository seoRepository)
        {
            _seoRepository = seoRepository;
        }

        public async Task<Result<SEOSettings>> GetSEOSettingsAsync(CancellationToken cancellationToken)
        {
            var settings = await _seoRepository.GetAsync(c => true, cancellationToken);
            if (settings == null)
            {
                return Result.NotFound("SEO settings not found.");
            }

            return Result.Success(settings);
        }

        public async Task<Result> SaveSEOSettingsAsync(SEOSettings settings, CancellationToken cancellationToken)
        {
            if (settings == null)
            {
                return Result.Invalid();
            }

            try
            {
                var existingSettings = await _seoRepository.GetAsync(c => true, cancellationToken);

                if (existingSettings == null)
                {
                    settings.Id = ObjectId.GenerateNewId().ToString();

                    await _seoRepository.AddAsync(settings, cancellationToken);
                }
                else
                {
                    settings.Id = existingSettings.Id;
                    await _seoRepository.UpdateAsync(settings, cancellationToken);
                }

                return Result.Success();
            }
            catch (Exception ex)
            {
                // Log exception as needed
                return Result.Error($"An error occurred while saving SEO settings: {ex.Message}");
            }
        }
    }
}
