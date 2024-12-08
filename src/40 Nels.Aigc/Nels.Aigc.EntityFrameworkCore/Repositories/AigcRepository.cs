using Nels.Aigc.EntityFrameworkCore;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Nels.Aigc.Repositories;

public class AigcRepository<TEntity>(IDbContextProvider<AigcDbContext> dbContextProvider) : EfCoreRepository<AigcDbContext, TEntity>(dbContextProvider), IAigcRepository<TEntity> where TEntity : class, IEntity
{
}
