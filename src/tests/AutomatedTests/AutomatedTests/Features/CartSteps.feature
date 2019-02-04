	Feature: CartSteps
	Description: The purpose of this feature is to test cart's functionality

	@AddToCart
	Scenario: Add seat to cart
	Given User is on home page
	When User logged in as a user
	And Selects filter by "Title"
	And Enters "test event" to filter input
	And Selects "test event" event
	And Adds seat to cart
	And Goes to cart page
	Then User can see added seat to cart

	@RemoveFromCart
	Scenario: Remove seat from cart
	Given User is on home page
	When User logged in as a user
	And Selects filter by "Title"
	And Enters "test event" to filter input
	And Selects "test event" event
	And Adds seat to cart
	And Goes to cart page
	And Removes seat from cart
	Then User can see no seats left in the cart

	@CompleteOrder
	Scenario: Comlete order
	Given User is on home page
	When User logged in as a user
	And Selects filter by "Title"
	And Enters "test event" to filter input
	And Selects "test event" event
	And Adds seat to cart
	And Goes to balance replenishment page
	And Enters "95.25" to amount input
	And Confirms balance replenishment
	And Goes to cart page
	And Confirms order
	Then User goes to purchase history page
	And Can see created order