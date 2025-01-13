using CMSModule.DTOs;
using CMSModule.Models;
using CMSModule.Services.SEOService;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Myrtus.Clarity.Core.Infrastructure.Authorization;
using Myrtus.Clarity.Core.WebAPI;
using Myrtus.Clarity.Core.WebAPI.Controllers;


namespace CMSModule.Controllers;

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
    [HasPermission(Attributes.CMSPermissions.SEORead)]
    public async Task<ActionResult<SEOSettings>> GetSEOSettings()
    {
        var settings = await _seoService.GetSEOSettingsAsync();
        if (settings == null)
            return NotFound();

        return Ok(settings);
    }

    [HttpPost]
    [HasPermission(Attributes.CMSPermissions.SEOCreate)]
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
