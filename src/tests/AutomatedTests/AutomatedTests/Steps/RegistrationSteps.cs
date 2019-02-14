using AutomatedTests.Configuration.WebPages;
using AutomatedTests.Utils;
using AutomatedTests.WebPages.Hooks;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using TechTalk.SpecFlow;

namespace AutomatedTests.Steps
{
	[Binding]
	public class RegistrationSteps
	{
		private static RegistrationPage RegistrationPage => PageFactory.Instance.GetPage<RegistrationPage>();
		private static Lazy<UserModel> User => new Lazy<UserModel>(() => (UserModel)ScenarioContext.Current[RegistrationHook.UserKey]);

		[Given(@"User is not authenticated")]
		public void GivenUserIsNotAuthenticated()
		{
			Assert.IsTrue(RegistrationPage.SignUpButton.Displayed);
		}

		[When(@"User sets ""(.*)"" language of site")]
		public void WhenUserSetsLanguageOfSite(string language)
		{
			RegistrationPage.SelectLanguageOfSiteFromDropDown(language);
		}

		[When(@"User clicks registration button")]
		public void WhenUserClicksRegistrationButton()
		{
			RegistrationPage.SignUpButton.Click();
		}

		[When(@"Enters generated username to username input on registration page")]
		public void WhenEntersGeneratedUsernameToUsernameInput()
		{
			RegistrationPage.UsernameInput.SendKeys(User.Value.Username);
		}

		[When(@"Enters ""(.*)"" to username input on registration page")]
		public void WhenEntersToUsernameInput(string username)
		{
			RegistrationPage.UsernameInput.SendKeys(username);
		}

		[When(@"Enters generated email to email input on registration page")]
		public void WhenEntersGeneratedEmailToEmailInput()
		{
			RegistrationPage.EmailInput.SendKeys(User.Value.Email);
		}

		[When(@"Enters ""(.*)"" to email input on registration page")]
		public void WhenEntersToEmailInput(string email)
		{
			RegistrationPage.EmailInput.SendKeys(email);
		}

		[When(@"Sets site language to ""(.*)"" on registration page")]
		public void WhenSetsLanguage(string language)
		{
			RegistrationPage.SelectLanguageFromDropDown(language);
		}

		[When(@"Sets timezone to ""(.*)"" on registration page")]
		public void WhenSetsTimezone(string timezone)
		{
			RegistrationPage.SelectTimezoneFromDropDown(timezone);
		}

		[When(@"Enters generated password to password input on registration page")]
		public void WhenEntersGeneratedPasswordToPasswordInput()
		{
			RegistrationPage.PasswordRegistrationInput.SendKeys(User.Value.Password);
		}

		[When(@"Enters ""(.*)"" to password input on registration page")]
		public void WhenEntersToPasswordInput(string password)
		{
			RegistrationPage.PasswordRegistrationInput.SendKeys(password);
		}

		[When(@"Enters generated password to confirm password input on registration page")]
		public void WhenEntersGeneratedPasswordToConfirmPasswordInput()
		{
			RegistrationPage.ConfirmPasswordInput.SendKeys(User.Value.Password);
		}

		[When(@"Enters ""(.*)"" to password confirm input on registration page")]
		public void WhenEntersToConfirmPasswordInput(string password)
		{
			RegistrationPage.ConfirmPasswordInput.SendKeys(password);
		}

		[When(@"Clicks register button on registration page")]
		public void WhenClicksRegistrationButton()
		{
			RegistrationPage.RegisterButton.Click();
		}

		[Then(@"User sees profile container with his username")]
		public void ThenLoginFormHasError()
		{
			Assert.DoesNotThrow(() => RegistrationPage.UserProfileContainer.
				FindElement(By.XPath($"//*[contains(text(), '{User.Value.Username}')]")));
		}

		[Then(@"User is on home page with title ""(.*)""")]
		public void ThenUserIsOnHomePageAfterSuccessfulRegistration(string title)
		{
			Assert.That(RegistrationPage.Title, Is.EqualTo(title));
        }

        [Then(@"Registration form has error ""(.*)""")]
        public void ThenRegistrationFormHasPasswordError(string error)
        {
            string errorMessage = string.Empty;
            Assert.DoesNotThrow(() => errorMessage = RegistrationPage.RegistrationErrors.Text);
			StringAssert.Contains(error, errorMessage);
        }
    }
}
