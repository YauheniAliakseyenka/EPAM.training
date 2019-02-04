	Feature: SeatPurchaseSteps
	Description: The purpose of this feature is to test seat purchase flow

	@SeatPurchase
	Scenario: Seat purchase
	Given User is on home page
	When User clicks registration button
	And Enters generated username to username input on registration page
	And Enters generated email to email input on registration page
	And Sets site language to "English" on registration page
	And Enters generated password to password input on registration page
	And Enters generated password to confirm password input on registration page
	And Clicks register button on registration page
	And Goes to home page
	And Selects filter by "Title"
	And Enters "test event" to filter input
	And Selects "test event" event
	And Adds seat to cart
	And Goes to balance replenishment page
	And Enters "100.25" to amount input
	And Confirms balance replenishment
	And Goes to cart page
	And Removes seat from cart
	And Confirms order
	Then User goes to purchase history page
	And Can see created order
