using AutomatedTests.Utils;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace AutomatedTests.WebPages
{
	public class CartPage : AbstractWebPage
	{
		public CartPage(IWebDriver driver) : base(driver) { }

		public IWebElement OrderTable => FindById("orderedList");
		public IWebElement OrderButton => FindByXPath("//*[contains(@onclick, 'CompleteOrder')]");

		public void DeleteFirstSeatFromCart()
		{
            var deleteButton = FindByXPath("//*[@id='orderedList']//*[contains(@onclick, 'DeleteSeatFromCart')]");		
			deleteButton.Click();

            CustomConditionals.WaitForNotisfaction(Driver);
		}

		public void Open()
		{
			var url = ConfigurationManager.AppSettings["SiteUrl"] + "Account/Cart";
			base.Open(url);
		}

		public IEnumerable<IWebElement> OrderedSeats()
		{
            var xPath = "//*[@id='orderedSeatsList']//tbody//tr";

            new WebDriverWait(Driver, TimeSpan.FromSeconds(4)).
                Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath(xPath)));

            return FindElementsByXPath(xPath);
		}

		public void CompleteOrder()
		{
			OrderButton.Click();
            CustomConditionals.WaitForNotisfaction(Driver);
		}
	}
}
