	Feature: CreateEventSteps
	Description: The purpose of this feature is to test ability to create event

	@CreateNewEvent
	Scenario: Create new event
	Given User is on home page
	And Logged in as an event manager
	When User sets "English" language for the site
	And Selects Create event from dropdown menu on create event page
	And Enters "test event" to title input on create event page
	And Selects "Royal Albert Hall" from venue dropdown list on create event page
	And Enters a date of event on create event page
	And Enters a time of event on create event page
	And Enters "test description" to description input on create event page
	And Confirms creating of event on create event page
	Then User can see message "Saved" on create event page

	@CreateEventFailedBecauseAnEventWithTheSameDateExists
	Scenario: Creating of event failed because of an event with the same date  and at the same place already exists
	Given User is on home page
	And Logged in as an event manager
	When User sets "English" language for the site
	And Selects Create event from dropdown menu on create event page
	And Enters "test event" to title input on create event page
	And Selects "Royal Albert Hall" from venue dropdown list on create event page
	And Enters a date of event on create event page
	And Enters a time of event on create event page
	And Enters "test description" to description input on create event page
	And Confirms creating of event on create event page
	Then Create event form has error "Some event already created at the selected layout in picked time"

	Scenario: Creating of event failed because of a date in the past
	Given User is on home page
	And Logged in as an event manager
	When User sets "Русский" language for the site
	And Selects Create event from dropdown menu on create event page
	And Enters "test event" to title input on create event page
	And Selects "Royal Albert Hall" from venue dropdown list on create event page
	And Enters a past date of event on create event page
	And Enters a time of event on create event page
	And Enters "test description" to description input on create event page
	And Confirms creating of event on create event page
	Then Create event form has error "Попытка создания события на прошлую дату"
