using PlanitAutomation.Utils;
using Reqnroll;

namespace PlanitAutomation.StepDefinitions;

/// <summary>
/// Reqnroll Before/After scenario hooks.
/// Creates a <see cref="BrowserManager"/> before each scenario and disposes it afterwards.
/// On failure: captures a full-page screenshot and logs the error details to file.
/// </summary>
[Binding]
public sealed class Hooks(ScenarioContext scenarioContext)
{
    [BeforeScenario]
    public async Task SetUpAsync()
    {
        TestLogger.Info($"▶ Starting scenario: '{scenarioContext.ScenarioInfo.Title}'");

        // Run headless in CI environments (e.g., GitHub Actions) to avoid XServer issues
        bool isCi = Environment.GetEnvironmentVariable("GHAC") != null;
        var browserManager = new BrowserManager();
        await browserManager.InitAsync(headless: isCi);
        scenarioContext.Set(browserManager);
    }

    [AfterScenario]
    public async Task TearDownAsync()
    {
        var title = scenarioContext.ScenarioInfo.Title;

        if (scenarioContext.TryGetValue(out BrowserManager browserManager))
        {
            // ── On failure: screenshot + error log ──────────────────────────
            if (scenarioContext.ScenarioExecutionStatus == ScenarioExecutionStatus.TestError)
            {
                try
                {
                    var screenshotPath = await browserManager.TakeScreenshotAsync(title);
                    TestLogger.Error(
                        $"✖ Scenario FAILED: '{title}'" +
                        $"{Environment.NewLine}  Screenshot saved → {screenshotPath}");

                    if (scenarioContext.TestError is { } ex)
                        TestLogger.Error("  Exception details:", ex);
                }
                catch (Exception screenshotEx)
                {
                    TestLogger.Error("  Could not capture failure screenshot.", screenshotEx);
                }
            }
            else
            {
                TestLogger.Info($"✔ Scenario PASSED: '{title}'");
            }

            await browserManager.DisposeAsync();
        }
    }
}
