using AutomatedTests.Utils;
using OpenQA.Selenium;
using TechTalk.SpecFlow;

namespace AutomatedTests.WebPages.Hooks
{
	[Binding]
	public class GlobalHooks
	{
		protected static HelpersMethods Helpers => new HelpersMethods(Driver);
		public static IWebDriver Driver { get; private set; }

		[BeforeTestRun]
		public static void OneTimeSetUp()
		{
			Driver = WebDriverFactory.GetDriver();
		}

		[AfterTestRun]
		public static void OneTimeTearDown()
		{
			Driver.Close();
			Driver.Quit();
		}

		[BeforeScenario(Order = 1)]
		public static void OpenBrowser()
		{
			Helpers.SignOut();
		}

		[AfterScenario]
		public static void CloseBrowser()
		{
			ScenarioContext.Current.Clear();
		}
	}
}
