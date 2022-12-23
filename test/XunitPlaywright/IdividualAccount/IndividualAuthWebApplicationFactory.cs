using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.Http.Headers;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;

namespace XunitPlaywright.IdividualAccount;

public class IndividualAuthWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    public string url = "https://localhost:5001";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        //var connectionString = "Server=(localdb)\\mssqllocaldb;Database=aspnet-IndividualAccount.Server-6763a243-49aa-4cfc-9a9d-376c9b5908b5;Trusted_Connection=True;MultipleActiveResultSets=true";
        //ToDo: Update to inMemory database

        

        builder.ConfigureServices(services =>
        {
            var oldAuthentication = services.SingleOrDefault(d => d.ServiceType == typeof(JwtBearerDefaults));
            services.Remove(oldAuthentication);

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

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

    // Relay the call to both test host and kestrel host.
    public class CompositeHost : IHost
    {
        private readonly IHost testHost;
        private readonly IHost kestrelHost;
        public CompositeHost(IHost testHost, IHost kestrelHost)
        {
            this.testHost = testHost;
            this.kestrelHost = kestrelHost;
        }
        public IServiceProvider Services => this.testHost.Services;
        public void Dispose()
        {
            this.testHost.Dispose();
            this.kestrelHost.Dispose();
        }
        public async Task StartAsync(
          CancellationToken cancellationToken = default)
        {
            await this.testHost.StartAsync(cancellationToken);
            await this.kestrelHost.StartAsync(cancellationToken);
        }
        public async Task StopAsync(
          CancellationToken cancellationToken = default)
        {
            await this.testHost.StopAsync(cancellationToken);
            await this.kestrelHost.StopAsync(cancellationToken);
        }
    }
}
