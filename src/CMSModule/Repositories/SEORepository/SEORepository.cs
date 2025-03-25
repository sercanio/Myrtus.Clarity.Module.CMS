using CMSModule.Models;
using MongoDB.Driver;
using AppTemplate.Infrastructure.Repositories.NoSQL;

namespace CMSModule.Repositories.SeoRepository;

public class SeoRepository : NoSqlRepository<SEOSettings>, ISeoRepository
{
    public SeoRepository(IMongoDatabase database)
        : base(database, "SEO")
    {
    }
}