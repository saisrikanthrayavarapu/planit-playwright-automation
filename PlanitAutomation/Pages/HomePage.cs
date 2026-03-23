using Microsoft.Playwright;

namespace PlanitAutomation.Pages;

/// <summary>
/// Represents the Jupiter Toys home page.
/// Provides navigation helpers to other pages.
/// </summary>
public class HomePage(IPage page) : BasePage(page)
{
    private const string NavContact = "a:text('Contact')";
    private const string NavShop    = "a:text('Shop')";

    public async Task<ContactPage> GoToContactAsync()
    {
        await ClickAsync(NavContact);
        return new ContactPage(Page);
    }

    public async Task<ShopPage> GoToShopAsync()
    {
        await ClickAsync(NavShop);
        return new ShopPage(Page);
    }
}
