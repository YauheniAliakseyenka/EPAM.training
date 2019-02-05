using AutomatedTests.Utils;
using OpenQA.Selenium;
using TechTalk.SpecFlow;

namespace AutomatedTests.WebPages.Hooks
{
	[Binding]
	public class GlobalHooks
	{
		public static string DriverKey { get; } = "Driver";

		protected static HelpersMethods Helpers => new HelpersMethods((IWebDriver)ScenarioContext.Current[DriverKey]);

		[BeforeScenario(Order = 1)]
		public static void OpenBrowser()
		{
			ScenarioContext.Current.Add(DriverKey, WebDriverFactory.GetDriver());
		}

		[AfterScenario]
		public static void CloseBrowser()
		{
			var driver = (IWebDriver)ScenarioContext.Current[DriverKey];
			ScenarioContext.Current.Clear();
			driver.Close();
			driver.Quit();
		}
	}
}
