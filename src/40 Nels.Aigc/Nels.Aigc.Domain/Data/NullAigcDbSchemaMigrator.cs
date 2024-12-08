using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Nels.Aigc.Data;

/* This is used if database provider does't define
 * IAigcDbSchemaMigrator implementation.
 */
public class NullAigcDbSchemaMigrator : IAigcDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
