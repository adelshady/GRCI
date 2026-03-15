using Volo.Abp.Modularity;

namespace GRCi;

[DependsOn(
    typeof(GRCiDomainModule),
    typeof(GRCiTestBaseModule)
)]
public class GRCiDomainTestModule : AbpModule
{

}
