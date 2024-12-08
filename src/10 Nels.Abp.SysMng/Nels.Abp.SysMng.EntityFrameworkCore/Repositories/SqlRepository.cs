using Microsoft.EntityFrameworkCore;
using Nels.Abp.SysMng.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
namespace Nels.Abp.SysMng.Repositories;

public class SqlRepository(IDbContextProvider<ISysMngDbContext> dbContextProvider) : ISqlRepository, ITransientDependency
{
    private readonly IDbContextProvider<ISysMngDbContext> _dbContextProvider = dbContextProvider;

    public bool? IsChangeTrackingEnabled { get; protected set; }

    public async Task<List<TResult>> SqlQueryRaw<TResult>(string sql, params object[] parameters)
    {
        return (await _dbContextProvider.GetDbContextAsync()).Database.SqlQueryRaw<TResult>(sql, parameters).ToList();
    }
}
