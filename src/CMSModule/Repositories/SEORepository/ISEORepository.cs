using CMSModule.Models;
using AppTemplate.Application.Repositories.NoSQL;

namespace CMSModule.Repositories.SeoRepository;

public interface ISeoRepository: INoSqlRepository<SEOSettings>
{
}