using FluentAssertions;
using PlanitAutomation.Pages;
using PlanitAutomation.Utils;
using Reqnroll;

namespace PlanitAutomation.StepDefinitions;

/// <summary>
/// Step definitions for Cart scenario (TC3).
/// </summary>
[Binding]
public sealed class CartSteps(ScenarioContext scenarioContext)
{
    // ── Page-object references ─────────────────────────────────────────────────

    private BrowserManager BrowserManager =>
        scenarioContext.Get<BrowserManager>();

    private HomePage?  _homePage;
    private ShopPage?  _shopPage;
    private CartPage?  _cartPage;

    // ── Navigation ─────────────────────────────────────────────────────────────

    [When("I navigate to the shop page")]
    public async Task INavigateToTheShopPage()
    {
        _homePage ??= new HomePage(BrowserManager.Page);
        _shopPage   = await _homePage.GoToShopAsync();
    }

    [When("I navigate to the cart page")]
    public async Task INavigateToTheCartPage()
    {
        _shopPage ??= new ShopPage(BrowserManager.Page);
        _cartPage   = await _shopPage.GoToCartAsync();
    }

    // ── Cart actions ───────────────────────────────────────────────────────────

    [When(@"I add (\d+) of ""(.*)"" to the cart")]
    public async Task IAddOfToTheCart(int quantity, string productName)
    {
        _shopPage ??= new ShopPage(BrowserManager.Page);
        await _shopPage.AddToCartAsync(productName, quantity);
    }

    // ── Cart assertions ────────────────────────────────────────────────────────

    [Then(@"the subtotal for ""(.*)"" should equal unit price multiplied by (\d+)")]
    public async Task TheSubtotalForShouldEqualUnitPriceMultipliedBy(string productName, int multiplier)
    {
        _cartPage ??= new CartPage(BrowserManager.Page);

        var unitPrice        = await _cartPage.GetUnitPriceAsync(productName);
        var expectedSubtotal = PriceUtils.Round2Dp(unitPrice * multiplier);
        var actualSubtotal   = PriceUtils.Round2Dp(await _cartPage.GetSubtotalAsync(productName));

        actualSubtotal.Should().Be(expectedSubtotal,
            $"subtotal for '{productName}' should be {expectedSubtotal:F2} " +
            $"(unit price {unitPrice:F2} × {multiplier})");
    }

    [Then("the grand total should equal the sum of all subtotals")]
    public async Task TheGrandTotalShouldEqualTheSumOfAllSubtotals()
    {
        _cartPage ??= new CartPage(BrowserManager.Page);

        var rows         = await _cartPage.GetAllRowsAsync();
        var computedSum  = rows.Sum(r => r.Subtotal);
        var grandTotal   = await _cartPage.GetGrandTotalAsync();

        PriceUtils.Round2Dp(grandTotal).Should().Be(
            PriceUtils.Round2Dp(computedSum),
            $"grand total {grandTotal:F2} should equal sum of subtotals {computedSum:F2}");
    }
}
