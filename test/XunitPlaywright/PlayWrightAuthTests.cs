using Microsoft.Playwright;
using System.Net;
using XunitPlaywright.Auth;
using XunitPlaywright.Setup;

namespace XunitPlaywright;

//[Collection(PlaywrightFixture.PlaywrightCollection)]
[Collection("AuthApplicationFactory Collection")]
public class PlayWrightAuthTests
{
    private readonly PlaywrightFixture playwrightFixture;

    private readonly IndividualAuthWebApplicationFactory<AssemblyClassLocator> _factory;

    public PlayWrightAuthTests(IndividualAuthWebApplicationFactory<AssemblyClassLocator> factory, PlaywrightFixture playwrightFixture)
    {
        this.playwrightFixture = playwrightFixture;
        _factory = factory;
    }

   
    [Fact]
    public async Task MyFirstAuthenticatedTest()
    {
        var url = "https://localhost:5001";
        using var client = _factory.CreateClient();
        var response = await client.GetAsync("/WeatherForecast");
        

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

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

              //ToDo: Login
              //ToDo: Verify the user is authenticated?

          }, Browser.Chromium);
    }

}
