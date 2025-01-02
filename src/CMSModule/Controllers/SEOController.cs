using CMSModule.DTOs;
using CMSModule.Models;
using CMSModule.Services.SEOService;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Myrtus.Clarity.Core.WebAPI;
using Myrtus.Clarity.Core.WebAPI.Controllers;
using NSwag.Annotations;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/SEO")]
[EnableRateLimiting("fixed")]
public partial class SEOController : BaseController
{
    private readonly ISEOService _seoService;

    public SEOController(ISEOService seoService, ISender sender, IErrorHandlingService errorHandlingService) : base(sender, errorHandlingService)
    {
        _seoService = seoService;
    }

    [HttpGet]
    [OpenApiOperation("GetSEOSettings", "Retrieves the SEO settings.")]
    public async Task<ActionResult<SEOSettings>> GetSEOSettings()
    {
        var settings = await _seoService.GetSEOSettingsAsync();
        if (settings == null)
            return NotFound();

        return Ok(settings);
    }

    [HttpPost]
    public async Task<ActionResult> SaveSEOSettings([FromBody] SEOSettingsDto seoDto)
    {
        var settings = new SEOSettings
        {
            DefaultMetaTitle = seoDto.DefaultMetaTitle,
            DefaultMetaDescription = seoDto.DefaultMetaDescription,
            DefaultMetaKeywords = seoDto.DefaultMetaKeywords
        };

        await _seoService.SaveSEOSettingsAsync(settings);

        return NoContent();
    }
}
