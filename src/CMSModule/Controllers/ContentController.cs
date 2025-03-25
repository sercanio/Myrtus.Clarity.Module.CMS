using CMSModule.DTOs;
using CMSModule.Models;
using CMSModule.Services.ContentService;
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
[Route("api/v{version:apiVersion}/Content")]
[EnableRateLimiting("fixed")]
public class ContentController : BaseController
{
    private readonly IContentService _contentService;

    public ContentController(IContentService contentService, ISender sender, IErrorHandlingService errorHandlingService)
        : base(sender, errorHandlingService)
    {
        _contentService = contentService;
    }

    [HttpGet("slug/exists/{slug}")]
    [HasPermission(Attributes.CMSPermissions.ContentRead)]
    public async Task<IActionResult> CheckIfContentBySlugExists(string slug, CancellationToken cancellationToken)
    {
        var result = await _contentService.CheckIfContentExistsBySlugAsync(slug, cancellationToken);
        return result.IsSuccess ? Ok(new { exists = result.Value, message = "Content exists." }) : _errorHandlingService.HandleErrorResponse(result);
    }

    [HttpGet("{id}")]
    [HasPermission(Attributes.CMSPermissions.ContentRead)]
    public async Task<IActionResult> GetContentById(string id, CancellationToken cancellationToken)
    {
        var result = await _contentService.GetContentByIdAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : _errorHandlingService.HandleErrorResponse(result);
    }

    [HttpGet("slug/{slug}")]
    [HasPermission(Attributes.CMSPermissions.ContentRead)]
    public async Task<IActionResult> GetContentBySlug(string slug, CancellationToken cancellationToken)
    {
        var result = await _contentService.GetContentBySlugAsync(slug, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : _errorHandlingService.HandleErrorResponse(result);
    }

    [HttpGet]
    [HasPermission(Attributes.CMSPermissions.ContentRead)]
    public async Task<IActionResult> GetAllContents(int pageIndex, int pageSize, CancellationToken cancellationToken)
    {
        var result = await _contentService.GetAllContentsAsync(pageIndex, pageSize, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : _errorHandlingService.HandleErrorResponse(result);
    }

    [HttpPost("dynamic")]
    [HasPermission(Attributes.CMSPermissions.ContentRead)]
    public async Task<IActionResult> GetAllContentsDynamic([FromBody] DynamicQuery dynamicQuery, [FromQuery] int pageIndex = 0, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var result = await _contentService.GetAllContentsDynamicAsync(dynamicQuery, pageIndex, pageSize, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : _errorHandlingService.HandleErrorResponse(result);
    }

    [HttpPost]
    [HasPermission(Attributes.CMSPermissions.ContentCreate)]
    public async Task<IActionResult> CreateContent([FromBody] ContentDto contentDto, CancellationToken cancellationToken)
    {
        var content = new Content
        {
            ContentType = contentDto.ContentType,
            Title = contentDto.Title,
            Slug = contentDto.Slug,
            Body = contentDto.Body,
            Tags = contentDto.Tags,
            Status = contentDto.Status,
            Language = contentDto.Language,
            CoverImageUrl = contentDto.CoverImageUrl,
            MetaTitle = contentDto.MetaTitle,
            MetaDescription = contentDto.MetaDescription,
            MetaKeywords = contentDto.MetaKeywords
        };

        var result = await _contentService.CreateContentAsync(content, User.Identity.Name ?? "system", cancellationToken); // Replace "system" with actual user

        return result.IsSuccess ? CreatedAtAction(nameof(GetContentById), new { id = result.Value.Id }, result.Value) : _errorHandlingService.HandleErrorResponse(result);
    }

    [HttpPut("{id}")]
    [HasPermission(Attributes.CMSPermissions.ContentUpdate)]
    public async Task<IActionResult> UpdateContent(string id, [FromBody] ContentDto contentDto, CancellationToken cancellationToken)
    {
        var existingContentResult = await _contentService.GetContentByIdAsync(id, cancellationToken);
        if (!existingContentResult.IsSuccess)
        {
            return _errorHandlingService.HandleErrorResponse(existingContentResult);
        }

        var existingContent = existingContentResult.Value;

        existingContent.ContentType = contentDto.ContentType;
        existingContent.Title = contentDto.Title;
        existingContent.Slug = contentDto.Slug;
        existingContent.Body = contentDto.Body;
        existingContent.Tags = contentDto.Tags;
        existingContent.Status = contentDto.Status;
        existingContent.Language = contentDto.Language;
        existingContent.CoverImageUrl = contentDto.CoverImageUrl;
        existingContent.MetaTitle = contentDto.MetaTitle;
        existingContent.MetaDescription = contentDto.MetaDescription;
        existingContent.MetaKeywords = contentDto.MetaKeywords;

        var updateResult = await _contentService.UpdateContentAsync(existingContent, User.Identity.Name ?? "system", cancellationToken); // Replace "system" with actual user

        return updateResult.IsSuccess ? NoContent() : _errorHandlingService.HandleErrorResponse(updateResult);
    }

    [HttpDelete("{id}")]
    [HasPermission(Attributes.CMSPermissions.ContentDelete)]
    public async Task<IActionResult> DeleteContent(string id, CancellationToken cancellationToken)
    {
        var result = await _contentService.DeleteContentAsync(id, cancellationToken);
        return result.IsSuccess ? NoContent() : _errorHandlingService.HandleErrorResponse(result);
    }

    [HttpPost("{id}/restore/{versionNumber}")]
    [HasPermission(Attributes.CMSPermissions.ContentUpdate)]
    public async Task<IActionResult> RestoreContentVersion(string id, int versionNumber, CancellationToken cancellationToken)
    {
        var result = await _contentService.RestoreContentVersionAsync(id, versionNumber, cancellationToken);
        return result.IsSuccess ? NoContent() : _errorHandlingService.HandleErrorResponse(result);
    }
}