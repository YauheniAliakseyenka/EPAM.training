using AutomatedTests.Utils;
using AutomatedTests.WebPages;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace AutomatedTests.Steps
{
    [Binding]
    public class CreateEventSteps
    {
        private static CreateEventPage CreateEventPage => PageFactory.Instance.GetPage<CreateEventPage>();
	
		[Given(@"Logged in as an event manager")]
        public void GivenUserOnHomePage()
        {
			var manager = UserFactory.GetEventManager();
			CreateEventPage.SignIn(manager.Username, manager.Password);
        }

        [When(@"User sets ""(.*)"" language for the site")]
        public void WhenEventManagerSetsLanguageOfSite(string language)
        {
			ScenarioContext.Current.Add(CreateEventPage.SelectedCultureByUserKey, 
				CreateEventPage.SelectLanguageOfSiteFromDropDown(language));
		}

        [When(@"Selects Create event from dropdown menu on create event page")]
        public void WhenSelectsCreateEvent()
        {
			CreateEventPage.SelectCreateEventFromDropDownMenu();
        }

        [When(@"Enters ""(.*)"" to title input on create event page")]
        public void WhenEntersTitleToTitleInput(string title)
        {
			CreateEventPage.TitleInput.SendKeys(title);

		}

        [When(@"Selects ""(.*)"" from venue dropdown list on create event page")]
        public void WhenSelectsVenueFromDropdown(string venue)
        {
			CreateEventPage.SelectVenueFromDropDown(venue);
        }

        [When(@"Selects ""(.*)"" from layout dropdown list on create event page")]
        public void WhenSelectsLayoutFromDropdown(string layout)
        {
			CreateEventPage.SelectLayoutFromDropDown(layout);
        }

        [When(@"Enters a date of event on create event page")]
        public void WhenEntersDate()
        {
			CreateEventPage.DatePicker.Clear();
			CreateEventPage.DatePicker.SendKeys(
				CreateEventPage.GetDate(ScenarioContext.Current[CreateEventPage.SelectedCultureByUserKey].ToString()));
        }

        [When(@"Enters a past date of event on create event page")]
        public void WhenEntersPastDate()
        {
			CreateEventPage.DatePicker.Clear();
			CreateEventPage.DatePicker.SendKeys(
				CreateEventPage.GetDate(ScenarioContext.Current[CreateEventPage.SelectedCultureByUserKey].ToString(), true));
        }

        [When(@"Enters a time of event on create event page")]
        public void WhenEntersTime()
        {
			CreateEventPage.TimePicker.Clear();
			CreateEventPage.TimePicker.SendKeys(
				CreateEventPage.GetTime(ScenarioContext.Current[CreateEventPage.SelectedCultureByUserKey].ToString()));
        }

        [When(@"Enters ""(.*)"" to description input on create event page")]
        public void WhenEntersDescriptionToDescriptionInput(string description)
        {
			CreateEventPage.DescriptionInput.SendKeys(description);
        }

        [When(@"Confirms creating of event on create event page")]
        public void WhenCreateEvent()
        {
			CreateEventPage.CreateButton.Click();
        }

		[Then(@"User can see message ""(.*)"" on create event page")]
        public void ThenEventWasCreated(string message)
        {
			string notisfaction = string.Empty;
			Assert.DoesNotThrow(() => notisfaction = CreateEventPage.SuccessNotisfaction.Text);
			StringAssert.Contains(message, notisfaction);
		}

        [Then(@"Create event form has error ""(.*)""")]
        public void ThenFormHasError(string error)
        {
            string message = string.Empty;
            Assert.DoesNotThrow(() => message = CreateEventPage.Errors.Text);
            StringAssert.Contains(message, error);
        }
    }
}
