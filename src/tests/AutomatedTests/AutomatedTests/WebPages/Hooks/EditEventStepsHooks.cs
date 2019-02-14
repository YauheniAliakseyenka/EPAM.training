using AutomatedTests.Utils;
using System.Configuration;
using TechTalk.SpecFlow;

namespace AutomatedTests.WebPages.Hooks
{
	[Binding]
	public class UpdateEventStepsHooks : GlobalHooks
	{
		[BeforeScenario(
			"UpdateEvent",
			"UpdateEventArea",
			"DeleteEvent",
			Order = 2)]
		public static void CreateEvent()
		{
			var helpers = Helpers;
			helpers.GoToUrl(ConfigurationManager.AppSettings["SiteUrl"]);
			var manager = UserFactory.GetEventManager();
			helpers.SignIn(manager.Username, manager.Password);
			helpers.CreateTestEvent();
			helpers.SignOut();
		}

		[BeforeScenario(
			"UpdateEventFailedWithLockedSeats",
			"DeleteEventFailedWithLockedSeats",
			Order = 2)]
		public static void CreatePublishedEventWithLockedSeats()
		{
			var helpers = Helpers;
			helpers.GoToUrl(ConfigurationManager.AppSettings["SiteUrl"]);
			var manager = UserFactory.GetEventManager();
			helpers.SignIn(manager.Username, manager.Password);
			helpers.CreateTestEvent();
			helpers.SetPriceToAreasOfCreatedTestEvent();
            helpers.LockSeatOfTestEvent();
            helpers.SignOut();
		}

		[AfterScenario(
			"UpdateEventFailedWithLockedSeats",
			"DeleteEventFailedWithLockedSeats",
			Order = 1)]
		public static void DeleteEventWithLockedSeats()
		{
			var helpers = Helpers;
			helpers.GoToUrl(ConfigurationManager.AppSettings["SiteUrl"]);
			helpers.DeleteSeatsFromCart();
			helpers.DeleteTestEvent();
			helpers.SignOut();
		}

		[AfterScenario(
			"UpdateEvent",
			"UpdateEventArea",
			Order = 1)]
		public static void DeleteEvent()
		{
			var helpers = Helpers;
			helpers.GoToUrl(ConfigurationManager.AppSettings["SiteUrl"]);
			helpers.DeleteTestEvent();
			helpers.SignOut();
		}
	}
}
