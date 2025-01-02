using Ardalis.Result;
using CMSModule.DTOs;
using CMSModule.Models;
using CMSModule.Services.ContentService;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Myrtus.Clarity.Core.WebAPI;
using Myrtus.Clarity.Core.WebAPI.Controllers;

namespace CMSModule.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/CMS")]
    [EnableRateLimiting("fixed")]
    public class CMSController : BaseController
    {
        private readonly IContentService _contentService;

        public CMSController(IContentService contentService, ISender sender, IErrorHandlingService errorHandlingService)
            : base(sender, errorHandlingService)
        {
            _contentService = contentService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetContentById(string id)
        {
            var result = await _contentService.GetContentByIdAsync(id);
            return result switch
            {
                { Status: ResultStatus.Ok, Value: var content } => Ok(content),
                { Status: ResultStatus.NotFound } => NotFound(new { message = result.Errors?.FirstOrDefault() }),
                _ => StatusCode(500, new { message = "An unexpected error occurred." })
            };
        }

        [HttpGet("slug/{slug}")]
        public async Task<IActionResult> GetContentBySlug(string slug)
        {
            var result = await _contentService.GetContentBySlugAsync(slug);
            return result switch
            {
                { Status: ResultStatus.Ok, Value: var content } => Ok(content),
                { Status: ResultStatus.NotFound } => NotFound(new { message = result.Errors?.FirstOrDefault() }),
                _ => StatusCode(500, new { message = "An unexpected error occurred." })
            };
        }

        [HttpGet]
        public async Task<IActionResult> GetAllContents([FromQuery] ContentQueryParameters parameters)
        {
            var result = await _contentService.GetAllContentsAsync();
            return result switch
            {
                { Status: ResultStatus.Ok, Value: var contents } => Ok(contents),
                _ => StatusCode(500, new { message = "An unexpected error occurred." })
            };
        }

        [HttpPost]
        public async Task<IActionResult> CreateContent([FromBody] ContentDto contentDto)
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
                MetaTitle = contentDto.MetaTitle,
                MetaDescription = contentDto.MetaDescription,
                MetaKeywords = contentDto.MetaKeywords
            };

            var result = await _contentService.CreateContentAsync(content, User.Identity.Name ?? "system"); // Replace "system" with actual user

            return result.Status switch
            {
                ResultStatus.Ok => CreatedAtAction(nameof(GetContentById), new { id = result.Value.Id }, result.Value),
                ResultStatus.Invalid => BadRequest(result.Errors),
                _ => StatusCode(500, new { message = "An unexpected error occurred." })
            };
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContent(string id, [FromBody] ContentDto contentDto)
        {
            var existingContentResult = await _contentService.GetContentByIdAsync(id);
            if (!existingContentResult.IsSuccess)
            {
                return existingContentResult.Status == ResultStatus.NotFound
? NotFound(new { message = existingContentResult.Errors?.FirstOrDefault() })
                    : StatusCode(500, new { message = "An unexpected error occurred." });
            }

            var existingContent = existingContentResult.Value;

            existingContent.ContentType = contentDto.ContentType;
            existingContent.Title = contentDto.Title;
            existingContent.Slug = contentDto.Slug;
            existingContent.Body = contentDto.Body;
            existingContent.Tags = contentDto.Tags;
            existingContent.Status = contentDto.Status;
            existingContent.Language = contentDto.Language;
            existingContent.MetaTitle = contentDto.MetaTitle;
            existingContent.MetaDescription = contentDto.MetaDescription;
            existingContent.MetaKeywords = contentDto.MetaKeywords;

            var updateResult = await _contentService.UpdateContentAsync(existingContent, User.Identity.Name ?? "system"); // Replace "system" with actual user

            return updateResult.Status switch
            {
                ResultStatus.Ok => NoContent(),
                ResultStatus.NotFound => NotFound(new { message = updateResult.Errors?.FirstOrDefault() }),
                ResultStatus.Invalid => BadRequest(updateResult.Errors),
                _ => StatusCode(500, new { message = "An unexpected error occurred." })
            };
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContent(string id)
        {
            var result = await _contentService.DeleteContentAsync(id);
            return result.Status switch
            {
                ResultStatus.Ok => NoContent(),
                ResultStatus.NotFound => NotFound(new { message = result.Errors?.FirstOrDefault() }),
                _ => StatusCode(500, new { message = "An unexpected error occurred." })
            };
        }

        [HttpPost("{id}/restore/{versionNumber}")]
        public async Task<IActionResult> RestoreContentVersion(string id, int versionNumber)
        {
            var result = await _contentService.RestoreContentVersionAsync(id, versionNumber);
            return result.Status switch
            {
                ResultStatus.Ok => NoContent(),
                ResultStatus.NotFound => NotFound(new { message = result.Errors?.FirstOrDefault() }),
                _ => StatusCode(500, new { message = "An unexpected error occurred." })
            };
        }
    }
}