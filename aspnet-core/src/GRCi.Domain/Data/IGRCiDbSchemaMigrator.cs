using System.Threading.Tasks;

namespace GRCi.Data;

public interface IGRCiDbSchemaMigrator
{
    Task MigrateAsync();
}
