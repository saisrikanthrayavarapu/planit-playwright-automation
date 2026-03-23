using Microsoft.Playwright;

namespace PlanitAutomation.Pages;

/// <summary>
/// Represents the Jupiter Toys cart page.
/// Provides helpers to read unit prices, subtotals and the grand total.
/// </summary>
public class CartPage(IPage page) : BasePage(page)
{
    private const string CartRows  = "table tbody tr";
    private const string CartTotal = ".total > strong, tfoot .total, tr.total td:last-child";

    // ── Inner record ───────────────────────────────────────────────────────────

    /// <summary>Represents a single row in the cart table.</summary>
    public record CartRow(string Name, double UnitPrice, int Quantity, double Subtotal);

    // ── Private helpers ────────────────────────────────────────────────────────

    private async Task<IReadOnlyList<IElementHandle>> GetCellsForProductAsync(string productName)
    {
        await WaitForVisibleAsync(CartRows);
        var rows = await Page.QuerySelectorAllAsync(CartRows);

        foreach (var row in rows)
        {
            var cells = await row.QuerySelectorAllAsync("td");
            if (cells.Count == 0) continue;

            var name = await cells[0].InnerTextAsync();
            if (name.Contains(productName, StringComparison.OrdinalIgnoreCase))
                return cells;
        }

        throw new InvalidOperationException($"Product '{productName}' not found in cart.");
    }

    // ── Public API ─────────────────────────────────────────────────────────────

    public async Task<double> GetUnitPriceAsync(string productName)
    {
        var cells = await GetCellsForProductAsync(productName);
        return ParsePrice(await cells[1].InnerTextAsync());
    }

    /// <summary>
    /// Returns the subtotal for a product row.
    /// Jupiter Toys cart columns: 0=Name | 1=Price | 2=Qty | 3=Subtotal | 4=Remove
    /// Subtotal is the 2nd-to-last column when 5 columns exist.
    /// </summary>
    public async Task<double> GetSubtotalAsync(string productName)
    {
        var cells      = await GetCellsForProductAsync(productName);
        var subIdx     = cells.Count > 4 ? cells.Count - 2 : cells.Count - 1;
        return ParsePrice(await cells[subIdx].InnerTextAsync());
    }

    public async Task<double> GetGrandTotalAsync()
    {
        await WaitForVisibleAsync(CartRows);

        // Try multiple selectors for the grand-total element
        string[] selectors =
        [
            ".total > strong",
            "tr.total td:last-child",
            "tfoot tr td:last-child",
            ".cart-total strong"
        ];

        foreach (var sel in selectors)
        {
            var elements = await Page.QuerySelectorAllAsync(sel);
            foreach (var el in elements)
            {
                var txt = (await el.InnerTextAsync()).Trim();
                if (!string.IsNullOrEmpty(txt) && txt.Any(char.IsDigit))
                    return ParsePrice(txt);
            }
        }

        // Fallback: sum all row subtotals
        var allRows = await GetAllRowsAsync();
        return allRows.Sum(r => r.Subtotal);
    }

    public async Task<List<CartRow>> GetAllRowsAsync()
    {
        await WaitForVisibleAsync(CartRows);
        var rows   = await Page.QuerySelectorAllAsync(CartRows);
        var result = new List<CartRow>();

        foreach (var row in rows)
        {
            var cells = await row.QuerySelectorAllAsync("td");
            if (cells.Count < 4) continue;

            var name = (await cells[0].InnerTextAsync()).Trim();
            if (string.IsNullOrEmpty(name)) continue;

            var unitPrice = ParsePrice(await cells[1].InnerTextAsync());
            if (unitPrice == 0.0) continue; // skip header/total rows

            // Quantity may be in plain text or an <input> value
            var qtyRaw = (await cells[2].InnerTextAsync()).Trim();
            if (string.IsNullOrEmpty(qtyRaw))
            {
                var input = await cells[2].QuerySelectorAsync("input");
                if (input is not null)
                    qtyRaw = await input.GetAttributeAsync("value") ?? "1";
            }

            var quantity = int.TryParse(qtyRaw, out var q) ? q : 1;
            var subIdx   = cells.Count > 4 ? cells.Count - 2 : cells.Count - 1;
            var subtotal = ParsePrice(await cells[subIdx].InnerTextAsync());

            result.Add(new CartRow(name, unitPrice, quantity, subtotal));
        }

        return result;
    }
}
