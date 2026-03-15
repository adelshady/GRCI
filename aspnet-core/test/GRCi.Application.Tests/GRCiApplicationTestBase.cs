using Volo.Abp.Modularity;

namespace GRCi;

public abstract class GRCiApplicationTestBase<TStartupModule> : GRCiTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
