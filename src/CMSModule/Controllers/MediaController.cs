using CMSModule.Controllers.DTOs;
using CMSModule.DTOs;
using CMSModule.Services.MediaService;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Myrtus.Clarity.Core.Infrastructure.Authorization;
using Myrtus.Clarity.Core.Infrastructure.Dynamic;
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
    [HasPermission(Attributes.CMSPermissions.MediaRead)]
    public async Task<ActionResult<string>> GetMediaUrl(string id, CancellationToken cancellationToken)
    {
        var url = await _mediaService.GetMediaUrlAsync(id, cancellationToken);
        return Ok(url);
    }

    [HttpGet]
    [HasPermission(Attributes.CMSPermissions.MediaRead)]
    public async Task<IActionResult> GetAllMedia(CancellationToken cancellationToken)
    {
        var result = await _mediaService.GetAllMediaAsync(cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : _errorHandlingService.HandleErrorResponse(result);
    }

    [HttpPost("dynamic")]
    [HasPermission(Attributes.CMSPermissions.MediaRead)]
    public async Task<IActionResult> GetAllMediaDynamic([FromBody] DynamicQuery dynamicQuery, [FromQuery] int pageIndex = 0, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var result = await _mediaService.GetAllMediaDynamicAsync(dynamicQuery, pageIndex, pageSize, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : _errorHandlingService.HandleErrorResponse(result);
    }

    /// <summary>
    /// Uploads media to the server.
    /// </summary>
    /// <param name="file">The media file to upload.</param>
    /// <returns>A response indicating the success of the upload.</returns>
    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    [HasPermission(Attributes.CMSPermissions.MediaCreate)]
    public async Task<IActionResult> UploadMedia([FromForm] UploadMediaRequest request, CancellationToken cancellationToken)
    {
        if (request.File == null || request.File.Length == 0)
            return BadRequest("File is missing or empty.");

        var result = await _mediaService.UploadMediaAsync(request.File, cancellationToken);

        if (!result.IsSuccess)
        {
            return _errorHandlingService.HandleErrorResponse(result);
        }

        var response = new UploadMediaResponse
        {
            Id = result.Value.Id,
            FileName = result.Value.FileName,
            BlobUri = result.Value.BlobUri,
            ContentType = result.Value.ContentType,
            Size = result.Value.Size,
            UploadedAt = result.Value.UploadedAt,
            UploadedBy = result.Value.UploadedBy
        };

        return CreatedAtAction(nameof(GetMediaUrl), new { id = response.Id }, response);
    }

    [HttpDelete("{id}")]
    [HasPermission(Attributes.CMSPermissions.MediaDelete)]
    public async Task<IActionResult> DeleteMedia(string id, CancellationToken cancellationToken)
    {
        var result = await _mediaService.DeleteMediaAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : _errorHandlingService.HandleErrorResponse(result);
    }
}
