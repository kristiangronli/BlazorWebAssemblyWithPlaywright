using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XunitPlaywright.Auth;
using XunitPlaywright.NoAuth;
using XunitPlaywright.Setup;

namespace XunitPlaywright;

[Collection("NoAuthApplicationFactory Collection")]
public class PlayWrightNoAuthTests
{
    private readonly PlaywrightFixture playwrightFixture;
    private readonly NoAuthWebApplicationFactory<AssemblyClassLocator> _factory;

    public PlayWrightNoAuthTests(NoAuthWebApplicationFactory<AssemblyClassLocator> factory, PlaywrightFixture playwrightFixture)
    {
        this.playwrightFixture = playwrightFixture;
        _factory = factory;
    }


    
    [Fact]
    public async Task NotAuthenticatedTest()
    {
        using var hostFactory = new NoAuthWebApplicationFactory<AssemblyClassLocator>();
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

          }, Browser.Chromium);
    }
}
