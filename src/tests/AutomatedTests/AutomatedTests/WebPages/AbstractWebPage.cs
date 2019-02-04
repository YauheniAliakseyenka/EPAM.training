using AutomatedTests.Utils;
using OpenQA.Selenium;
using System.Collections.Generic;

namespace AutomatedTests.WebPages
{
    public abstract class AbstractWebPage
	{
        protected IWebDriver Driver;
		protected HelpersMethods HelperMethods;
		private DriverSelectors _selectors;

		public AbstractWebPage(IWebDriver driver)
		{
			Driver = driver;
			HelperMethods = new HelpersMethods(driver);
			_selectors = new DriverSelectors(driver);
		}

		protected IWebElement FindByCss(string css)
		{
			return _selectors.FindByCss(css);
		}

		protected IWebElement FindById(string id)
		{
			return _selectors.FindById(id);
		}

		protected IWebElement FindByXPath(string xpath)
		{
			return _selectors.FindByXPath(xpath);
		}

        protected IEnumerable<IWebElement> FindElementsByXPath(string xpath)
        {
            return _selectors.FindElementsByXpath(xpath);
        }

		protected void Open(string url)
		{
            Driver.Navigate().GoToUrl(url);
		}

		protected IWebElement SelectFromDropDownByText(By element, string text)
		{
			return _selectors.SelectFromDropDownByText(element, text);
		}
	}
}

