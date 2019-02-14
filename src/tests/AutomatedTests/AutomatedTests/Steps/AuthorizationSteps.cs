using AutomatedTests.Utils;
using AutomatedTests.WebPages;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace AutomatedTests.Steps
{
	[Binding]
	public class AuthorizationSteps
	{
		private static HomePage HomePage => PageFactory.Instance.GetPage<HomePage>();

		[When(@"Tries to go to url ""(.*)""")]
		public void WhenUserGoesToUrl(string url)
		{
			HomePage.GoToUrl(url);
		}

		[Then(@"User stayed on home page")]
		public void ThenUserStayedOnHomePage()
		{
			StringAssert.DoesNotContain("/Event/Create", HomePage.CurrentUrl);
		}


		[Then(@"User successfully moved to create event page")]
		public void ThenUserMovedToCreateEventPage()
		{
			StringAssert.Contains("/Event/Create", HomePage.CurrentUrl);
		}

		[Then(@"User successfully moved to edit event page")]
		public void ThenUserMovedToEditEventPage()
		{
			StringAssert.Contains("/Event/Edit", HomePage.CurrentUrl);
		}
	}
}
