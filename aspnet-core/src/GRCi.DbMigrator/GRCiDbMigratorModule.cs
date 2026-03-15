using GRCi.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace GRCi.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(GRCiEntityFrameworkCoreModule),
    typeof(GRCiApplicationContractsModule)
    )]
public class GRCiDbMigratorModule : AbpModule
{
}
