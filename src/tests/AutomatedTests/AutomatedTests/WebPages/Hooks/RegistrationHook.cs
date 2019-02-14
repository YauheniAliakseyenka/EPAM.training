using AutomatedTests.Utils;
using TechTalk.SpecFlow;

namespace AutomatedTests.WebPages.Hooks
{
	[Binding]
	public sealed class RegistrationHook : GlobalHooks
	{
		public const string UserKey = "UserRegistration";

		[BeforeScenario(
			"Registration", 
			"SeatPurchase", Order = 2)]
		public static void GenerateUserToRegistrate()
		{
			var user = UserFactory.GenerateUser();
			ScenarioContext.Current.Add(UserKey, user);
		}
	}
}
