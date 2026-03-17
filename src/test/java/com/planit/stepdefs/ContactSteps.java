package com.planit.stepdefs;

import com.planit.pages.ContactPage;
import com.planit.pages.HomePage;
import com.planit.utils.DriverManager;
import io.cucumber.java.en.And;
import io.cucumber.java.en.Given;
import io.cucumber.java.en.Then;
import io.cucumber.java.en.When;
import org.openqa.selenium.WebDriver;

import static org.assertj.core.api.Assertions.assertThat;

public class ContactSteps {

    private WebDriver driver;
    private HomePage homePage;
    private ContactPage contactPage;

    private void init() {
        driver      = DriverManager.getDriver();
        homePage    = new HomePage(driver);
        contactPage = new ContactPage(driver);
    }

    @Given("I am on the home page")
    public void iAmOnTheHomePage() {
        init();
        // Driver is already navigated to the base URL by DriverManager.initDriver()
    }

    @When("I navigate to the contact page")
    public void iNavigateToTheContactPage() {
        contactPage = homePage.goToContact();
    }

    @And("I click the submit button")
    public void iClickTheSubmitButton() {
        contactPage.clickSubmit();
    }

    @Then("I should see forename validation error {string}")
    public void iShouldSeeForenameValidationError(String expectedText) {
        assertThat(contactPage.isForenameErrorVisible())
                .as("Forename error should be visible")
                .isTrue();
        assertThat(contactPage.getForenameErrorText())
                .as("Forename error text should contain: " + expectedText)
                .contains(expectedText);
    }

    @And("I should see email validation error {string}")
    public void iShouldSeeEmailValidationError(String expectedText) {
        assertThat(contactPage.isEmailErrorVisible())
                .as("Email error should be visible")
                .isTrue();
        assertThat(contactPage.getEmailErrorText())
                .as("Email error text should contain: " + expectedText)
                .contains(expectedText);
    }

    @And("I should see message validation error {string}")
    public void iShouldSeeMessageValidationError(String expectedText) {
        assertThat(contactPage.isMessageErrorVisible())
                .as("Message error should be visible")
                .isTrue();
        assertThat(contactPage.getMessageErrorText())
                .as("Message error text should contain: " + expectedText)
                .contains(expectedText);
    }

    @When("I fill in forename {string}")
    public void iFillInForename(String value) {
        contactPage.fillForename(value);
    }

    @And("I fill in email {string}")
    public void iFillInEmail(String value) {
        contactPage.fillEmail(value);
    }

    @And("I fill in message {string}")
    public void iFillInMessage(String value) {
        contactPage.fillMessage(value);
    }

    @Then("the forename validation error should not be visible")
    public void theForenameValidationErrorShouldNotBeVisible() {
        assertThat(contactPage.isForenameErrorHidden())
                .as("Forename error should be hidden after filling in the field")
                .isTrue();
    }

    @And("the email validation error should not be visible")
    public void theEmailValidationErrorShouldNotBeVisible() {
        assertThat(contactPage.isEmailErrorHidden())
                .as("Email error should be hidden after filling in the field")
                .isTrue();
    }

    @And("the message validation error should not be visible")
    public void theMessageValidationErrorShouldNotBeVisible() {
        assertThat(contactPage.isMessageErrorHidden())
                .as("Message error should be hidden after filling in the field")
                .isTrue();
    }

    @Then("I should see a success confirmation message containing {string}")
    public void iShouldSeeASuccessConfirmationMessageContaining(String expectedText) {
        assertThat(contactPage.isSuccessMessageVisible())
                .as("Success message should be visible")
                .isTrue();
        assertThat(contactPage.getSuccessMessageText())
                .as("Success message text should contain: " + expectedText)
                .contains(expectedText);
    }
}
