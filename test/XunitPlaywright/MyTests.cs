using FakeAuth;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using XunitPlaywright.FakeAuth;

namespace XunitPlaywright;

//[Collection(PlaywrightFixture.PlaywrightCollection)]
[Collection("MyWebApplicationFactory Collection")]
public class MyTests
{
    private readonly PlaywrightFixture playwrightFixture;

    private readonly IndividualAuthWebApplicationFactory<AssemblyClassLocator> _factory;

    public MyTests(IndividualAuthWebApplicationFactory<AssemblyClassLocator> factory, PlaywrightFixture playwrightFixture)
    {
        this.playwrightFixture = playwrightFixture;
        _factory = factory;
    }

    [Fact]
    public async Task PlayTest()
    {
        

        using var hostFactory = new IndividualAuthWebApplicationFactory<AssemblyClassLocator>();
        hostFactory.CreateDefaultClient();

        var url = hostFactory.url;

        await this.playwrightFixture.GotoPageAsync(
          url,
          async (page) =>
          {
              await page.WaitForURLAsync($"{url}/");
              await page.GetByRole(AriaRole.Link, new() { NameString = "Counter" }).ClickAsync();

              await page.GetByText("Current count: 0").ClickAsync();

              await page.GetByRole(AriaRole.Button, new() { NameString = "Click me" }).ClickAsync();

              await page.GetByText("Current count: 1").ClickAsync();


              //Trouble in test: The following line fails with:
              await page.WaitForURLAsync($"{url}/fetchdata");

              //Verify the user is authenticated?

          }, Browser.Chromium);
    }

        [Fact]
    public async Task MyFirstTest()
    {
        var url = "https://localhost:5001";
        using var client = _factory.CreateClient();
        var response = await client.GetAsync("/WeatherForecast");
        

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        //// Create the host factory with the App class as parameter and the
        //// url we are going to use.
        //using var hostFactory = new WebTestingHostFactory<AssemblyClassLocator>();
        //hostFactory
        //  // Override host configuration to mock stuff if required.
        //  .WithWebHostBuilder(builder =>
        //  {
        //      // Setup the url to use.
        //      builder.UseUrls(url);
        //      // Replace or add services if needed.
        //      builder.ConfigureServices(services =>
        //      {
        //          // Is it possible to add a mock'ed authentication service here?
        //          //Something like:
        //          //services.AddAuthentication("Test")

        //          services.AddAuthentication().AddFakeAuth((options) =>
        //          {
        //              // Adding Claims directly to each request -- for testing / demos
        //              options.Claims.Add(new Claim(ClaimTypes.Name, ">> Fake User <<"));
        //              options.Claims.Add(new Claim(ClaimTypes.Role, "Expense_Approver"));
        //              //options.Claims.Add(new Claim("Approval_Limit", "25.00"));
        //              //options.Claims.Add(new Claim("Approval_Currency", "USD"));
        //              //options.Claims.Add(new Claim("Preffered_Location", "Disney Island"));

        //              //// adding AllowedHosts so we can test from non-localhost scenarios
        //              //options.AllowedHosts.Add("my-container-host");
        //              //options.AllowedHosts.Add("my-ci-qa-server");
        //              //options.AllowedHosts.Add(FakeAuthOptions.DefaultAllowedHost); // localhost
        //          });
        //      })
        //      // Replace or add configuration if needed.
        //      .ConfigureAppConfiguration((app, conf) =>
        //      {
        //          // conf.AddJsonFile("appsettings.Test.json");
        //          //add a mock'ed user here?
        //      });
        //  })
        //  // Create the host using the CreateDefaultClient method.
        //  .CreateDefaultClient();

        await this.playwrightFixture.GotoPageAsync(
          url,
          async (page) =>
          {
              await page.WaitForURLAsync($"{url}/");
              await page.GetByRole(AriaRole.Link, new() { NameString = "Counter" }).ClickAsync();

              await page.GetByText("Current count: 0").ClickAsync();

              await page.GetByRole(AriaRole.Button, new() { NameString = "Click me" }).ClickAsync();

              await page.GetByText("Current count: 1").ClickAsync();


              //Trouble in test: The following line fails with:
              await page.WaitForURLAsync($"{url}/fetchdata");

              //Verify the user is authenticated?

          }, Browser.Chromium);
    }

}
