using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using XunitPlaywright.ServerApiTests;

namespace XunitPlaywright;

[Collection("ServerApiApplicationFactory Collection")]
public class ServerApiTest
{
    private readonly ServerApiApplicationFactory<AssemblyClassLocator> _factory;

    public ServerApiTest(ServerApiApplicationFactory<AssemblyClassLocator> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task TestApi()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/WeatherForecast");


        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseContent = await response.Content.ReadAsStringAsync();

        Assert.Contains("temperatureC", responseContent);
    }
}
