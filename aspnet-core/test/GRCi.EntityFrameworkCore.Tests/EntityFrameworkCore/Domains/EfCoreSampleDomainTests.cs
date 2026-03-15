using GRCi.Samples;
using Xunit;

namespace GRCi.EntityFrameworkCore.Domains;

[Collection(GRCiTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<GRCiEntityFrameworkCoreTestModule>
{

}
