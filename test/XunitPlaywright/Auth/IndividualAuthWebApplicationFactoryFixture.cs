using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XunitPlaywright.Setup;

namespace XunitPlaywright.Auth;


[CollectionDefinition("AuthApplicationFactory Collection")]
public class IndividualAuthWebApplicationFactoryFixture : ICollectionFixture<IndividualAuthWebApplicationFactory<AssemblyClassLocator>>, ICollectionFixture<PlaywrightFixture>
{
}