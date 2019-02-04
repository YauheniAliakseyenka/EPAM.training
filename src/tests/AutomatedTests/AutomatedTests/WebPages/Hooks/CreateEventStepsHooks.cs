using AutomatedTests.Utils;
using System.Configuration;
using TechTalk.SpecFlow;

namespace AutomatedTests.WebPages.Hooks
{
	[Binding]
	public class CreateEventStepsHooks : GlobalHooks
	{
		[BeforeScenario("CreateEventFailedBecauseAnEventWithTheSameDateExists", Order = 2)]
		public static void CreateEvent()
		{
			var helpers = Helpers;
			helpers.GoToUrl(ConfigurationManager.AppSettings["SiteUrl"]);
			var manager = UserFactory.GetEventManager();
			helpers.SignIn(manager.Username, manager.Password);
			helpers.CreateTestEvent();
			helpers.SignOut();
		}

		[AfterScenario(
			"CreateNewEvent",
			"CreateEventFailedBecauseAnEventWithTheSameDateExists",
			Order = 1)]
		public static void DeleteEvent()
		{
			var helpers = Helpers;
			helpers.DeleteTestEvent();
			helpers.SignOut();
		}
	}
}
