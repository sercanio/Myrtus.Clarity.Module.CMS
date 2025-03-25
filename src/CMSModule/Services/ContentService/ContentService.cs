using Ardalis.Result;
using CMSModule.Errors;
using CMSModule.Events;
using CMSModule.Models;
using CMSModule.Repositories.ContentRepository;
using MediatR;
using MongoDB.Bson;
using Myrtus.Clarity.Core.Application.Abstractions.Authentication;
using Myrtus.Clarity.Core.Application.Abstractions.Pagination;
using Myrtus.Clarity.Core.Infrastructure.Dynamic;
using Myrtus.Clarity.Core.Infrastructure.Pagination;
using AppTemplate.Application.Services.AppUsers;

namespace CMSModule.Services.ContentService;

public class ContentService : IContentService
{
    private readonly IContentRepository _contentRepository;
    private readonly IMediator _mediator;
    private readonly IUserContext _userContext;
    private readonly IAppUsersService _userService;

    public ContentService(
        IContentRepository contentRepository,
        IMediator mediator,
        IUserContext userContext,
        IAppUsersService userService)
    {
        _contentRepository = contentRepository;
        _mediator = mediator;
        _userContext = userContext;
        _userService = userService;
    }

    public async Task<Result<bool>> CheckIfContentExistsBySlugAsync(string slug, CancellationToken cancellationToken)
    {
        bool check = await _contentRepository.CheckIfContentExistsBySlugAsync(slug, cancellationToken);

        return Result.Success<bool>(check);
    }

    public async Task<Result<Content>> GetContentByIdAsync(string id, CancellationToken cancellationToken)
    {
        var content = await _contentRepository.GetAsync(c => c.Id == id, cancellationToken);
        if (content == null)
        {
            return Result.NotFound(CMSModuleErrors.Content.NotFound.Name);
        }

        return Result.Success(content);
    }

    public async Task<Result<Content>> GetContentBySlugAsync(string slug, CancellationToken cancellationToken)
    {
        var content = await _contentRepository.GetAsync(c => c.Slug == slug, cancellationToken);
        if (content == null)
        {
            return Result.NotFound(CMSModuleErrors.Content.NotFound.Name);
        }

        return Result.Success(content);
    }

    public async Task<Result<IPaginatedList<Content>>> GetAllContentsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken)
    {
        var contents = await _contentRepository.GetAllAsync(pageIndex, pageSize, null, cancellationToken);
        return Result.Success(contents);
    }

    public async Task<Result<IPaginatedList<Content>>> GetAllContentsDynamicAsync(DynamicQuery dynamicQuery, int pageIndex, int pageSize, CancellationToken cancellationToken)
    {
        var contents = await _contentRepository.GetAllAsync(pageIndex, pageSize, null, cancellationToken);
        var filteredContents = contents.Items.AsQueryable().ToDynamic(dynamicQuery);

        var paginatedContents = filteredContents
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .ToList();

        var paginatedList = new PaginatedList<Content>(
            paginatedContents,
            filteredContents.Count(),
            pageIndex,
            pageSize
        );

        return Result.Success<IPaginatedList<Content>>(paginatedList);
    }

    public async Task<Result<Content>> CreateContentAsync(Content content, string createdBy, CancellationToken cancellationToken)
    {
        var slugExists = await CheckIfContentExistsBySlugAsync(content.Slug, cancellationToken);
        if (slugExists.Value)
        {
            return Result.Conflict(CMSModuleErrors.Content.SlugExists.Name);
        }

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
                CoverImageUrl = content.CoverImageUrl,
                ModifiedAt = content.CreatedAt,
                ModifiedBy = createdBy
            }
        };

        await _contentRepository.AddAsync(content, cancellationToken);

        await _mediator.Publish(new ContentCreatedEvent(content.Id, content.Title, createdBy), cancellationToken);

        return Result.Success(content);
    }

    public async Task<Result> UpdateContentAsync(Content content, string modifiedBy, CancellationToken cancellationToken)
    {
        var existingContent = await _contentRepository.GetAsync(c => c.Id == content.Id, cancellationToken);
        if (existingContent == null)
        {
            return Result.NotFound(CMSModuleErrors.Content.NotFound.Name);
        }

        var newVersion = new ContentVersion
        {
            VersionNumber = existingContent.Versions.Count + 1,
            Title = content.Title,
            Body = content.Body,
            CoverImageUrl = content.CoverImageUrl,
            ModifiedAt = DateTime.UtcNow,
            ModifiedBy = modifiedBy
        };
        content.Versions = existingContent.Versions;
        content.Versions.Add(newVersion);

        content.UpdatedAt = DateTime.UtcNow;

        await _contentRepository.UpdateAsync(content, cancellationToken);

        if (existingContent.Status != content.Status && content.Status == ContentStatus.Published)
        {
            await _mediator.Publish(new ContentPublishedEvent(content.Id, content.Title, modifiedBy));
        }

        await _mediator.Publish(new ContentUpdatedEvent(content.Id, content.Title, modifiedBy));

        return Result.Success();
    }

    public async Task<Result> DeleteContentAsync(string id, CancellationToken cancellationToken)
    {
        var existingContent = await _contentRepository.GetAsync(c => c.Id == id, cancellationToken);
        if (existingContent == null)
        {
            return Result.NotFound(CMSModuleErrors.Content.NotFound.Name);
        }

        await _contentRepository.DeleteAsync(c => c.Id == id, cancellationToken);

        await _mediator.Publish(new ContentDeletedEvent(id, existingContent.Title, "system")); // Replace "system" with actual user

        return Result.Success();
    }

    public async Task<Result> RestoreContentVersionAsync(string id, int versionNumber, CancellationToken cancellationToken)
    {
        const int MaxVersions = 10;

        var content = await _contentRepository.GetAsync(c => c.Id == id, cancellationToken);
        if (content == null)
        {
            return Result.NotFound(CMSModuleErrors.Content.NotFound.Name);
        }

        var version = content.Versions.FirstOrDefault(v => v.VersionNumber == versionNumber);
        if (version == null)
        {
            return Result.NotFound(CMSModuleErrors.Content.VersionNotFound.Name);
        }

        content.Versions.Remove(version);
        content.Versions.Add(version);

        for (int i = 0; i < content.Versions.Count; i++)
        {
            content.Versions[i].VersionNumber = i + 1;
        }

        content.Title = version.Title;
        content.Body = version.Body;
        content.CoverImageUrl = version.CoverImageUrl;
        content.Status = ContentStatus.Draft;
        content.UpdatedAt = DateTime.UtcNow;

        var user = await _userService.GetUserByIdAsync(_userContext.UserId, cancellationToken);

        if (content.Versions.Count > MaxVersions)
        {
            content.Versions = content.Versions
                .OrderByDescending(v => v.VersionNumber)
                .Take(MaxVersions)
                .OrderBy(v => v.VersionNumber)
                .ToList();
        }

        await _contentRepository.UpdateAsync(content, cancellationToken);

        await _mediator.Publish(new ContentRestoredEvent(id, content.Title, user.Id.ToString()));

        return Result.Success();
    }
}
