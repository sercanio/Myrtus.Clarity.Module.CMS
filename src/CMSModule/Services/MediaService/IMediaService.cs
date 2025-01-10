using Ardalis.Result;
using CMSModule.Models;
using Microsoft.AspNetCore.Http;
using Myrtus.Clarity.Core.Application.Abstractions.Pagination;
using Myrtus.Clarity.Core.Infrastructure.Dynamic;

namespace CMSModule.Services.MediaService;

public interface IMediaService
{
    Task<Result<Media>> UploadMediaAsync(IFormFile file, CancellationToken cancellationToken);
    Task<Result<Media>> GetMediaByIdAsync(string id, CancellationToken cancellationToken);
    Task<Result<IPaginatedList<Media>>> GetAllMediaAsync(CancellationToken cancellationToken);
    Task<Result<IPaginatedList<Media>>> GetAllMediaDynamicAsync(DynamicQuery dynamicQuery, int pageIndex, int pageSize, CancellationToken cancellationToken);
    Task<Result> DeleteMediaAsync(string id, CancellationToken cancellationToken);
    Task<Result<string>> GetMediaUrlAsync(string id, CancellationToken cancellationToken);
}
