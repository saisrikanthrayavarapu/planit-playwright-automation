using Microsoft.Playwright;
using PlanitAutomation.Utils;

namespace PlanitAutomation.Pages;

/// <summary>
/// Base class for all page objects.  Wraps common Playwright interactions
/// (waiting, clicking, typing, visibility checks) so individual pages stay concise.
/// </summary>
public abstract class BasePage
{
    protected readonly IPage Page;

    protected BasePage(IPage page) => Page = page;

    // ── Waits ──────────────────────────────────────────────────────────────────

    protected async Task WaitForVisibleAsync(string selector, int timeoutMs = 15_000) =>
        await Page.WaitForSelectorAsync(selector, new PageWaitForSelectorOptions
        {
            State   = WaitForSelectorState.Visible,
            Timeout = timeoutMs
        });

    protected async Task WaitForHiddenAsync(string selector, int timeoutMs = 10_000) =>
        await Page.WaitForSelectorAsync(selector, new PageWaitForSelectorOptions
        {
            State   = WaitForSelectorState.Hidden,
            Timeout = timeoutMs
        });

    // ── Interactions ───────────────────────────────────────────────────────────

    protected async Task ClickAsync(string selector)
    {
        await WaitForVisibleAsync(selector);
        await Page.ClickAsync(selector);
    }

    protected async Task FillAsync(string selector, string text)
    {
        await WaitForVisibleAsync(selector);
        await Page.FillAsync(selector, text);
    }

    protected async Task<string> GetTextAsync(string selector)
    {
        await WaitForVisibleAsync(selector);
        return await Page.InnerTextAsync(selector);
    }

    // ── Visibility helpers ─────────────────────────────────────────────────────

    protected async Task<bool> IsVisibleAsync(string selector, int timeoutMs = 5_000)
    {
        try
        {
            await Page.WaitForSelectorAsync(selector, new PageWaitForSelectorOptions
            {
                State   = WaitForSelectorState.Visible,
                Timeout = timeoutMs
            });
            return true;
        }
        catch
        {
            return false;
        }
    }

    protected async Task<bool> IsHiddenAsync(string selector, int timeoutMs = 5_000)
    {
        try
        {
            await Page.WaitForSelectorAsync(selector, new PageWaitForSelectorOptions
            {
                State   = WaitForSelectorState.Hidden,
                Timeout = timeoutMs
            });
            return true;
        }
        catch
        {
            return false;
        }
    }

    // ── Price helper ───────────────────────────────────────────────────────────

    protected static double ParsePrice(string? raw) => PriceUtils.ParsePrice(raw);
}
