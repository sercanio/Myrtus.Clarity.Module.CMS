using Ardalis.Result;
using CMSModule.Models;
using Myrtus.Clarity.Core.Application.Abstractions.Pagination;
using Myrtus.Clarity.Core.Infrastructure.Dynamic;

namespace CMSModule.Services.ContentService;

public interface IContentService
{
    Task<Result<bool>> CheckIfContentExistsBySlugAsync(string slug, CancellationToken cancellationToken);
    Task<Result<Content>> GetContentByIdAsync(string id, CancellationToken cancellationToken);
    Task<Result<Content>> GetContentBySlugAsync(string slug, CancellationToken cancellationToken);
    Task<Result<IPaginatedList<Content>>> GetAllContentsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken);
    Task<Result<IPaginatedList<Content>>> GetAllContentsDynamicAsync(DynamicQuery dynamicQuery, int pageIndex, int pageSize, CancellationToken cancellationToken);
    Task<Result<Content>> CreateContentAsync(Content content, string createdBy, CancellationToken cancellationToken);
    Task<Result> UpdateContentAsync(Content content, string modifiedBy, CancellationToken cancellationToken);
    Task<Result> DeleteContentAsync(string id, CancellationToken cancellationToken);
    Task<Result> RestoreContentVersionAsync(string id, int versionNumber, CancellationToken cancellationToken);
}
