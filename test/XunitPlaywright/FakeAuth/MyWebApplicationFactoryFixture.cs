using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XunitPlaywright.FakeAuth;

[CollectionDefinition("MyWebApplicationFactory Collection")]
public class MyWebApplicationFactoryFixture : ICollectionFixture<MyWebApplicationFactory<AssemblyClassLocator>>
{
}
