	Feature: BalanceReplenishmentSteps
	Description: The purpose of this feature is to test opportunity of balance replenishment

	@BalanceReplenishment
	Scenario: Balance replenishment
	Given User is on home page
	When User logged in as a user 
	And User sets "English" language of site
	And Goes to balance replenishment page
	And Enters "15.25" to amount input
	And Confirms balance replenishment 
	Then User can see his cash balance "15.25" 

	@BalanceReplenishment
	Scenario: Balance replenishment failed with wrong amount
	Given User is on home page
	When User logged in as a user 
	And User sets "Русский" language of site
	And Goes to balance replenishment page
	And Enters "15.25" to amount input
	And Confirms balance replenishment 
	Then Balance replenishment form has error "Значение '15.25' некорректно для Сумма"
