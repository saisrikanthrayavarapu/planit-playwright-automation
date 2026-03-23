Feature: Jupiter Toys Application Tests

  @TC1
  Scenario: Contact form shows validation errors on empty submit and clears them after filling fields
    Given I am on the home page
    When I navigate to the contact page
    And I click the submit button
    Then I should see forename validation error "Forename is required"
    And I should see email validation error "Email is required"
    And I should see message validation error "Message is required"
    When I fill in forename "Srikanth"
    And I fill in email "srikanth@test.com"
    And I fill in message "This is an automated test message"
    Then the forename validation error should not be visible
    And the email validation error should not be visible
    And the message validation error should not be visible

  @TC2
  Scenario Outline: Successful contact form submission
    Given I am on the home page
    When I navigate to the contact page
    And I fill in forename "<run>"
    And I fill in email "test<run>@example.com"
    And I fill in message "Automated test message for <run>"
    And I click the submit button
    Then I should see a success confirmation message containing "Thanks"

    Examples:
      | run  |
      | run1 |
      | run2 |
      | run3 |
      | run4 |
      | run5 |

  @TC3
  Scenario: Cart subtotals and grand total are calculated correctly
    Given I am on the home page
    When I navigate to the shop page
    And I add 2 of "Stuffed Frog" to the cart
    And I add 5 of "Fluffy Bunny" to the cart
    And I add 3 of "Valentine Bear" to the cart
    And I navigate to the cart page
    Then the subtotal for "Stuffed Frog" should equal unit price multiplied by 2
    And the subtotal for "Fluffy Bunny" should equal unit price multiplied by 5
    And the subtotal for "Valentine Bear" should equal unit price multiplied by 3
    And the grand total should equal the sum of all subtotals
