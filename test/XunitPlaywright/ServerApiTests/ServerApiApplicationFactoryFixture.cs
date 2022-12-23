using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XunitPlaywright.ServerApiTests;

[CollectionDefinition("ServerApiApplicationFactory Collection")]
public class ServerApiApplicationFactoryFixture : ICollectionFixture<ServerApiApplicationFactory<AssemblyClassLocator>>
{
}
