using Ardalis.Result;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using CMSModule.Events;
using CMSModule.Errors;
using CMSModule.Models;
using CMSModule.Repositories.MediaRepository;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using System;
using System.Threading.Tasks;
using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.Clarity.Core.Application.Abstractions.Authentication;
using Myrtus.Clarity.Application.Services.Users;
using Myrtus.Clarity.Core.Application.Abstractions.Pagination;
using Myrtus.Clarity.Core.Infrastructure.Dynamic;
using Myrtus.Clarity.Core.Infrastructure.Pagination;

namespace CMSModule.Services.MediaService;

public class MediaService : IMediaService
{
    private readonly IMediaRepository _mediaRepository;
    private readonly BlobContainerClient _containerClient;
    private readonly IMediator _mediator;
    private readonly IUserContext _userContext;
    private readonly IUserService _userService;

    public MediaService(IConfiguration configuration, IMediaRepository mediaRepository, IMediator mediator, IUserContext userContext, IUserService userService)
    {
        _mediaRepository = mediaRepository;
        _mediator = mediator;

        var connectionString = configuration["AzureBlobStorage:ConnectionString"];
        var containerName = configuration["AzureBlobStorage:ContainerName"];

        if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(containerName))
        {
            throw new ArgumentException("Azure Blob Storage connection string or container name is missing.");
        }

        try
        {
            var blobServiceClient = new BlobServiceClient(connectionString);
            _containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            _containerClient.CreateIfNotExists();
        }
        catch (FormatException ex)
        {
            throw new ArgumentException("Azure Blob Storage connection string is not in the correct format.", ex);
        }

        _userContext = userContext;
        _userService = userService;
    }

    public async Task<Result<Media>> UploadMediaAsync(IFormFile file, CancellationToken cancellationToken)
    {
        if (file == null || file.Length == 0)
            return Result.Invalid();

        var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
        var blobClient = _containerClient.GetBlobClient(uniqueFileName);

        try
        {
            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, overwrite: true, cancellationToken);
            }
        }
        catch (Exception ex)
        {
            // Log the exception as needed
            return Result.Error(CMSModuleErrors.Media.UploadFailed(ex.Message).Name);
        }

        var media = new Media
        {
            Id = ObjectId.GenerateNewId().ToString(),
            FileName = file.FileName,
            BlobUri = blobClient.Uri.ToString(),
            ContentType = file.ContentType,
            Size = file.Length,
            UploadedAt = DateTime.UtcNow,
            UploadedBy = "user",
        };

        await _mediaRepository.AddAsync(media, cancellationToken);

        // Raise Domain Event
        await _mediator.Publish(new MediaUploadedEvent(media.Id, media.FileName, "user"));

        return Result.Success(media);
    }

    public async Task<Result<Media>> GetMediaByIdAsync(string id, CancellationToken cancellationToken)
    {
        var media = await _mediaRepository.GetAsync(m => m.Id == id, cancellationToken);
        if (media == null)
            return Result.NotFound(CMSModuleErrors.Media.NotFound.Name);

        return Result.Success(media);
    }

    public async Task<Result<IPaginatedList<Media>>> GetAllMediaAsync(CancellationToken cancellationToken)
    {
        var mediaList = await _mediaRepository.GetAllAsync(cancellationToken: cancellationToken);
        return Result.Success(mediaList);
    }

    public async Task<Result<IPaginatedList<Media>>> GetAllMediaDynamicAsync(DynamicQuery dynamicQuery, int pageIndex, int pageSize, CancellationToken cancellationToken)
    {
        var mediaList = await _mediaRepository.GetAllAsync(pageIndex, pageSize, null, cancellationToken);
        var filteredMediaList = mediaList.Items.AsQueryable().ToDynamic(dynamicQuery);

        var paginatedMediaList = filteredMediaList
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .ToList();

        var paginatedList = new PaginatedList<Media>(
            paginatedMediaList,
            filteredMediaList.Count(),
            pageIndex,
            pageSize
        );

        return Result.Success<IPaginatedList<Media>>(paginatedList);
    }

    public async Task<Result> DeleteMediaAsync(string id, CancellationToken cancellationToken)
    {
        var media = await _mediaRepository.GetAsync(m => m.Id == id, cancellationToken);
        if (media == null)
            return Result.NotFound(CMSModuleErrors.Media.NotFound.Name);

        var blobClient = _containerClient.GetBlobClient(media.Id);

        try
        {
            await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            // Log the exception as needed
            return Result.Error(new DomainError("Media.DeleteFailed", 500, ex.Message).Name);
        }

        await _mediaRepository.DeleteAsync(m => m.Id == id, cancellationToken);

        // Raise Domain Event
        await _mediator.Publish(new MediaDeletedEvent(id, media.FileName, "system"), cancellationToken); // Replace "system" with actual user

        return Result.Success();
    }

    public async Task<Result<string>> GetMediaUrlAsync(string id, CancellationToken cancellationToken)
    {
        var media = await _mediaRepository.GetAsync(m => m.Id == id, cancellationToken);
        if (media == null)
            return Result.NotFound(CMSModuleErrors.Media.NotFound.Name);

        var blobClient = new BlobClient(new Uri(media.BlobUri));
        if (!blobClient.CanGenerateSasUri)
            return Result.Error(CMSModuleErrors.Media.CannotGenerateSasUri.Name);

        var sasBuilder = new BlobSasBuilder
        {
            BlobContainerName = _containerClient.Name,
            BlobName = blobClient.Name,
            Resource = "b",
            ExpiresOn = DateTimeOffset.UtcNow.AddHours(1) // Adjust expiration as needed
        };
        sasBuilder.SetPermissions(BlobSasPermissions.Read);

        // Assume storage account key is set in environment variables
        var storageAccountName = _containerClient.AccountName;
        var storageAccountKey = Environment.GetEnvironmentVariable("AZURE_STORAGE_KEY");

        if (string.IsNullOrEmpty(storageAccountKey))
        {
            return Result.Error(CMSModuleErrors.Media.StorageKeyMissing.Name);
        }

        var credential = new StorageSharedKeyCredential(storageAccountName, storageAccountKey);
        var sasToken = sasBuilder.ToSasQueryParameters(credential).ToString();

        var sasUri = $"{blobClient.Uri}?{sasToken}";
        return Result.Success(sasUri);
    }
}
