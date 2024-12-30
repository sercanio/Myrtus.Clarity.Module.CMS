using CMSModule.Models;

namespace CMSModule.Repositories.MediaRepository;

public interface IMediaRepository
{
    Task<Media> GetByIdAsync(string id);
    Task<IEnumerable<Media>> GetAllAsync();
    Task CreateAsync(Media media);
    Task DeleteAsync(string id);
}
