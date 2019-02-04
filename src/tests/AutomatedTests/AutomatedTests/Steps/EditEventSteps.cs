using AutomatedTests.Utils;
using AutomatedTests.WebPages;
using NUnit.Framework;
using OpenQA.Selenium;
using TechTalk.SpecFlow;

namespace AutomatedTests.Steps
{
	[Binding]
	public class EditEventSteps
	{
		private static EditEventPage _page => PageFactory.Instance.GetPage<EditEventPage>();

		[When(@"Goes to edit event page")]
		public void WhenUserGoesToEditEventPage()
		{
			_page.GoToEventEditPage();
		}

		[When(@"Selects venue ""(.*)""")]
		public void WhenUserSelectsVenueToSortEvents(string venue)
		{
			_page.SelectVenueFromDropDownToSort(venue);
		}

		[When(@"Selects event ""(.*)"" to edit on edit event page")]
		public void WhenUserSelectsEventToEdit(string title)
		{
			_page.SelectEventFromDropDown(title);
		}

		[When(@"Enters ""(.*)"" to title input on edit event page")]
		public void WhenUserEntersEventTitle(string title)
		{
			_page.TitleInput.Clear();
			_page.TitleInput.SendKeys(title);
		}

		[When(@"Enters ""(.*)"" to description input on edit event page")]
		public void WhenUserEntersEventDescription(string description)
		{
			_page.DescriptionInput.Clear();
			_page.DescriptionInput.SendKeys(description);
		}

		[When(@"Selects area to edit")]
		public void WhenUserSelectsArea()
		{
			_page.AreaTable.FindElement(By.XPath("//*[contains(@onclick, 'GetAreaToEdit')]")).Click();
		}

		[When(@"Enters ""(.*)"" to area description input")]
		public void WhenUserEntersAreaDescription(string description)
		{
			_page.AreaDescriptionInput.Clear();
			_page.AreaDescriptionInput.SendKeys(description);
		}

		[When(@"Enters ""(.*)"" to area price input")]
		public void WhenUserEntersAreaPrice(string price)
		{
			_page.AreaPriceInput.Clear();
			_page.AreaPriceInput.SendKeys(price);
		}

		[When(@"Adds seat to area")]
		public void WhenUserAddsSeat()
		{
			_page.AddSeatToTable();
		}

		[When(@"Clicks save area button on area form")]
		public void WhenUserSaveArea()
		{
			_page.SaveAreaButton.Click();
		}

		[When(@"Clicks save button on edit event page")]
		public void WhenUserSaveEvent()
		{
			_page.SaveButton.Click();
		}

		[When(@"Changes venue of event on ""(.*)""")]
		public void WhenUserChanchesEventVenue(string venue)
		{
			_page.SelectVenueFromDropDown(venue);
		}

		[When(@"Clicks delete event button on edit event page")]
		public void WhenUserClicksDeleteButton()
		{
			_page.DeleteEventButton.Click();
			_page.AcceptAlert();
		}

		[Then(@"User can see message ""(.*)"" on edit event page")]
		public void WhenUserSavedEventSuccessfully(string extectedMessage)
		{
			string message = string.Empty;
			Assert.DoesNotThrow(() => message = _page.SuccessNotisfaction.Text);
			StringAssert.Contains(extectedMessage, message);
		}

		[Then(@"Edit event form has error ""(.*)""")]
		public void ThenFormHasError(string expectedError)
		{
			string error = string.Empty;
			Assert.DoesNotThrow(() => error = _page.EventErrors.Text);
			StringAssert.Contains(expectedError, error);
		}

		[Then(@"User can see message ""(.*)"" on area form")]
		public void WhenUserSaveAreaSuccessfully(string extectedMessage)
		{
			string message = string.Empty;
			Assert.DoesNotThrow(() => message = _page.SuccessSaveArea.Text);
			StringAssert.Contains(extectedMessage, message);
		}

		[Then(@"Event is not represent in a system anymore")]
		public void WhenUserSuccessfullyDeleteEvent()
		{
			Assert.Throws<WebDriverTimeoutException>(
				() => _page.EventIsNotRepresentInSystem("Royal Albert Hall","test event"));
		}
	}
}
