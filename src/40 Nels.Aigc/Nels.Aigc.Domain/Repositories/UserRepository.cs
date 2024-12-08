using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Nels.Aigc.Repositories;

public interface IAigcRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
{
}
