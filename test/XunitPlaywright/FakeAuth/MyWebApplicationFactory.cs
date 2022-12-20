using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;

namespace XunitPlaywright.FakeAuth;

public class MyWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
   

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var oldAuthentication = services.SingleOrDefault(d => d.ServiceType == typeof(JwtBearerDefaults));

            services.Remove(oldAuthentication);


            services.AddAuthentication("Test")
                .AddScheme<AuthenticationSchemeOptions, TestAuthenticationHandler>("Test", AuthenticationOptions => { });

            
        });
    }

    protected override void ConfigureClient(HttpClient client)
    {
        client.BaseAddress = new Uri("https://localhost:5001");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test", "test");
    }
}
