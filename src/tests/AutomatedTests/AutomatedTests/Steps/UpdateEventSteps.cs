using AutomatedTests.Utils;
using AutomatedTests.WebPages;
using NUnit.Framework;
using OpenQA.Selenium;
using TechTalk.SpecFlow;

namespace AutomatedTests.Steps
{
	[Binding]
	public class UpdateEventSteps
	{
		private static UpdateEventPage EditEventPage => PageFactory.Instance.GetPage<UpdateEventPage>();

		[When(@"Goes to edit event page")]
		public void WhenUserGoesToEditEventPage()
		{
			EditEventPage.GoToEventEditPage();
		}

		[When(@"Selects venue ""(.*)""")]
		public void WhenUserSelectsVenueToSortEvents(string venue)
		{
			EditEventPage.SelectVenueFromDropDownToSort(venue);
		}

		[When(@"Selects event ""(.*)"" to edit on edit event page")]
		public void WhenUserSelectsEventToEdit(string title)
		{
			EditEventPage.SelectEventFromDropDown(title);
		}

		[When(@"Enters ""(.*)"" to title input on edit event page")]
		public void WhenUserEntersEventTitle(string title)
		{
			EditEventPage.TitleInput.Clear();
			EditEventPage.TitleInput.SendKeys(title);
		}

		[When(@"Enters ""(.*)"" to description input on edit event page")]
		public void WhenUserEntersEventDescription(string description)
		{
			EditEventPage.DescriptionInput.Clear();
			EditEventPage.DescriptionInput.SendKeys(description);
		}

		[When(@"Selects area to edit")]
		public void WhenUserSelectsArea()
		{
			EditEventPage.AreaTable.FindElement(By.XPath("//*[contains(@onclick, 'GetAreaToEdit')]")).Click();
		}

		[When(@"Enters ""(.*)"" to area description input")]
		public void WhenUserEntersAreaDescription(string description)
		{
			EditEventPage.AreaDescriptionInput.Clear();
			EditEventPage.AreaDescriptionInput.SendKeys(description);
		}

		[When(@"Enters ""(.*)"" to area price input")]
		public void WhenUserEntersAreaPrice(string price)
		{
			EditEventPage.AreaPriceInput.Clear();
			EditEventPage.AreaPriceInput.SendKeys(price);
		}

		[When(@"Adds seat to area")]
		public void WhenUserAddsSeat()
		{
			EditEventPage.AddSeatToTable();
		}

		[When(@"Clicks save area button on area form")]
		public void WhenUserSaveArea()
		{
			EditEventPage.SaveAreaButton.Click();
		}

		[When(@"Clicks save button on edit event page")]
		public void WhenUserSaveEvent()
		{
			EditEventPage.SaveButton.Click();
		}

		[When(@"Changes venue of event on ""(.*)""")]
		public void WhenUserChanchesEventVenue(string venue)
		{
			EditEventPage.SelectVenueFromDropDown(venue);
		}

		[When(@"Clicks delete event button on edit event page")]
		public void WhenUserClicksDeleteButton()
		{
			EditEventPage.DeleteEventButton.Click();
			EditEventPage.AcceptAlert();
		}

		[Then(@"User can see message ""(.*)"" on edit event page")]
		public void WhenUserSavedEventSuccessfully(string extectedMessage)
		{
			string message = string.Empty;
			Assert.DoesNotThrow(() => message = EditEventPage.SuccessNotisfaction.Text);
			StringAssert.Contains(extectedMessage, message);
		}

		[Then(@"Edit event form has error ""(.*)""")]
		public void ThenFormHasError(string expectedError)
		{
			string error = string.Empty;
			Assert.DoesNotThrow(() => error = EditEventPage.EventErrors.Text);
			StringAssert.Contains(expectedError, error);
		}

		[Then(@"User can see message ""(.*)"" on area form")]
		public void WhenUserSaveAreaSuccessfully(string extectedMessage)
		{
			string message = string.Empty;
			Assert.DoesNotThrow(() => message = EditEventPage.SuccessSaveArea.Text);
			StringAssert.Contains(extectedMessage, message);
		}

		[Then(@"Event is not represent in a system anymore")]
		public void WhenUserSuccessfullyDeleteEvent()
		{
			Assert.Throws<WebDriverTimeoutException>(
				() => EditEventPage.EventIsNotRepresentInSystem("Royal Albert Hall","test event"));
		}
	}
}
