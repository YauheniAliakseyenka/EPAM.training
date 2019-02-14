	Feature: UpdateEventSteps
	Description: The purpose of this feature is to test ability to edit event

	@UpdateEvent
	Scenario: Edit event information
	Given User is on home page
	When User logged in as an event manager
	And User sets "English" language of site
	And Goes to edit event page
	And Selects venue "Royal Albert Hall"
	And Selects event "test event" to edit on edit event page
	And Enters "Edit test event" to title input on edit event page
	And Enters "new description" to description input on edit event page
	And Clicks save button on edit event page
	Then User can see message "Saved" on edit event page

	@UpdateEventArea
	Scenario: Edit area of event
	Given User is on home page
	When User logged in as an event manager
	And User sets "Русский" language of site
	And Goes to edit event page
	And Selects venue "Royal Albert Hall"
	And Selects event "test event" to edit on edit event page
	And Selects area to edit
	And Enters "test edit area" to area description input
	And Enters "15,25" to area price input
	And Adds seat to area
	And Clicks save area button on area form
	Then User can see message "Сохранено" on area form

	@UpdateEventFailedWithLockedSeats
	Scenario: Edit event failed because of event has locked seats
	Given User is on home page
	When User logged in as an event manager
	And User sets "Русский" language of site
	And Goes to edit event page
	And Selects venue "Royal Albert Hall"
	And Selects event "test event" to edit on edit event page
	And Changes venue of event on "Symphony Hall"
	And Clicks save button on edit event page
	Then Edit event form has error "Нельзя изменять зал событие в котором присутствуют купленные или заблокированные места"
