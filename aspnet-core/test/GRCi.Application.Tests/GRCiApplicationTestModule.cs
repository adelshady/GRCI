using Volo.Abp.Modularity;

namespace GRCi;

[DependsOn(
    typeof(GRCiApplicationModule),
    typeof(GRCiDomainTestModule)
)]
public class GRCiApplicationTestModule : AbpModule
{

}
