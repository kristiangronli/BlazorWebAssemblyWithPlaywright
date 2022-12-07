using Microsoft.AspNetCore.Hosting;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XunitPlaywright;

[Collection(PlaywrightFixture.PlaywrightCollection)]
public class MyTests
{
    private readonly PlaywrightFixture playwrightFixture;

    public MyTests(PlaywrightFixture playwrightFixture)
    {
        this.playwrightFixture = playwrightFixture;
    }

    [Fact]
    public async Task MyFirstTest()
    {
        var url = "https://localhost:5001";

        // Create the host factory with the App class as parameter and the
        // url we are going to use.
        using var hostFactory = new WebTestingHostFactory<AssemblyClassLocator>();
        hostFactory
          // Override host configuration to mock stuff if required.
          .WithWebHostBuilder(builder =>
          {
              // Setup the url to use.
              builder.UseUrls(url);
              // Replace or add services if needed.
              builder.ConfigureServices(services =>
              {
                  // services.AddTransient<....>();
              })
              // Replace or add configuration if needed.
              .ConfigureAppConfiguration((app, conf) =>
              {
                  // conf.AddJsonFile("appsettings.Test.json");
              });
          })
          // Create the host using the CreateDefaultClient method.
          .CreateDefaultClient();

        await this.playwrightFixture.GotoPageAsync(
          url,
          async (page) =>
          {
              await page.WaitForURLAsync($"{url}/");
              await page.GetByRole(AriaRole.Link, new() { NameString = "Counter" }).ClickAsync();

              await page.GetByText("Current count: 0").ClickAsync();

              await page.GetByRole(AriaRole.Button, new() { NameString = "Click me" }).ClickAsync();

              await page.GetByText("Current count: 1").ClickAsync();
          }, Browser.Chromium);
    }
    
}
