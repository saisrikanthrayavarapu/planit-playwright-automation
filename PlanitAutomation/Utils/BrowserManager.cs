using Microsoft.Playwright;

namespace PlanitAutomation.Utils;

/// <summary>
/// Manages creation and disposal of a Playwright browser/page instance per async-local context
/// (one instance per test scenario when tests run sequentially).
/// </summary>
public sealed class BrowserManager : IAsyncDisposable
{
    private IPlaywright? _playwright;
    private IBrowser?    _browser;
    private IPage?       _page;

    public const string BaseUrl = "http://jupiter.cloud.planittesting.com";

    /// <summary>The active Playwright <see cref="IPage"/> for the current scenario.</summary>
    public IPage Page => _page ?? throw new InvalidOperationException(
        "BrowserManager has not been initialised. Call InitAsync() first.");

    /// <summary>Launches Chromium and navigates to the base URL.</summary>
    public async Task InitAsync(bool headless = false)
    {
        _playwright = await Playwright.CreateAsync();
        _browser    = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = headless,
            Args     = new[] { "--no-sandbox", "--disable-dev-shm-usage" }
        });

        var context = await _browser.NewContextAsync(new BrowserNewContextOptions
        {
            ViewportSize = new ViewportSize { Width = 1920, Height = 1080 }
        });

        _page = await context.NewPageAsync();
        await _page.GotoAsync(BaseUrl);
    }

    /// <summary>
    /// Captures a PNG screenshot and saves it to TestResults/Screenshots/.
    /// Returns the full path of the saved file.
    /// </summary>
    public async Task<string> TakeScreenshotAsync(string scenarioTitle)
    {
        var screenshotDir = Path.Combine(AppContext.BaseDirectory, "TestResults", "Screenshots");
        Directory.CreateDirectory(screenshotDir);

        // Sanitise the scenario title for use as a filename
        var safeName = string.Concat(scenarioTitle.Split(Path.GetInvalidFileNameChars()));
        var fileName  = $"{safeName}_{DateTime.Now:yyyyMMdd_HHmmss}.png";
        var filePath  = Path.Combine(screenshotDir, fileName);

        if (_page is not null)
            await _page.ScreenshotAsync(new PageScreenshotOptions { Path = filePath, FullPage = true });

        return filePath;
    }

    /// <summary>Closes the browser and releases Playwright resources.</summary>
    public async ValueTask DisposeAsync()
    {
        if (_browser is not null)
            await _browser.CloseAsync();

        _playwright?.Dispose();
    }
}
