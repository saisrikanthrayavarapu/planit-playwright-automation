using Microsoft.Playwright;

namespace PlanitAutomation.Pages;

/// <summary>
/// Represents the Jupiter Toys contact page.
/// Provides methods to fill in the form, submit it, and inspect validation/success messages.
/// </summary>
public class ContactPage(IPage page) : BasePage(page)
{
    private const string FieldForename = "#forename";
    private const string FieldEmail    = "#email";
    private const string FieldMessage  = "#message";
    private const string BtnSubmit     = "a.btn-contact, .btn-contact";
    private const string ErrForename   = "#forename-err";
    private const string ErrEmail      = "#email-err";
    private const string ErrMessage    = "#message-err";
    private const string SuccessAlert  = ".alert-success";

    // ── Form actions ───────────────────────────────────────────────────────────

    public async Task ClickSubmitAsync()         => await ClickAsync(BtnSubmit);
    public async Task FillForenameAsync(string v) => await FillAsync(FieldForename, v);
    public async Task FillEmailAsync(string v)    => await FillAsync(FieldEmail, v);
    public async Task FillMessageAsync(string v)  => await FillAsync(FieldMessage, v);

    // ── Validation-error visibility ────────────────────────────────────────────

    public Task<bool> IsForenameErrorVisibleAsync() => IsVisibleAsync(ErrForename);
    public Task<bool> IsEmailErrorVisibleAsync()    => IsVisibleAsync(ErrEmail);
    public Task<bool> IsMessageErrorVisibleAsync()  => IsVisibleAsync(ErrMessage);

    public Task<bool> IsForenameErrorHiddenAsync() => IsHiddenAsync(ErrForename);
    public Task<bool> IsEmailErrorHiddenAsync()    => IsHiddenAsync(ErrEmail);
    public Task<bool> IsMessageErrorHiddenAsync()  => IsHiddenAsync(ErrMessage);

    // ── Validation-error text ──────────────────────────────────────────────────

    public Task<string> GetForenameErrorTextAsync() => GetTextAsync(ErrForename);
    public Task<string> GetEmailErrorTextAsync()    => GetTextAsync(ErrEmail);
    public Task<string> GetMessageErrorTextAsync()  => GetTextAsync(ErrMessage);

    // ── Success banner ─────────────────────────────────────────────────────────

    /// <summary>
    /// Waits up to 30 s for the success banner to appear
    /// (Jupiter Toys shows a spinner before the banner).
    /// </summary>
    public Task<bool> IsSuccessMessageVisibleAsync() =>
        IsVisibleAsync(SuccessAlert, timeoutMs: 30_000);

    public async Task<string> GetSuccessMessageTextAsync()
    {
        await WaitForVisibleAsync(SuccessAlert, timeoutMs: 30_000);
        return await GetTextAsync(SuccessAlert);
    }
}
