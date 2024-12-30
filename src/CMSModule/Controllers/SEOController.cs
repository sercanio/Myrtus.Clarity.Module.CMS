using CMSModule.DTOs;
using CMSModule.Models;
using CMSModule.Services.SEOService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "cms.read")]
public class SEOController : ControllerBase
{
    private readonly ISEOService _seoService;

    public SEOController(ISEOService seoService)
    {
        _seoService = seoService;
    }

    [HttpGet]
    public async Task<ActionResult<SEOSettings>> GetSEOSettings()
    {
        var settings = await _seoService.GetSEOSettingsAsync();
        if (settings == null)
            return NotFound();

        return Ok(settings);
    }

    [HttpPost]
    [Authorize(Policy = "cms.write")]
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