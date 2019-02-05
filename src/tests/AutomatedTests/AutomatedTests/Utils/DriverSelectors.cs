﻿using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutomatedTests.Utils
{
	public class DriverSelectors
	{
		private IWebDriver _driver;

		public DriverSelectors(IWebDriver driver)
		{
			_driver = driver;
		}

		public IWebElement FindByCss(string css)
		{
			new WebDriverWait(_driver, TimeSpan.FromSeconds(3)).
				Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector(css)));

			return _driver.FindElement(By.CssSelector(css));
		}

		public IWebElement FindById(string id)
		{
			new WebDriverWait(_driver, TimeSpan.FromSeconds(3)).
				Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id(id)));

			return _driver.FindElement(By.Id(id));
		}

		public IWebElement FindByXPath(string xpath)
		{
			new WebDriverWait(_driver, TimeSpan.FromSeconds(3)).
				Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath(xpath)));

			return _driver.FindElement(By.XPath(xpath));
		}

        public IEnumerable<IWebElement> FindElementsByXpath(string xpath)
        {
            new WebDriverWait(_driver, TimeSpan.FromSeconds(3))
                .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath(xpath)));

            return _driver.FindElements(By.XPath(xpath));
        }

		public IWebElement SelectFromDropDownByText(By findBy, string selectByText)
		{
			new WebDriverWait(_driver, TimeSpan.FromSeconds(3)).
				Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(findBy));

			var element = _driver.FindElement(findBy);
			var select = new SelectElement(element);
			select.SelectByText(selectByText, true);

			//find again, cuz page can be refreshed
			return _driver.FindElement(findBy);
		}
	}
}
