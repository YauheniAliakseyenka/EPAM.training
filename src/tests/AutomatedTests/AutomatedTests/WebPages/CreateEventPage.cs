using OpenQA.Selenium;
using System.Configuration;

namespace AutomatedTests.WebPages
{
    public class CreateEventPage : AbstractWebPage
    {
		public CreateEventPage(IWebDriver driver) : base(driver) { }

        public IWebElement TitleInput => FindById("Title");
        public IWebElement DatePicker => FindById("datepicker");
        public IWebElement TimePicker => FindById("timepicker");
        public IWebElement DescriptionInput => FindById("description");
        public IWebElement CreateButton => FindByCss(".register-button");
		public IWebElement SuccessNotisfaction => FindById("success");
        public IWebElement Errors => FindById("errorMessagesCreateEvent");

        public void SelectVenueFromDropDown(string venue)
        {
			SelectFromDropDownByText(By.Id("venueList"), venue);
		}

        public void SelectLayoutFromDropDown(string layout)
        {
			SelectFromDropDownByText(By.Id("layoutList"), layout);
		}

        public void SelectCreateEventFromDropDownMenu()
        {
			Open(ConfigurationManager.AppSettings["SiteUrl"] + "Event/Create");
        }

        public void SignIn(string username, string password)
        {
			HelperMethods.SignIn(username, password);
        }

		public void SignOut()
		{
			HelperMethods.SignOut();
		}

		public string SelectLanguageOfSiteFromDropDown(string language)
		{
			var dropdown = SelectFromDropDownByText(By.Id("footerCulture"), language);
			var culture = dropdown.GetAttribute("value");

			return culture;
		}
    }
}
