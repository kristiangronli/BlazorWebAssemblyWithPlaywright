﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.Http.Headers;
using XunitPlaywright.Setup;

namespace XunitPlaywright.ServerApiTests;

public class ServerApiApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    public string url = "https://localhost:5001";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var oldAuthentication = services.SingleOrDefault(d => d.ServiceType == typeof(JwtBearerDefaults));
            services.Remove(oldAuthentication);
            services.AddAuthentication("Test")
                .AddScheme<AuthenticationSchemeOptions, ServerApiAuthenticationHandler>("Test", AuthenticationOptions => { });            
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
          webHostBuilder => webHostBuilder.UseKestrel());
        var host = builder.Build();
        host.Start();
        // In order to cleanup and properly dispose HTTP server
        // resources we return a composite host object that is
        // actually just a way to intercept the StopAsync and Dispose
        // call and relay to our HTTP host.
        return new CompositeHost(testHost, host);
    }
}