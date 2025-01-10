using CMSModule.Models;
using Myrtus.Clarity.Application.Repositories.NoSQL;

namespace CMSModule.Repositories.ContentRepository;

public interface IContentRepository : INoSqlRepository<Content>
{
    Task<bool> CheckIfContentExistsBySlugAsync(string slug, CancellationToken cancellationToken);
}
