using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Nels.Abp.SysMng.Repositories;

public interface ISqlRepository : IRepository
{
    Task<List<TResult>> SqlQueryRaw<TResult>(string sql, params object[] parameters);
}
