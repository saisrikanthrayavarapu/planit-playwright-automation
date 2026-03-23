using Microsoft.Playwright;

namespace PlanitAutomation.Pages;

/// <summary>
/// Represents the Jupiter Toys shop page.
/// Allows adding products to the cart and navigating to the cart.
/// </summary>
public class ShopPage(IPage page) : BasePage(page)
{
    private const string CartLink = "a[href*='cart']";

    /// <summary>
    /// Returns an XPath selector for the Buy button of the given product.
    /// </summary>
    private static string BuyButtonSelector(string productName) =>
        $"//li[contains(@class,'product') or contains(@class,'product-wrap')]" +
        $"[.//*[contains(text(),'{productName}')]]" +
        $"//a[contains(@class,'btn-success') or contains(text(),'Buy')]";

    /// <summary>Clicks the Buy button for <paramref name="productName"/> <paramref name="quantity"/> times.</summary>
    public async Task AddToCartAsync(string productName, int quantity)
    {
        var selector = BuyButtonSelector(productName);
        for (var i = 0; i < quantity; i++)
            await ClickAsync(selector);
    }

    public async Task<CartPage> GoToCartAsync()
    {
        await ClickAsync(CartLink);
        return new CartPage(Page);
    }
}
