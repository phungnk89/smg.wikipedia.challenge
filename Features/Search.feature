Feature: Search

Contains all the test scenarios for the Search feature

@search
Scenario: F03 - Verify the Search function in Wikipedia Main Page
	Given I have accessed to Wikipedia Main Page
	Then the Wikipedia Main Page should display
	When I input keywords 'Software Testing' in Search field
	Then it should pop out suggestion that match the keywords
	And I click Search button
	Then the relevant result page should display
