using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace GRCi.Data;

/* This is used if database provider does't define
 * IGRCiDbSchemaMigrator implementation.
 */
public class NullGRCiDbSchemaMigrator : IGRCiDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
