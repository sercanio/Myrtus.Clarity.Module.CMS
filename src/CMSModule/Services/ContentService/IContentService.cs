using Ardalis.Result;
using CMSModule.Models;

namespace CMSModule.Services.ContentService;

public interface IContentService
{
    Task<Result<Content>> GetContentByIdAsync(string id);
    Task<Result<Content>> GetContentBySlugAsync(string slug);
    Task<Result<IEnumerable<Content>>> GetAllContentsAsync();
    Task<Result<IEnumerable<Content>>> QueryContentsAsync(ContentQueryParameters parameters);
    Task<Result<Content>> CreateContentAsync(Content content, string createdBy);
    Task<Result> UpdateContentAsync(Content content, string modifiedBy);
    Task<Result> DeleteContentAsync(string id);
    Task<Result> RestoreContentVersionAsync(string id, int versionNumber);
}
