using OpenQA.Selenium;
using System;
using System.Configuration;
using System.Globalization;

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

		public const string SelectedCultureByUserKey = "SelectedCulture";

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

		public string GetDate(string culture, bool getPastDate = false)
		{
			var cultureInfo = new CultureInfo(culture, false);
			DateTimeFormatInfo format = cultureInfo.DateTimeFormat;

			if(getPastDate)
				return Convert.ToDateTime(
					DateTime.Today.AddMonths(-2).ToString(cultureInfo), format).
					ToString(format.ShortDatePattern);

			return Convert.ToDateTime(
				DateTime.Today.AddYears(5).ToString(cultureInfo), format).
				ToString(format.ShortDatePattern);
		}

		public string GetTime(string culture)
		{
			var cultureInfo = new CultureInfo(culture, false);
			DateTimeFormatInfo format = cultureInfo.DateTimeFormat;

			return Convert.ToDateTime(
				DateTime.Today.AddHours(12).ToString(cultureInfo), format).
				ToString(format.ShortTimePattern);
		}
	}
}
