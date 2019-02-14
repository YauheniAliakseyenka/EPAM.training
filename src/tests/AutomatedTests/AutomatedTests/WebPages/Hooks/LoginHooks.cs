using AutomatedTests.Utils;
using System.Configuration;
using TechTalk.SpecFlow;

namespace AutomatedTests.WebPages.Hooks
{
	[Binding]
	public class LoginHooks:GlobalHooks
	{
		public const string UserKey  = "UserLogin";

		[BeforeScenario(
			"LoginAsUser", 
			"AddToCart", 
			"RemoveFromCart", 
			"BalanceReplenishment",
			"CompleteOrder",
			"AuthorizationAsUser", Order = 2)]
		public static void RegisterUser()
		{
			var user = UserFactory.GenerateUser();
			ScenarioContext.Current.Add(UserKey, user);

			var helpers = Helpers;
			helpers.GoToUrl(ConfigurationManager.AppSettings["SiteUrl"]);
			helpers.RegisterUser(user);
		}
	}
}
