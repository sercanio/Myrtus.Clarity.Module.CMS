using CMSModule.Models;
using AppTemplate.Application.Repositories.NoSQL;

namespace CMSModule.Repositories.MediaRepository;

public interface IMediaRepository : INoSqlRepository<Media>
{
}
