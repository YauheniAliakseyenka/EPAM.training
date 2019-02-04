using AutomatedTests.Utils;
using TechTalk.SpecFlow;

namespace AutomatedTests.WebPages.Hooks
{
	[Binding]
	public class CartHooks : GlobalHooks
	{
		[AfterScenario("AddToCart", "RemoveFromCart", Order = 1)]
		public static void DeleteEvent()
		{
			var helpers = Helpers;
			helpers.DeleteSeatsFromCart();
			helpers.SignOut();
			var manager = UserFactory.GetEventManager();
			helpers.SignIn(manager.Username, manager.Password);
			helpers.DeleteTestEvent();
			helpers.SignOut();
		}
	}
}
