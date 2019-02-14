	Feature: DeleteEventSteps
	Description: The purpose of this feature is to test ability to delete event

	@DeleteEventFailedWithLockedSeats
	Scenario: Delete event failed because of event has locked seats
	Given User is on home page
	When User logged in as an event manager
	And User sets "English" language of site
	And Goes to edit event page
	And Selects venue "Royal Albert Hall"
	And Selects event "test event" to edit on edit event page
	And Clicks delete event button on edit event page
	Then Edit event form has error "Not allowed to delete event. Event has locked seats"

	@DeleteEvent
	Scenario: Delete event
	Given User is on home page
	When User logged in as an event manager
	And User sets "English" language of site
	And Goes to edit event page
	And Selects venue "Royal Albert Hall"
	And Selects event "test event" to edit on edit event page
	And Clicks delete event button on edit event page
	Then Event is not represent in a system anymore
