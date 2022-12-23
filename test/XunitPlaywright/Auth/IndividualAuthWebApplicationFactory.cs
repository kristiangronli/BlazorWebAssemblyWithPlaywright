using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.Http.Headers;
using XunitPlaywright.Setup;

namespace XunitPlaywright.Auth;

public class IndividualAuthWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    public string url = "https://localhost:5001";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        //var connectionString = "Server=(localdb)\\mssqllocaldb;Database=aspnet-IndividualAccount.Server-6763a243-49aa-4cfc-9a9d-376c9b5908b5;Trusted_Connection=True;MultipleActiveResultSets=true";
        

        

        builder.ConfigureServices(services =>
        {
            var oldAuthentication = services.SingleOrDefault(d => d.ServiceType == typeof(JwtBearerDefaults));
            services.Remove(oldAuthentication);

            //services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

            //services.AddDatabaseDeveloperPageExceptionFilter();

            //services.AddIdentityServer().AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

            //services.AddAuthentication().AddIdentityServerJwt();

            services.AddIdentityServer()
            //.AddInMemoryApiScopes(Config.ApiScopes)
            .AddInMemoryApiResources(InMemAuthConfig.Apis)
            .AddInMemoryClients(InMemAuthConfig.Clients)
            //.AddInMemoryIdentityResources(Config.IdentityResources)
            .AddTestUsers(InMemAuthConfig.Users);
            //.AddDeveloperSigningCredential();

        });
        builder.UseUrls(url);
    }

    protected override void ConfigureClient(HttpClient client)
    {
        client.BaseAddress = new Uri("https://localhost:5001");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test", "test");
    }



    protected override IHost CreateHost(IHostBuilder builder)
    {
        // Create the host that is actually used by the
        // TestServer (In Memory).
        var testHost = base.CreateHost(builder);
        // configure and start the actual host using Kestrel.
        builder.ConfigureWebHost(
          webHostBuilder => { 
              webHostBuilder.UseKestrel();
          });
        var host = builder.Build();
        
        //host.UseWebAssemblyDebugging();
        //host.UseIdentityServer();
        host.Start();
        // In order to cleanup and properly dispose HTTP server
        // resources we return a composite host object that is
        // actually just a way to intercept the StopAsync and Dispose
        // call and relay to our HTTP host.
        return new CompositeHost(testHost, host);
    }
}
