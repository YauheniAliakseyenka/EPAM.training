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
		private static RegistrationPage _registrationPage => PageFactory.Instance.GetPage<RegistrationPage>();

		private static Lazy<UserModel> _user => new Lazy<UserModel>(() => (UserModel)ScenarioContext.Current[RegistrationHook.UserKey]);

		[Given(@"User is not authenticated")]
		public void GivenUserIsNotAuthenticated()
		{
			Assert.IsTrue(_registrationPage.SignUpButton.Displayed);
		}

		[When(@"User sets ""(.*)"" language of site")]
		public void WhenUserSetsLanguageOfSite(string language)
		{
			_registrationPage.SelectLanguageOfSiteFromDropDown(language);
		}

		[When(@"User clicks registration button")]
		public void WhenUserClicksRegistrationButton()
		{
            _registrationPage.SignUpButton.Click();
		}

		[When(@"Enters generated username to username input on registration page")]
		public void WhenEntersGeneratedUsernameToUsernameInput()
		{
			_registrationPage.UsernameInput.SendKeys(_user.Value.Username);
		}

		[When(@"Enters ""(.*)"" to username input on registration page")]
		public void WhenEntersToUsernameInput(string username)
		{
			_registrationPage.UsernameInput.SendKeys(username);
		}

		[When(@"Enters generated email to email input on registration page")]
		public void WhenEntersGeneratedEmailToEmailInput()
		{
            _registrationPage.EmailInput.SendKeys(_user.Value.Email);
		}

		[When(@"Enters ""(.*)"" to email input on registration page")]
		public void WhenEntersToEmailInput(string email)
		{
			_registrationPage.EmailInput.SendKeys(email);
		}

		[When(@"Sets site language to ""(.*)"" on registration page")]
		public void WhenSetsLanguage(string language)
		{
            _registrationPage.SelectLanguageFromDropDown(language);
		}

		[When(@"Sets timezone to ""(.*)"" on registration page")]
		public void WhenSetsTimezone(string timezone)
		{
            _registrationPage.SelectTimezoneFromDropDown(timezone);
		}

		[When(@"Enters generated password to password input on registration page")]
		public void WhenEntersGeneratedPasswordToPasswordInput()
		{
			_registrationPage.PasswordRegistrationInput.SendKeys(_user.Value.Password);
		}

		[When(@"Enters ""(.*)"" to password input on registration page")]
		public void WhenEntersToPasswordInput(string password)
		{
			_registrationPage.PasswordRegistrationInput.SendKeys(password);
		}

		[When(@"Enters generated password to confirm password input on registration page")]
		public void WhenEntersGeneratedPasswordToConfirmPasswordInput()
		{
			_registrationPage.ConfirmPasswordInput.SendKeys(_user.Value.Password);
		}

		[When(@"Enters ""(.*)"" to password confirm input on registration page")]
		public void WhenEntersToConfirmPasswordInput(string password)
		{
			_registrationPage.ConfirmPasswordInput.SendKeys(password);
		}

		[When(@"Clicks register button on registration page")]
		public void WhenClicksRegistrationButton()
		{
            _registrationPage.RegisterButton.Click();
		}

		[Then(@"User sees profile container with his username")]
		public void ThenLoginFormHasError()
		{
			Assert.DoesNotThrow(() => _registrationPage.UserProfileContainer.FindElement(By.XPath($"//*[contains(text(), '{_user.Value.Username}')]")));
		}

		[Then(@"User is on home page with title ""(.*)""")]
		public void ThenUserIsOnHomePageAfterSuccessfulRegistration(string title)
		{
			Assert.That(_registrationPage.Title, Is.EqualTo(title));
        }

        [Then(@"Registration form has error ""(.*)""")]
        public void ThenRegistrationFormHasPasswordError(string error)
        {
            string errorMessage = string.Empty;
            Assert.DoesNotThrow(() => errorMessage = _registrationPage.RegistrationErrors.Text);
			StringAssert.Contains(error, errorMessage);
        }
    }
}
