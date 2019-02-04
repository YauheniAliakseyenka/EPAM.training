using AutomatedTests.Utils;
using AutomatedTests.WebPages;
using NUnit.Framework;
using System;
using System.Globalization;
using TechTalk.SpecFlow;

namespace AutomatedTests.Steps
{
    [Binding]
    public class CreateEventSteps
    {
        private static CreateEventPage _page => PageFactory.Instance.GetPage<CreateEventPage>();

		private string _culture;
        private string _date
		{
			get
			{
				var cultureInfo = new CultureInfo(_culture, false);
				DateTimeFormatInfo format = cultureInfo.DateTimeFormat;

				return Convert.ToDateTime(DateTime.Today.AddYears(5).ToString(cultureInfo), format).ToString(format.ShortDatePattern);
			}
		}
		private string _time
		{
			get
			{
				var cultureInfo = new CultureInfo(_culture, false);
				DateTimeFormatInfo format = cultureInfo.DateTimeFormat;

				return Convert.ToDateTime(DateTime.Today.AddHours(12).ToString(cultureInfo), format).ToString(format.ShortTimePattern);
			}
		}

		[Given(@"Logged in as an event manager")]
        public void GivenUserOnHomePage()
        {
			var manager = UserFactory.GetEventManager();
			_page.SignIn(manager.Username, manager.Password);
        }

        [When(@"User sets ""(.*)"" language for the site")]
        public void WhenEventManagerSetsLanguageOfSite(string language)
        {
			_culture = _page.SelectLanguageOfSiteFromDropDown(language);
		}

        [When(@"Selects Create event from dropdown menu on create event page")]
        public void WhenSelectsCreateEvent()
        {
            _page.SelectCreateEventFromDropDownMenu();
        }

        [When(@"Enters ""(.*)"" to title input on create event page")]
        public void WhenEntersTitleToTitleInput(string title)
        {
            _page.TitleInput.SendKeys(title);

		}

        [When(@"Selects ""(.*)"" from venue dropdown list on create event page")]
        public void WhenSelectsVenueFromDropdown(string venue)
        {
            _page.SelectVenueFromDropDown(venue);
        }

        [When(@"Selects ""(.*)"" from layout dropdown list on create event page")]
        public void WhenSelectsLayoutFromDropdown(string layout)
        {
            _page.SelectLayoutFromDropDown(layout);
        }

        [When(@"Enters a date of event on create event page")]
        public void WhenEntersDate()
        {
            _page.DatePicker.Clear();
			_page.DatePicker.SendKeys(_date);
        }

        [When(@"Enters a past date of event on create event page")]
        public void WhenEntersPastDate()
        {
			var cultureInfo = new CultureInfo(_culture, false);
			DateTimeFormatInfo format = cultureInfo.DateTimeFormat;
			var pastDate = Convert.ToDateTime(DateTime.Today.AddMonths(-2).ToString(cultureInfo), format).ToString(format.ShortDatePattern);

			_page.DatePicker.Clear();
            _page.DatePicker.SendKeys(pastDate);
        }

        [When(@"Enters a time of event on create event page")]
        public void WhenEntersTime()
        {
            _page.TimePicker.Clear();
			_page.TimePicker.SendKeys(_time);
        }

        [When(@"Enters ""(.*)"" to description input on create event page")]
        public void WhenEntersDescriptionToDescriptionInput(string description)
        {
            _page.DescriptionInput.SendKeys(description);
        }

        [When(@"Confirms creating of event on create event page")]
        public void WhenCreateEvent()
        {
            _page.CreateButton.Click();
        }

		[Then(@"User can see message ""(.*)"" on create event page")]
        public void ThenEventWasCreated(string message)
        {
			string notisfaction = string.Empty;
			Assert.DoesNotThrow(() => notisfaction = _page.SuccessNotisfaction.Text);
			StringAssert.Contains(message, notisfaction);
		}

        [Then(@"Create event form has error ""(.*)""")]
        public void ThenFormHasError(string error)
        {
            string message = string.Empty;
            Assert.DoesNotThrow(() => message = _page.Errors.Text);
            StringAssert.Contains(message, error);
        }
    }
}
