using Volo.Abp.Modularity;

namespace GRCi;

/* Inherit from this class for your domain layer tests. */
public abstract class GRCiDomainTestBase<TStartupModule> : GRCiTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
