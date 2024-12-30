using CMSModule.Models;

namespace CMSModule.Repositories.ContentRepository;

public interface IContentRepository
{
    Task<Content> GetByIdAsync(string id);
    Task<Content> GetBySlugAsync(string slug);
    Task<IEnumerable<Content>> GetAllAsync();
    Task<IEnumerable<Content>> QueryAsync(ContentQueryParameters parameters);
    Task CreateAsync(Content content);
    Task UpdateAsync(Content content);
    Task DeleteAsync(string id);
}
