using CMSModule.Models;
using Myrtus.Clarity.Application.Repositories.NoSQL;

namespace CMSModule.Repositories.SeoRepository;

public interface ISeoRepository: INoSqlRepository<SEOSettings>
{
}