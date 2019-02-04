using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace AutomatedTests.Utils
{
    public class WebDriverFactory
    {
        public static IWebDriver GetDriver()
        {
            var driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
			driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(60);
			driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
			driver.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromMilliseconds(2000);

            return driver;
        }
    }
}
