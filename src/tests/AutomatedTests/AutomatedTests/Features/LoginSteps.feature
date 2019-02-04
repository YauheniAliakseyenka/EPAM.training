	Feature: LoginSteps
	Description: The purpose of this feature is to test ability to authenticated on a site

	Scenario: Sign in as an event manager
	Given User is on home page
	When User logged in as an event manager
	Then User can see user profile container 

	@LoginAsUser
	Scenario: Sign in as a common user 
	Given User is on home page
	When User logged in as a user
	Then User can see user profile container

	Scenario: Sign in failed with wrong credentials
	Given User is on home page
	When User sets "Русский" language of site
	And User enters "venue_manager" to username input and "123" to password input
	Then User can see error message "Имя пользователя или пароль нeверны" on login form
