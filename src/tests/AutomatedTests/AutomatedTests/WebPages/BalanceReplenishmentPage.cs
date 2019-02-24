using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Configuration;

namespace AutomatedTests.WebPages
{
	public class BalanceReplenishmentPage : AbstractWebPage
	{
		public BalanceReplenishmentPage(IWebDriver driver) : base(driver) { }

		public IWebElement AmountInput => FindById("Amount");
		public IWebElement Errors => FindById("errorMessagesBalanceReplenishment");
		public IWebElement BalanceReplenishmentButton => FindByXPath("//*[contains(@onclick, 'BalanceReplenishment')]");

		public void Open()
		{
			var url = ConfigurationManager.AppSettings["SiteUrl"] + "Account/BalanceReplenishment";
			base.Open(url);
		}

		public void CompleteBalanceReplenishment()
		{
			BalanceReplenishmentButton.Click();
			try
			{
				new WebDriverWait(Driver, TimeSpan.FromSeconds(4)).
						Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector(".event-list")));
			}
			catch (WebDriverTimeoutException) { }
		}
	}
}
