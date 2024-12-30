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

namespace CMSModule.Services.MediaService;

public class MediaService : IMediaService
{
    private readonly IMediaRepository _mediaRepository;
    private readonly BlobContainerClient _containerClient;
    private readonly IMediator _mediator;

    public MediaService(IConfiguration configuration, IMediaRepository mediaRepository, IMediator mediator)
    {
        _mediaRepository = mediaRepository;
        _mediator = mediator;

        var connectionString = configuration["AzureBlobStorage:ConnectionString"];
        var containerName = configuration["AzureBlobStorage:ContainerName"];

        var blobServiceClient = new BlobServiceClient(connectionString);
        _containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        _containerClient.CreateIfNotExists();
    }

    public async Task<Result<Media>> UploadMediaAsync(IFormFile file, string uploadedBy)
    {
        if (file == null || file.Length == 0)
            return Result.Invalid();

        var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
        var blobClient = _containerClient.GetBlobClient(uniqueFileName);

        try
        {
            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, overwrite: true);
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
            UploadedBy = uploadedBy
        };

        await _mediaRepository.CreateAsync(media);

        // Raise Domain Event
        await _mediator.Publish(new MediaUploadedEvent(media.Id, media.FileName, uploadedBy));

        return Result.Success(media);
    }

    public async Task<Result<Media>> GetMediaByIdAsync(string id)
    {
        var media = await _mediaRepository.GetByIdAsync(id);
        if (media == null)
            return Result.NotFound(CMSModuleErrors.Media.NotFound.Name);

        return Result.Success(media);
    }

    public async Task<Result<IEnumerable<Media>>> GetAllMediaAsync()
    {
        var mediaList = await _mediaRepository.GetAllAsync();
        return Result.Success(mediaList);
    }

    public async Task<Result> DeleteMediaAsync(string id)
    {
        var media = await _mediaRepository.GetByIdAsync(id);
        if (media == null)
            return Result.NotFound(CMSModuleErrors.Media.NotFound.Name);

        var blobClient = new BlobClient(new Uri(media.BlobUri));

        try
        {
            await blobClient.DeleteIfExistsAsync();
        }
        catch (Exception ex)
        {
            // Log the exception as needed
            return Result.Error(new DomainError("Media.DeleteFailed", 500, ex.Message).Name);
        }

        await _mediaRepository.DeleteAsync(id);

        // Raise Domain Event
        await _mediator.Publish(new MediaDeletedEvent(id, media.FileName, "system")); // Replace "system" with actual user

        return Result.Success();
    }

    public async Task<Result<string>> GetMediaUrlAsync(string id)
    {
        var media = await _mediaRepository.GetByIdAsync(id);
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
