package com.planit.pages;

import org.openqa.selenium.By;
import org.openqa.selenium.WebDriver;

public class ShopPage extends BasePage {

    private static final By CART_LINK = By.cssSelector("a[href*='cart']");

    public ShopPage(WebDriver driver) {
        super(driver);
    }

    private By getBuyButton(String productName) {
        String xpath = String.format(
            "//li[contains(@class,'product') or contains(@class,'product-wrap')]" +
            "[.//*[contains(text(),'%s')]]" +
            "//a[contains(@class,'btn-success') or contains(text(),'Buy')]",
            productName
        );
        return By.xpath(xpath);
    }

    public void addToCart(String productName, int quantity) {
        By buyButton = getBuyButton(productName);
        for (int i = 0; i < quantity; i++) {
            click(buyButton);
        }
    }

    public CartPage goToCart() {
        click(CART_LINK);
        return new CartPage(driver);
    }
}
