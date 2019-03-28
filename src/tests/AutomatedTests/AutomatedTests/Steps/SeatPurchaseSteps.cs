using AutomatedTests.Utils;
using AutomatedTests.WebPages;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Linq;
using System.Threading;
using TechTalk.SpecFlow;

namespace AutomatedTests.Steps
{
	[Binding]
	public class SeatPurchaseSteps
	{
		private static HomePage HomePage => PageFactory.Instance.GetPage<HomePage>();
		private static BalanceReplenishmentPage BalanceReplenishmentPage => PageFactory.Instance.GetPage<BalanceReplenishmentPage>();
		private static CartPage CartPage => PageFactory.Instance.GetPage<CartPage>();
		private static PurchaseHistoryPage PurchaseHistoryPage => PageFactory.Instance.GetPage<PurchaseHistoryPage>();

		[When(@"Goes to home page")]
		public void WhenUserGoesToHomePage()
		{
			Thread.Sleep(150);
			HomePage.Open();
		}

		[When(@"Selects filter by ""(.*)""")]
		public void WhenUserSelectsFilter(string option)
		{
			HomePage.SelectFilterOption(option);
		}

		[When(@"Enters ""(.*)"" to filter input")]
		public void WhenUserEntersTextToFilterInput(string filterText)
		{
			HomePage.FilterInput.SendKeys(filterText);
			HomePage.SearchButton.Click();
			Thread.Sleep(300);
		}

		[When(@"Selects ""(.*)"" event")]
		public void WhenUserSelectsEvent(string title)
		{
			HomePage.SelectEvent(title);
		}

		[When(@"Adds seat to cart")]
		public void WhenUserAddsTwoSeatToCart()
		{
			HomePage.LockSeat();
			Thread.Sleep(300);
		}

		[When(@"Goes to balance replenishment page")]
		public void WhenUserGoesToBalanceReplenishmentPage()
		{
			BalanceReplenishmentPage.Open();
		}

		[When(@"Enters ""(.*)"" to amount input")]
		public void WhenUserEntersAmountToAmountInput(string amount)
		{
			BalanceReplenishmentPage.AmountInput.Clear();
			BalanceReplenishmentPage.AmountInput.SendKeys(amount);
		}

		[When(@"Confirms balance replenishment")]
		public void WhenUserConfirmsBalanceReplenishment()
		{
			BalanceReplenishmentPage.CompleteBalanceReplenishment();
		}

		[When(@"Goes to cart page")]
		public void WhenUserGoesToCartPage()
		{
			CartPage.Open();
		}

		[When(@"Removes seat from cart")]
		public void WhenUserRemovesOneSeatFromCart()
		{
			CartPage.DeleteFirstSeatFromCart();
		}

		[When(@"Confirms order")]
		public void WhenUserConfirmsOrder()
		{
			CartPage.CompleteOrder();
		}

		[Then(@"User goes to purchase history page")]
		public void ThenUseGoesToPurchaseHistoryPage()
		{
			PurchaseHistoryPage.Open();
		}

		[Then(@"Can see created order")]
		public void ThenUserCanUseOrder()
		{
			Assert.IsTrue(PurchaseHistoryPage.OrderTable.Displayed);
		}

		[Then(@"User can see added seat to cart")]
		public void ThenUserCanSeeAddedSeatsToCart()
		{
			Assert.IsTrue(CartPage.OrderedSeats().Any());
		}

		[Then(@"User can see no seats left in the cart")]
		public void ThenUserCanSeeOneSeatLeftInTheCart()
		{
			Assert.Throws<WebDriverTimeoutException>(() => CartPage.OrderedSeats());
		}

		[Then(@"User can see his cash balance ""(.*)""")]
		public void ThenUserSeesHisBalance(string balance)
		{
			var currentBalance = string.Empty;
			Assert.DoesNotThrow(() => currentBalance = HomePage.AccountBalance.Text);
			StringAssert.Contains(balance, currentBalance);
		}

		[Then(@"Balance replenishment form has error ""(.*)""")]
		public void ThenBalanceReplenishmentFormHasError(string expectedError)
		{
			var error = string.Empty;
			Assert.DoesNotThrow(() => error = BalanceReplenishmentPage.Errors.Text);
			StringAssert.Contains(expectedError, error);
		}
	}
}
