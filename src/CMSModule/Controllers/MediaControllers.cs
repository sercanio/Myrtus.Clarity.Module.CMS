using CMSModule.DTOs;
using CMSModule.Models;
using CMSModule.Services.MediaService;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Myrtus.Clarity.Core.WebAPI;
using Myrtus.Clarity.Core.WebAPI.Controllers;

namespace CMSModule.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/Media")]
[EnableRateLimiting("fixed")]
public class MediaController : BaseController
{
    private readonly IMediaService _mediaService;

    public MediaController(IMediaService mediaService, ISender sender, IErrorHandlingService errorHandlingService)
        : base(sender, errorHandlingService)
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

    /// <summary>
    /// Uploads media to the server.
    /// </summary>
    /// <param name="file">The media file to upload.</param>
    /// <returns>A response indicating the success of the upload.</returns>
    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    public IActionResult UploadMedia([FromForm] UploadMediaRequest request)
    {
        if (request.File == null || request.File.Length == 0)
            return BadRequest("File is missing or empty.");

        // Perform file processing logic
        return Ok("File uploaded successfully.");
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteMedia(string id)
    {
        await _mediaService.DeleteMediaAsync(id);
        return NoContent();
    }
}
