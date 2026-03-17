package com.planit.pages;

import org.openqa.selenium.By;
import org.openqa.selenium.WebDriver;

public class HomePage extends BasePage {

    private static final By NAV_CONTACT = By.linkText("Contact");
    private static final By NAV_SHOP    = By.linkText("Shop");

    public HomePage(WebDriver driver) {
        super(driver);
    }

    public ContactPage goToContact() {
        click(NAV_CONTACT);
        return new ContactPage(driver);
    }

    public ShopPage goToShop() {
        click(NAV_SHOP);
        return new ShopPage(driver);
    }
}
