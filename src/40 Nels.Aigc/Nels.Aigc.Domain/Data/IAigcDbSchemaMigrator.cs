using System.Threading.Tasks;

namespace Nels.Aigc.Data;

public interface IAigcDbSchemaMigrator
{
    Task MigrateAsync();
}
