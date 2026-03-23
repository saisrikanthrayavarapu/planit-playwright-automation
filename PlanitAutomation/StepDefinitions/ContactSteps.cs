using FluentAssertions;
using PlanitAutomation.Pages;
using PlanitAutomation.Utils;
using Reqnroll;

namespace PlanitAutomation.StepDefinitions;

/// <summary>
/// Step definitions for Contact-form scenarios (TC1 and TC2).
/// </summary>
[Binding]
public sealed class ContactSteps(ScenarioContext scenarioContext)
{
    // ── Page-object references ─────────────────────────────────────────────────

    private BrowserManager BrowserManager =>
        scenarioContext.Get<BrowserManager>();

    private HomePage? _homePage;
    private ContactPage? _contactPage;

    // ── Shared Given step (also used by CartSteps) ─────────────────────────────

    [Given("I am on the home page")]
    public void IAmOnTheHomePage()
    {
        // The browser is already opened and navigated to BaseUrl by Hooks.SetUpAsync().
        _homePage = new HomePage(BrowserManager.Page);
    }

    // ── Navigation ─────────────────────────────────────────────────────────────

    [When("I navigate to the contact page")]
    public async Task INavigateToTheContactPage()
    {
        _homePage ??= new HomePage(BrowserManager.Page);
        _contactPage = await _homePage.GoToContactAsync();
    }

    // ── Form interactions ──────────────────────────────────────────────────────

    [When("I click the submit button")]
    public async Task IClickTheSubmitButton() =>
        await (_contactPage ??= new ContactPage(BrowserManager.Page)).ClickSubmitAsync();

    [When("I fill in forename {string}")]
    public async Task IFillInForename(string value) =>
        await (_contactPage ??= new ContactPage(BrowserManager.Page)).FillForenameAsync(value);

    [When("I fill in email {string}")]
    public async Task IFillInEmail(string value) =>
        await (_contactPage ??= new ContactPage(BrowserManager.Page)).FillEmailAsync(value);

    [When("I fill in message {string}")]
    public async Task IFillInMessage(string value) =>
        await (_contactPage ??= new ContactPage(BrowserManager.Page)).FillMessageAsync(value);

    // ── Validation-error assertions ────────────────────────────────────────────

    [Then("I should see forename validation error {string}")]
    public async Task IShouldSeeForenameValidationError(string expectedText)
    {
        var page = _contactPage ??= new ContactPage(BrowserManager.Page);
        (await page.IsForenameErrorVisibleAsync())
            .Should().BeTrue("forename error should be visible");
        (await page.GetForenameErrorTextAsync())
            .Should().Contain(expectedText);
    }

    [Then("I should see email validation error {string}")]
    public async Task IShouldSeeEmailValidationError(string expectedText)
    {
        var page = _contactPage ??= new ContactPage(BrowserManager.Page);
        (await page.IsEmailErrorVisibleAsync())
            .Should().BeTrue("email error should be visible");
        (await page.GetEmailErrorTextAsync())
            .Should().Contain(expectedText);
    }

    [Then("I should see message validation error {string}")]
    public async Task IShouldSeeMessageValidationError(string expectedText)
    {
        var page = _contactPage ??= new ContactPage(BrowserManager.Page);
        (await page.IsMessageErrorVisibleAsync())
            .Should().BeTrue("message error should be visible");
        (await page.GetMessageErrorTextAsync())
            .Should().Contain(expectedText);
    }

    [Then("the forename validation error should not be visible")]
    public async Task TheForenameValidationErrorShouldNotBeVisible() =>
        (await (_contactPage ??= new ContactPage(BrowserManager.Page)).IsForenameErrorHiddenAsync())
            .Should().BeTrue("forename error should be hidden after filling in the field");

    [Then("the email validation error should not be visible")]
    public async Task TheEmailValidationErrorShouldNotBeVisible() =>
        (await (_contactPage ??= new ContactPage(BrowserManager.Page)).IsEmailErrorHiddenAsync())
            .Should().BeTrue("email error should be hidden after filling in the field");

    [Then("the message validation error should not be visible")]
    public async Task TheMessageValidationErrorShouldNotBeVisible() =>
        (await (_contactPage ??= new ContactPage(BrowserManager.Page)).IsMessageErrorHiddenAsync())
            .Should().BeTrue("message error should be hidden after filling in the field");

    // ── Success-banner assertion ───────────────────────────────────────────────

    [Then("I should see a success confirmation message containing {string}")]
    public async Task IShouldSeeASuccessConfirmationMessageContaining(string expectedText)
    {
        var page = _contactPage ??= new ContactPage(BrowserManager.Page);
        (await page.IsSuccessMessageVisibleAsync())
            .Should().BeTrue("success message should be visible");
        (await page.GetSuccessMessageTextAsync())
            .Should().Contain(expectedText);
    }
}
