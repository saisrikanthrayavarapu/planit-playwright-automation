package com.planit.pages;

import org.openqa.selenium.By;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.WebElement;

import java.util.ArrayList;
import java.util.List;

public class CartPage extends BasePage {

    private static final By CART_ROWS  = By.cssSelector("table tbody tr");
    // Jupiter Toys cart grand total is in a tfoot/last-row td, or a strong with class "total"
    private static final By CART_TOTAL = By.cssSelector(".total > strong, tfoot .total, tr.total td:last-child");

    public CartPage(WebDriver driver) {
        super(driver);
    }

    /**
     * Inner static class representing a row in the cart table.
     */
    public static class CartRow {
        public String name;
        public double unitPrice;
        public int    quantity;
        public double subtotal;

        public CartRow(String name, double unitPrice, int quantity, double subtotal) {
            this.name      = name;
            this.unitPrice = unitPrice;
            this.quantity  = quantity;
            this.subtotal  = subtotal;
        }
    }

    private WebElement findRowByProductName(String productName) {
        List<WebElement> rows = driver.findElements(CART_ROWS);
        for (WebElement row : rows) {
            List<WebElement> cells = row.findElements(By.tagName("td"));
            if (!cells.isEmpty() && cells.get(0).getText().contains(productName)) {
                return row;
            }
        }
        throw new RuntimeException("Product not found in cart: " + productName);
    }

    public double getUnitPrice(String productName) {
        WebElement row   = findRowByProductName(productName);
        List<WebElement> cells = row.findElements(By.tagName("td"));
        return parsePrice(cells.get(1).getText());
    }

    /**
     * The Jupiter Toys cart table columns are:
     *   0: Item name | 1: Price | 2: Qty | 3: Subtotal | 4: Remove (×)
     * Subtotal is at index 3, NOT the last column (index 4 = Remove button = empty text).
     */
    public double getSubtotal(String productName) {
        WebElement row = findRowByProductName(productName);
        List<WebElement> cells = row.findElements(By.tagName("td"));
        // Use 2nd-to-last col; if only 4 cols exist the last IS the subtotal
        int subtotalIdx = cells.size() > 4 ? cells.size() - 2 : cells.size() - 1;
        return parsePrice(cells.get(subtotalIdx).getText());
    }

    public double getGrandTotal() {
        // Wait for at least one cart row to ensure page is loaded
        waitForVisible(CART_ROWS);
        // Try multiple selectors for the grand total row
        String[] selectors = {
            ".total > strong",
            "tr.total td:last-child",
            "tfoot tr td:last-child",
            ".cart-total strong"
        };
        for (String sel : selectors) {
            try {
                List<WebElement> els = driver.findElements(By.cssSelector(sel));
                for (WebElement el : els) {
                    String txt = el.getText().trim();
                    if (!txt.isEmpty() && txt.matches(".*[0-9].*")) {
                        return parsePrice(txt);
                    }
                }
            } catch (Exception ignored) { /* try next */ }
        }
        // Fallback: sum all subtotals from rows
        return getAllRows().stream().mapToDouble(r -> r.subtotal).sum();
    }

    public List<CartRow> getAllRows() {
        waitForVisible(CART_ROWS);
        List<WebElement> rows = driver.findElements(CART_ROWS);
        List<CartRow> result  = new ArrayList<>();
        for (WebElement row : rows) {
            List<WebElement> cells = row.findElements(By.tagName("td"));
            if (cells.size() < 4) continue;

            String name = cells.get(0).getText().trim();
            // Skip rows that aren't product rows (e.g. header/total rows)
            if (name.isEmpty()) continue;

            double unitPrice = parsePrice(cells.get(1).getText());
            // Skip rows without a parseable unit price (e.g. header rows)
            if (unitPrice == 0.0) continue;

            // Quantity column may be plain text or an <input> element
            String qtyRaw = cells.get(2).getText().trim();
            if (qtyRaw.isEmpty()) {
                try {
                    WebElement inp = cells.get(2).findElement(By.tagName("input"));
                    qtyRaw = inp.getAttribute("value").trim();
                } catch (Exception ignored) { qtyRaw = "1"; }
            }
            int quantity;
            try { quantity = Integer.parseInt(qtyRaw); }
            catch (NumberFormatException e) { quantity = 1; } // safe default

            // Subtotal: 2nd-to-last col if 5+ cols, otherwise last col
            int subIdx = cells.size() > 4 ? cells.size() - 2 : cells.size() - 1;
            double subtotal = parsePrice(cells.get(subIdx).getText());
            result.add(new CartRow(name, unitPrice, quantity, subtotal));
        }
        return result;
    }
}
