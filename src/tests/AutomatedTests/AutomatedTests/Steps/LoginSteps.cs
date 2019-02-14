using AutomatedTests.Utils;
using AutomatedTests.WebPages;
using AutomatedTests.WebPages.Hooks;
using NUnit.Framework;
using System;
using TechTalk.SpecFlow;

namespace AutomatedTests.Steps
{
    [Binding]
    public class LoginSteps
    {
        private static HomePage HomePage => PageFactory.Instance.GetPage<HomePage>();
		private static Lazy<UserModel> User => new Lazy<UserModel>(() => (UserModel)ScenarioContext.Current[LoginHooks.UserKey]);

		[Given(@"User is on home page")]
        public void GivenUserOnHomePage()
        {
			HomePage.Open();
        }

		[When(@"User logged in as an event manager")]
		public void WhenUserLoggedInAsManager()
		{
			var manager = UserFactory.GetEventManager();
			HomePage.SignIn(manager.Username, manager.Password);
		}

		[When(@"User enters ""(.*)"" to username input and ""(.*)"" to password input")]
		public void WhenUserLoggedIn(string username, string password)
		{
			HomePage.SignIn(username, password);
		}

		[When(@"User logged in as a user")]
		public void WhenUserLoggedIn()
		{
			var user = User.Value;
			HomePage.SignIn(user.Username, user.Password);
		}

		[Then(@"User can see user profile container")]
		public void ThenUserCanSeeUserProfileContainer()
		{
			Assert.IsTrue(HomePage.UserProfileContainer.Displayed);
		}

		[Then(@"User can see menu option to manage events")]
		public void ThenUserCanSeeMenuOptionToManageEvents()
		{
			Assert.IsTrue(HomePage.ManageEventsMenu.Displayed);
		}

		[Then(@"User can see error message ""(.*)"" on login form")]
		public void ThenLoginFormHasError(string error)
		{
			string errorMessage = string.Empty;
			Assert.DoesNotThrow(() => errorMessage = HomePage.LoginErrorMessages.Text);
			StringAssert.Contains(error, errorMessage);
		}
	}
}
