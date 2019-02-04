using AutomatedTests.Utils;
using System.Configuration;
using TechTalk.SpecFlow;

namespace AutomatedTests.WebPages.Hooks
{
	[Binding]
	public class SeatPurchaseHooks : GlobalHooks
	{
		[BeforeScenario(
			"SeatPurchase", 
			"AddToCart", 
			"RemoveFromCart",
			"CompleteOrder", Order = 3)]
		public static void CreateEvent()
		{
			var helpers = Helpers;
			helpers.GoToUrl(ConfigurationManager.AppSettings["SiteUrl"]);
			var manager = UserFactory.GetEventManager();
			helpers.SignIn(manager.Username, manager.Password);
			helpers.CreateTestEvent();
			helpers.SetPriceToAreasOfCreatedTestEvent();
			helpers.SignOut();
		}

		[AfterScenario(
			"SeatPurchase",
			"CompleteOrder", Order = 1)]
		public static void DeleteEvent()
		{
			var helpers = Helpers;
			helpers.AuthenticatedRefundPurchasedSeat();
			helpers.SignOut();
			var manager = UserFactory.GetEventManager();
			helpers.SignIn(manager.Username, manager.Password);
			helpers.DeleteTestEvent();
			helpers.SignOut();
		}
	}
}
