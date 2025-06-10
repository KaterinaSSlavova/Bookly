Feature: User Registration

Scenario: Successful user registration
Given I am on the registration page
When I enter a valid registration details
And I click the "Register" button
Then my account should be created
And I should be redirected to Log in page