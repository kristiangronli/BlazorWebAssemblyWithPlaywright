using FluentAssertions;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using XunitPlaywright.FakeAuth;

namespace XunitPlaywright;


public class PlaywrightFixture : IAsyncLifetime
{

    public IPlaywright Playwright { get; private set; }
    public Lazy<Task<IBrowser>> ChromiumBrowser { get; private set; }
    public Lazy<Task<IBrowser>> FirefoxBrowser { get; private set; }
    public Lazy<Task<IBrowser>> WebkitBrowser { get; private set; }

    public const string PlaywrightCollection = nameof(PlaywrightCollection);
    

    public async Task InitializeAsync()
    {
        var launchOptions = new BrowserTypeLaunchOptions
        {
            Headless = false
        };

        // Install Playwright and its dependencies.
        InstallPlaywright();
        // Create Playwright module.
        Playwright = await Microsoft.Playwright.Playwright.CreateAsync();
        // Setup Browser lazy initializers.
        ChromiumBrowser = new Lazy<Task<IBrowser>>(
          Playwright.Chromium.LaunchAsync(launchOptions));
        FirefoxBrowser = new Lazy<Task<IBrowser>>(
          Playwright.Firefox.LaunchAsync(launchOptions));
        WebkitBrowser = new Lazy<Task<IBrowser>>(
          Playwright.Webkit.LaunchAsync(launchOptions));

    }

    public async Task DisposeAsync()
    {
        if (Playwright != null)
        {
            if (ChromiumBrowser != null && ChromiumBrowser.IsValueCreated)
            {
                var browser = await ChromiumBrowser.Value;
                await browser.DisposeAsync();
            }
            if (FirefoxBrowser != null && FirefoxBrowser.IsValueCreated)
            {
                var browser = await FirefoxBrowser.Value;
                await browser.DisposeAsync();
            }
            if (WebkitBrowser != null && WebkitBrowser.IsValueCreated)
            {
                var browser = await WebkitBrowser.Value;
                await browser.DisposeAsync();
            }
            Playwright.Dispose();
            Playwright = null;
        }
    }



    private static void InstallPlaywright()
    {
        var exitCode = Microsoft.Playwright.Program.Main(
          new[] { "install-deps" });
        if (exitCode != 0)
        {
            throw new Exception(
              $"Playwright exited with code {exitCode} on install-deps");
        }
        exitCode = Microsoft.Playwright.Program.Main(new[] { "install" });
        if (exitCode != 0)
        {
            throw new Exception(
              $"Playwright exited with code {exitCode} on install");
        }
    }

    public async Task GotoPageAsync(
    string url,
    Func<IPage, Task> testHandler,
    Browser browserType)
    {
        // select and launch the browser.
        var browser = await SelectBrowserAsync(browserType);
        
        // Open a new page with an option to ignore HTTPS errors
        await using var context = await browser.NewContextAsync(
            new BrowserNewContextOptions
            {
                IgnoreHTTPSErrors = true
            }).ConfigureAwait(false);


        // Start tracing before creating the page.
        await context.Tracing.StartAsync(new TracingStartOptions()
        {
            Screenshots = true,
            Snapshots = true,
            Sources = true
        });

        var page = await context.NewPageAsync().ConfigureAwait(false);

        page.Should().NotBeNull();
        try
        {
            // Navigate to the given URL and wait until loading
            // network activity is done.
            var gotoResult = await page.GotoAsync(
              url,
              new PageGotoOptions
              {
                  WaitUntil = WaitUntilState.NetworkIdle
              });
            gotoResult.Should().NotBeNull();
            await gotoResult.FinishedAsync();
            gotoResult.Ok.Should().BeTrue();
            // Run the actual test logic.
            await testHandler(page);
        }
        finally
        {
            // Make sure the page is closed 
            await page.CloseAsync();

            // Stop tracing and save data into a zip archive.
            await context.Tracing.StopAsync(new TracingStopOptions()
            {
                Path = "trace.zip"
            });
        }
    }

    private Task<IBrowser> SelectBrowserAsync(Browser browser)
    {
        return browser switch
        {
            Browser.Chromium => ChromiumBrowser.Value,
            Browser.Firefox => FirefoxBrowser.Value,
            Browser.Webkit => WebkitBrowser.Value,
            _ => throw new NotImplementedException(),
        };
    }

    [CollectionDefinition(PlaywrightCollection)]
    public class PlaywrightCollectionDefinition
          : ICollectionFixture<PlaywrightFixture>
    {
        // This class is just xUnit plumbing code to apply
        // [CollectionDefinition] and the ICollectionFixture<>
        // interfaces. Witch in our case is parametrized
        // with the PlaywrightFixture.
    }
}

public enum Browser
{
    Chromium,
    Firefox,
    Webkit,
}



