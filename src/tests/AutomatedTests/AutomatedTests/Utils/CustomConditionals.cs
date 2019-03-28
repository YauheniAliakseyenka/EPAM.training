using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace AutomatedTests.Utils
{
    public class CustomConditionals
    {
		public static void SendedToField(IWebDriver driver, By to, string inputText)
		{
			var inputElement = new WebDriverWait(driver, TimeSpan.FromSeconds(4)).
				Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(to));

			new WebDriverWait(driver, TimeSpan.FromSeconds(5))
			   .Until<IWebElement>((d) =>
			   {
				   inputElement.Clear();
				   inputElement.SendKeys(inputText);

				   if (inputElement.GetAttribute("value").Equals(inputText, StringComparison.Ordinal))
					   return inputElement;

				   return null;
			   });
		}

		public static void ClickUntil(IWebDriver driver, By clickElement, By untilElementVisible, int attempts = 3)
		{
			new WebDriverWait(driver, TimeSpan.FromSeconds(4)).
							Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(clickElement));

			for (int i = 0; i < attempts; i++)
			{
				var elementToClick = driver.FindElement(clickElement);
				elementToClick.Click();
				try
				{
					new WebDriverWait(driver, TimeSpan.FromSeconds(4)).
						Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(untilElementVisible));
					break;
				}
				catch (WebDriverTimeoutException) { }
			}
		}

		public static void WaitForAlert(IWebDriver driver)
		{
			new WebDriverWait(driver, TimeSpan.FromSeconds(4)).
							Until(SeleniumExtras.WaitHelpers.ExpectedConditions.AlertIsPresent());
		}

		public static void WaitForLoggedIn(IWebDriver driver)
		{
			new WebDriverWait(driver, TimeSpan.FromSeconds(4)).
				Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector(".profile-right")));
		}

		public static void WaitForNotisfaction(IWebDriver driver)
		{
			new WebDriverWait(driver, TimeSpan.FromSeconds(4)).
			   Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible
			   (By.XPath("//*[@data-notify='container']")));
		}
	}
}
