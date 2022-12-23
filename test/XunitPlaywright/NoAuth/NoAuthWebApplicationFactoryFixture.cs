using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XunitPlaywright.Setup;

namespace XunitPlaywright.NoAuth;

[CollectionDefinition("NoAuthApplicationFactory Collection")]
public class NoAuthWebApplicationFactoryFixture : ICollectionFixture<NoAuthWebApplicationFactory<AssemblyClassLocator>>, ICollectionFixture<PlaywrightFixture>
{
}
