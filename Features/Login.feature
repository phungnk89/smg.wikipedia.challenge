Feature: Login

Contains all the test scenarios for the Login feature

@login
Scenario: F07 - Verify the user can login to Wikipedia with correct account
	Given I have accessed to Wikipedia Main Page
	Then the Wikipedia Main Page should display
	When I click on the Login link
	Then the Login screen should display
	When I input valid account
	And I click Login button
	Then the Main Page should display with my username on top


@forgotpassword
Scenario: F09 - Verify the forgot password in Wikipedia
	Given I have accessed to Wikipedia Main Page
	Then the Wikipedia Main Page should display
	When I click on the Login link
	Then the Login screen should display
	When I click Forgot Password link
	Then the Reset Password screen should display
	When I input valid username and email
	And I click Reset Password button
	Then the Reset Instruction text should display
	When I check my mailbox
	Then I should receive Reset Password email
	When I get the temporary password from the email
	And I navigate back to Wikipedia Main Page
	Then the Wikipedia Main Page should display
	When I click on the Login link
	Then the Login screen should display
	When I login with the temporary password
	Then the New Password screen should display
	When I input my new password
	And I click Continue Login button
	Then the Main Page should display with my username on top