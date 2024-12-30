using CMSModule.Models;
using CMSModule.Services.MediaService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CMSModule.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "cms.read")]
public class MediaController : ControllerBase
{
    private readonly IMediaService _mediaService;

    public MediaController(IMediaService mediaService)
    {
        _mediaService = mediaService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<string>> GetMediaUrl(string id)
    {
        var url = await _mediaService.GetMediaUrlAsync(id);
        return Ok(url);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Media>>> GetAllMedia()
    {
        var media = await _mediaService.GetAllMediaAsync();
        return Ok(media);
    }

    [HttpPost("upload")]
    [Authorize(Policy = "cms.write")]
    public async Task<ActionResult<Media>> UploadMedia([FromForm] IFormFile file)
    {
        var uploadedBy = User.Identity.Name ?? "system"; // Replace "system" with actual user
        var result = await _mediaService.UploadMediaAsync(file, uploadedBy);
        if (result.IsSuccess)
        {
            var media = result.Value;
            return CreatedAtAction(nameof(GetMediaUrl), new { id = media.Id }, media);
        }
        return BadRequest(result.Errors);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "cms.delete")]
    public async Task<ActionResult> DeleteMedia(string id)
    {
        await _mediaService.DeleteMediaAsync(id);
        return NoContent();
    }
}
