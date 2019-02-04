	Feature: RegistrationSteps
	Description: The purpose of this feature is to test registration of a user

	@Registration
	Scenario: Registration on the site
	Given User is on home page
	And User is not authenticated
	When User clicks registration button
	And Enters generated username to username input on registration page
	And Enters generated email to email input on registration page
	And Sets site language to "Беларускі" on registration page
	And Sets timezone to "W. Europe Standard Time" on registration page
	And Enters generated password to password input on registration page
	And Enters generated password to confirm password input on registration page
	And Clicks register button on registration page
	Then User sees profile container with his username
	And User is on home page with title "Галоўная"

	Scenario: Registration on the site failed with wrong confirmed password
	Given User is on home page
	And User is not authenticated
	When User sets "English" language of site
	And User clicks registration button
	And Enters "super_user" to username input on registration page
	And Enters "super_user@gmail.com" to email input on registration page
	And Sets site language to "Русский" on registration page
	And Sets timezone to "W. Europe Standard Time" on registration page
	And Enters "1231231231" to password input on registration page
	And Enters "1231231232" to password confirm input on registration page
	And Clicks register button on registration page
	Then Registration form has error "The password and confirmation password do not match"

	Scenario: Registration on the site failed because user has entered email which is already taken
	Given User is on home page
	And User is not authenticated
	When User sets "Русский" language of site
	And User clicks registration button
	And Enters "super_user" to username input on registration page
	And Enters "manager@gmail.com" to email input on registration page
	And Sets site language to "Русский" on registration page
	And Sets timezone to "W. Europe Standard Time" on registration page
	And Enters "1231231231" to password input on registration page
	And Enters "1231231231" to password confirm input on registration page
	And Clicks register button on registration page
	Then Registration form has error "Аккаунт с такой электронной почтой уже был создан"

	Scenario: Registration on the site failed because user has entered username which is already taken
	Given User is on home page
	And User is not authenticated
	When User sets "Беларускі" language of site
	And User clicks registration button
	And Enters "event_manager" to username input on registration page
	And Enters "manager@gmail.com" to email input on registration page
	And Sets site language to "Русский" on registration page
	And Sets timezone to "W. Europe Standard Time" on registration page
	And Enters "1231231231" to password input on registration page
	And Enters "1231231231" to password confirm input on registration page
	And Clicks register button on registration page
	Then Registration form has error "Карыстальнік з такім іменем ўжо існуе"


