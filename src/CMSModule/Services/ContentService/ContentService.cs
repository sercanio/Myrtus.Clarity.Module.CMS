using Ardalis.Result;
using CMSModule.Errors;
using CMSModule.Events;
using CMSModule.Models;
using CMSModule.Repositories.ContentRepository;
using MediatR;
using MongoDB.Bson;

namespace CMSModule.Services.ContentService;

public class ContentService : IContentService
{
    private readonly IContentRepository _contentRepository;
    private readonly IMediator _mediator;

    public ContentService(IContentRepository contentRepository, IMediator mediator)
    {
        _contentRepository = contentRepository;
        _mediator = mediator;
    }

    public async Task<Result<Content>> GetContentByIdAsync(string id)
    {
        var content = await _contentRepository.GetByIdAsync(id);
        if (content == null)
        {
            return Result.NotFound(CMSModuleErrors.Content.NotFound.Name);
        }

        return Result.Success(content);
    }

    public async Task<Result<Content>> GetContentBySlugAsync(string slug)
    {
        var content = await _contentRepository.GetBySlugAsync(slug);
        if (content == null)
        {
            return Result.NotFound(CMSModuleErrors.Content.NotFound.Name);
        }

        return Result.Success(content);
    }

    public async Task<Result<IEnumerable<Content>>> GetAllContentsAsync()
    {
        var contents = await _contentRepository.GetAllAsync();
        return Result.Success(contents);
    }

    public async Task<Result<IEnumerable<Content>>> QueryContentsAsync(ContentQueryParameters parameters)
    {
        var contents = await _contentRepository.QueryAsync(parameters);
        return Result.Success(contents);
    }

    public async Task<Result<Content>> CreateContentAsync(Content content, string createdBy)
    {
        content.Id = ObjectId.GenerateNewId().ToString();
        content.CreatedAt = DateTime.UtcNow;
        content.UpdatedAt = DateTime.UtcNow;
        content.Versions = new List<ContentVersion>
            {
                new ContentVersion
                {
                    VersionNumber = 1,
                    Title = content.Title,
                    Body = content.Body,
                    ModifiedAt = content.CreatedAt,
                    ModifiedBy = createdBy
                }
            };

        await _contentRepository.CreateAsync(content);

        // Raise Domain Event
        await _mediator.Publish(new ContentCreatedEvent(content.Id, content.Title, createdBy));

        return Result.Success(content);
    }

    public async Task<Result> UpdateContentAsync(Content content, string modifiedBy)
    {
        var existingContent = await _contentRepository.GetByIdAsync(content.Id);
        if (existingContent == null)
        {
            return Result.NotFound(CMSModuleErrors.Content.NotFound.Name);
        }

        // Add new version
        var newVersion = new ContentVersion
        {
            VersionNumber = existingContent.Versions.Count + 1,
            Title = content.Title,
            Body = content.Body,
            ModifiedAt = DateTime.UtcNow,
            ModifiedBy = modifiedBy
        };
        content.Versions = existingContent.Versions;
        content.Versions.Add(newVersion);

        content.UpdatedAt = DateTime.UtcNow;

        await _contentRepository.UpdateAsync(content);

        // Check for status change to Published
        if (existingContent.Status != content.Status && content.Status == ContentStatus.Published)
        {
            await _mediator.Publish(new ContentPublishedEvent(content.Id, content.Title, modifiedBy));
        }

        // Raise Domain Event for Update
        await _mediator.Publish(new ContentUpdatedEvent(content.Id, content.Title, modifiedBy));

        return Result.Success();
    }

    public async Task<Result> DeleteContentAsync(string id)
    {
        var existingContent = await _contentRepository.GetByIdAsync(id);
        if (existingContent == null)
        {
            return Result.NotFound(CMSModuleErrors.Content.NotFound.Name);
        }

        await _contentRepository.DeleteAsync(id);

        // Raise Domain Event
        await _mediator.Publish(new ContentDeletedEvent(id, existingContent.Title, "system")); // Replace "system" with actual user

        return Result.Success();
    }

    public async Task<Result> RestoreContentVersionAsync(string id, int versionNumber)
    {
        var content = await _contentRepository.GetByIdAsync(id);
        if (content == null)
        {
            return Result.NotFound(CMSModuleErrors.Content.NotFound.Name);
        }

        var version = content.Versions.FirstOrDefault(v => v.VersionNumber == versionNumber);
        if (version == null)
        {
            return Result.NotFound(CMSModuleErrors.Content.VersionNotFound.Name);
        }

        content.Title = version.Title;
        content.Body = version.Body;
        content.Status = ContentStatus.Draft; // Or set as per your workflow
        content.UpdatedAt = DateTime.UtcNow;

        // Add a new version entry
        var newVersion = new ContentVersion
        {
            VersionNumber = content.Versions.Count + 1,
            Title = content.Title,
            Body = content.Body,
            ModifiedAt = content.UpdatedAt,
            ModifiedBy = "system" // Replace with actual user
        };
        content.Versions.Add(newVersion);

        await _contentRepository.UpdateAsync(content);

        // Raise Domain Event
        await _mediator.Publish(new ContentRestoredEvent(id, content.Title, "system")); // Replace "system" with actual user

        return Result.Success();
    }
}
