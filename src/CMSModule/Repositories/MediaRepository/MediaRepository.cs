using CMSModule.Models;
using MongoDB.Driver;
using AppTemplate.Infrastructure.Repositories.NoSQL;

namespace CMSModule.Repositories.MediaRepository;

public class MediaRepository : NoSqlRepository<Media>, IMediaRepository
{
    public MediaRepository(IMongoDatabase database)
        : base(database, "Media")
    {
    }
}
