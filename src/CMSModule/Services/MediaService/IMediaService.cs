using Ardalis.Result;
using CMSModule.Models;
using Microsoft.AspNetCore.Http;

namespace CMSModule.Services.MediaService;

public interface IMediaService
{
    Task<Result<Media>> UploadMediaAsync(IFormFile file, string uploadedBy);
    Task<Result<Media>> GetMediaByIdAsync(string id);
    Task<Result<IEnumerable<Media>>> GetAllMediaAsync();
    Task<Result> DeleteMediaAsync(string id);
    Task<Result<string>> GetMediaUrlAsync(string id);
}