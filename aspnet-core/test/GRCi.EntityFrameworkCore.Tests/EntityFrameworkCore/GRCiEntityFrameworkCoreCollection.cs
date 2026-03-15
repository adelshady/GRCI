using Xunit;

namespace GRCi.EntityFrameworkCore;

[CollectionDefinition(GRCiTestConsts.CollectionDefinitionName)]
public class GRCiEntityFrameworkCoreCollection : ICollectionFixture<GRCiEntityFrameworkCoreFixture>
{

}
