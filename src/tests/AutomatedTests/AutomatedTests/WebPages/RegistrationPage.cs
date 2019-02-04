using AutomatedTests.WebPages;
using OpenQA.Selenium;

namespace AutomatedTests.Configuration.WebPages
{
    public class RegistrationPage : AbstractWebPage
    {
        public RegistrationPage(IWebDriver driver) : base(driver) { }

        public IWebElement UsernameInput => FindByXPath("//input[contains(@id,'UserName')]");
        public IWebElement EmailInput => FindByXPath("//input[contains(@id,'Email')]");
        public IWebElement PasswordRegistrationInput => FindByXPath("//input[contains(@id,'Password')]");
        public IWebElement ConfirmPasswordInput => FindByXPath("//input[contains(@id,'ConfirmPassword')]");
        public IWebElement RegisterButton => FindByCss(".register-button");
        public IWebElement SignUpButton => FindById("SignUp");
        public IWebElement UserProfileContainer => FindByCss(".profile-container");
        public string Title => Driver.Title;
		public IWebElement RegistrationErrors => FindById("errorMessagesRegistration");
        public IWebElement UserMenu => FindByCss(".profile-right");

		//select language on registration of user profile form
		public void SelectLanguageFromDropDown(string language)
		{
			SelectFromDropDownByText(By.Id("Culture"), language);
		}

		public void SelectTimezoneFromDropDown(string timezone)
		{
			var select = FindByXPath($"//select[contains(@class,'select-timezone')]//option[contains(@value, '{timezone}')]");
			select.Click();
		}

		//select site's language from footer
		public void SelectLanguageOfSiteFromDropDown(string language)
		{
			SelectFromDropDownByText(By.Id("footerCulture"), language);
		}

		public void SignOut()
        {
			HelperMethods.SignOut();
        }
    }
}
