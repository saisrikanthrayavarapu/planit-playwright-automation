package com.planit.pages;

import org.openqa.selenium.By;
import org.openqa.selenium.WebDriver;

public class ContactPage extends BasePage {

    private static final By FIELD_FORENAME = By.id("forename");
    private static final By FIELD_EMAIL    = By.id("email");
    private static final By FIELD_MESSAGE  = By.id("message");
    private static final By BTN_SUBMIT     = By.cssSelector("a.btn-contact, .btn-contact");
    private static final By ERR_FORENAME   = By.id("forename-err");
    private static final By ERR_EMAIL      = By.id("email-err");
    private static final By ERR_MESSAGE    = By.id("message-err");
    private static final By SUCCESS_ALERT  = By.cssSelector(".alert-success");

    public ContactPage(WebDriver driver) {
        super(driver);
    }

    public void clickSubmit() {
        click(BTN_SUBMIT);
    }

    public void fillForename(String value) {
        type(FIELD_FORENAME, value);
    }

    public void fillEmail(String value) {
        type(FIELD_EMAIL, value);
    }

    public void fillMessage(String value) {
        type(FIELD_MESSAGE, value);
    }

    public boolean isForenameErrorVisible() {
        return isVisible(ERR_FORENAME);
    }

    public boolean isEmailErrorVisible() {
        return isVisible(ERR_EMAIL);
    }

    public boolean isMessageErrorVisible() {
        return isVisible(ERR_MESSAGE);
    }

    public boolean isForenameErrorHidden() {
        return isHidden(ERR_FORENAME);
    }

    public boolean isEmailErrorHidden() {
        return isHidden(ERR_EMAIL);
    }

    public boolean isMessageErrorHidden() {
        return isHidden(ERR_MESSAGE);
    }

    public String getForenameErrorText() {
        return getText(ERR_FORENAME);
    }

    public String getEmailErrorText() {
        return getText(ERR_EMAIL);
    }

    public String getMessageErrorText() {
        return getText(ERR_MESSAGE);
    }

    /**
     * Waits up to 30 seconds for the success banner to appear
     * (the Jupiter Toys form submission shows a spinner before the banner).
     */
    public boolean isSuccessMessageVisible() {
        try {
            waitForVisible(SUCCESS_ALERT, 30);
            return true;
        } catch (Exception e) {
            return false;
        }
    }

    public String getSuccessMessageText() {
        try {
            waitForVisible(SUCCESS_ALERT, 30);
        } catch (Exception ignored) { /* fall through */ }
        return getText(SUCCESS_ALERT);
    }
}
