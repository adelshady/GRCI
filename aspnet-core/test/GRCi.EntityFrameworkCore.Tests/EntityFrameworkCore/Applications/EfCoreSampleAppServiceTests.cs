using GRCi.Samples;
using Xunit;

namespace GRCi.EntityFrameworkCore.Applications;

[Collection(GRCiTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<GRCiEntityFrameworkCoreTestModule>
{

}
