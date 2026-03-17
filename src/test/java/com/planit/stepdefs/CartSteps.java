package com.planit.stepdefs;

import com.planit.pages.CartPage;
import com.planit.pages.HomePage;
import com.planit.pages.ShopPage;
import com.planit.utils.DriverManager;
import com.planit.utils.PriceUtils;
import io.cucumber.java.en.And;
import io.cucumber.java.en.Then;
import io.cucumber.java.en.When;
import org.openqa.selenium.WebDriver;

import java.util.HashMap;
import java.util.List;
import java.util.Map;

import static org.assertj.core.api.Assertions.assertThat;

public class CartSteps {

    private WebDriver driver;
    private HomePage homePage;
    private ShopPage shopPage;
    private CartPage cartPage;

    private final Map<String, Double>  unitPrices    = new HashMap<>();
    private final Map<String, Integer> quantities    = new HashMap<>();
    /** Actual subtotals read from the cart page during individual subtotal assertions. */
    private final Map<String, Double>  actualSubtotals = new HashMap<>();

    private void init() {
        driver   = DriverManager.getDriver();
        homePage = new HomePage(driver);
        shopPage = new ShopPage(driver);
        cartPage = new CartPage(driver);
    }

    // NOTE: "I am on the home page" is already defined in ContactSteps.
    // Cucumber shares step definitions across classes, so no duplicate definition needed here.
    // However we need to re-init our page references after the Given step runs.
    // We handle that by calling init() lazily in each step that needs it.

    @When("I navigate to the shop page")
    public void iNavigateToTheShopPage() {
        driver   = DriverManager.getDriver();
        homePage = new HomePage(driver);
        shopPage = homePage.goToShop();
        cartPage = new CartPage(driver);
    }

    @And("I add {int} of {string} to the cart")
    public void iAddOfToTheCart(int quantity, String productName) {
        if (shopPage == null) {
            shopPage = new ShopPage(DriverManager.getDriver());
        }
        shopPage.addToCart(productName, quantity);
        quantities.put(productName, quantity);
    }

    @And("I navigate to the cart page")
    public void iNavigateToTheCartPage() {
        if (shopPage == null) {
            shopPage = new ShopPage(DriverManager.getDriver());
        }
        cartPage = shopPage.goToCart();
    }

    @Then("the subtotal for {string} should equal unit price multiplied by {int}")
    public void theSubtotalForShouldEqualUnitPriceMultipliedBy(String productName, int multiplier) {
        if (cartPage == null) {
            cartPage = new CartPage(DriverManager.getDriver());
        }
        double unitPrice        = cartPage.getUnitPrice(productName);
        unitPrices.put(productName, unitPrice);
        double expectedSubtotal = PriceUtils.round2dp(unitPrice * multiplier);
        double actualSubtotal   = PriceUtils.round2dp(cartPage.getSubtotal(productName));

        assertThat(actualSubtotal)
                .as("Subtotal for '%s' should be %.2f (unit price %.2f × %d)",
                        productName, expectedSubtotal, unitPrice, multiplier)
                .isEqualTo(expectedSubtotal);
    }

    @And("the grand total should equal the sum of all subtotals")
    public void theGrandTotalShouldEqualTheSumOfAllSubtotals() {
        if (cartPage == null) {
            cartPage = new CartPage(DriverManager.getDriver());
        }
        List<CartPage.CartRow> rows = cartPage.getAllRows();
        double computedTotal = rows.stream()
                .mapToDouble(row -> row.subtotal)
                .sum();

        double grandTotal = cartPage.getGrandTotal();

        assertThat(PriceUtils.round2dp(grandTotal))
                .as("Grand total %.2f should equal sum of subtotals %.2f", grandTotal, computedTotal)
                .isEqualTo(PriceUtils.round2dp(computedTotal));
    }
}
