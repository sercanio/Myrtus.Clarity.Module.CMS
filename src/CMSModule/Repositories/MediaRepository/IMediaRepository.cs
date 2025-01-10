using CMSModule.Models;
using Myrtus.Clarity.Application.Repositories.NoSQL;

namespace CMSModule.Repositories.MediaRepository;

public interface IMediaRepository : INoSqlRepository<Media>
{
}
