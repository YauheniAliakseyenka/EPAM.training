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
		private static HomePage _homePage => PageFactory.Instance.GetPage<HomePage>();
		private static BalanceReplenishmentPage _balanceReplenishmentPage => PageFactory.Instance.GetPage<BalanceReplenishmentPage>();
		private static CartPage _cartPage => PageFactory.Instance.GetPage<CartPage>();
		private static PurchaseHistoryPage _purchaseHistoryPage => PageFactory.Instance.GetPage<PurchaseHistoryPage>();

		[When(@"Goes to home page")]
		public void WhenUserGoesToHomePage()
		{
			Thread.Sleep(150);
			_homePage.Open();
		}

		[When(@"Selects filter by ""(.*)""")]
		public void WhenUserSelectsFilter(string option)
		{
			_homePage.SelectFilterOption(option);
		}

		[When(@"Enters ""(.*)"" to filter input")]
		public void WhenUserEntersTextToFilterInput(string filterText)
		{
			_homePage.FilterInput.SendKeys(filterText);
			_homePage.SearchButton.Click();
		}

		[When(@"Selects ""(.*)"" event")]
		public void WhenUserSelectsEvent(string title)
		{
			_homePage.SelectEvent(title);
		}

		[When(@"Adds seat to cart")]
		public void WhenUserAddsTwoSeatToCart()
		{
			_homePage.LockSeat();
			Thread.Sleep(300);
		}

		[When(@"Goes to balance replenishment page")]
		public void WhenUserGoesToBalanceReplenishmentPage()
		{
			_balanceReplenishmentPage.Open();
		}

		[When(@"Enters ""(.*)"" to amount input")]
		public void WhenUserEntersAmountToAmountInput(string amount)
		{
			_balanceReplenishmentPage.AmountInput.Clear();
			_balanceReplenishmentPage.AmountInput.SendKeys(amount);
		}

		[When(@"Confirms balance replenishment")]
		public void WhenUserConfirmsBalanceReplenishment()
		{
            _balanceReplenishmentPage.CompleteBalanceReplenishment();
		}

		[When(@"Goes to cart page")]
		public void WhenUserGoesToCartPage()
		{
			_cartPage.Open();
		}

		[When(@"Removes seat from cart")]
		public void WhenUserRemovesOneSeatFromCart()
		{
			_cartPage.DeleteFirstSeatFromCart();
		}

		[When(@"Confirms order")]
		public void WhenUserConfirmsOrder()
		{
			_cartPage.CompleteOrder();
		}

		[Then(@"User goes to purchase history page")]
		public void ThenUseGoesToPurchaseHistoryPage()
		{
			_purchaseHistoryPage.Open();
		}

		[Then(@"Can see created order")]
		public void ThenUserCanUseOrder()
		{
			Assert.IsTrue(_purchaseHistoryPage.OrderTable.Displayed);
		}

		[Then(@"User can see added seat to cart")]
		public void ThenUserCanSeeAddedSeatsToCart()
		{
			Assert.IsTrue(_cartPage.OrderedSeats().Any());
		}

		[Then(@"User can see no seats left in the cart")]
		public void ThenUserCanSeeOneSeatLeftInTheCart()
		{
			Assert.Throws<WebDriverTimeoutException>(() => _cartPage.OrderedSeats());
		}

		[Then(@"User can see his cash balance ""(.*)""")]
		public void ThenUserSeesHisBalance(string balance)
		{
			var currentBalance = string.Empty;
			Assert.DoesNotThrow(() => currentBalance = _homePage.AccountBalance.Text);
			StringAssert.Contains(balance, currentBalance);
		}

		[Then(@"Balance replenishment form has error ""(.*)""")]
		public void ThenBalanceReplenishmentFormHasError(string expectedError)
		{
			var error = string.Empty;
			Assert.DoesNotThrow(() => error = _balanceReplenishmentPage.Errors.Text);
			StringAssert.Contains(expectedError, error);
		}
	}
}
