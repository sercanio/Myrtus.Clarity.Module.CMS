using CMSModule.Models;
using AppTemplate.Application.Repositories.NoSQL;

namespace CMSModule.Repositories.ContentRepository;

public interface IContentRepository : INoSqlRepository<Content>
{
    Task<bool> CheckIfContentExistsBySlugAsync(string slug, CancellationToken cancellationToken);
}
