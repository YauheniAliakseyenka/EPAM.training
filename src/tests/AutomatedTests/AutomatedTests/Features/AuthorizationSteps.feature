	Feature: AuthorizationSteps
	Description: The purpose of this feature is to test authorization on a site

	@AuthorizationAsUser
	Scenario: Attempt of authorization on create event page as a user
	Given User is on home page
	When User logged in as a user
	And Tries to go to url "http://localhost:61963/Event/Create"
	Then User stayed on home page

	@AuthorizationAsUser
	Scenario: Attempt of authorization on edit event page as a user
	Given User is on home page
	When User logged in as a user
	And Tries to go to url "http://localhost:61963/Event/Edit"
	Then User stayed on home page

	Scenario: Authorization on create event page as an event manager
	Given User is on home page
	When User logged in as an event manager
	And Tries to go to url "http://localhost:61963/Event/Create"
	Then User successfully moved to create event page

	Scenario: Authorization on edit event page as an event manager
	Given User is on home page
	When User logged in as an event manager
	And Tries to go to url "http://localhost:61963/Event/Edit"
	Then User successfully moved to edit event page
