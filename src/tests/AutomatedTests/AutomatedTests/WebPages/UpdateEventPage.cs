using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Configuration;
using System.Linq;

namespace AutomatedTests.WebPages
{
	public class UpdateEventPage : AbstractWebPage
	{
		public UpdateEventPage(IWebDriver driver) : base(driver) { }

		public IWebElement TitleInput => FindById("title");
		public IWebElement DatePicker => FindById("datepicker");
		public IWebElement TimePicker => FindById("timepicker");
		public IWebElement DescriptionInput => FindById("description");
		public IWebElement SaveButton => FindByXPath("//*[contains(@onclick,'SaveEventChanges')]");
		public IWebElement AddAreaButton => FindByXPath("//*[contains(@onclick,'CreateArea')]");
		public IWebElement SuccessNotisfaction => FindById("success");
		public IWebElement AreaTable => FindById("areaTable");
		public IWebElement AreaDescriptionInput=> FindById("Description");
		public IWebElement CoordXInput => FindById("CoordX");
		public IWebElement CoordYInput => FindById("CoordY");
		public IWebElement SeatTable => FindById("seatTable");
		public IWebElement AreaPriceInput => FindById("Price");
		public IWebElement EventErrors => FindById("errorMessagesEditEvent");
		public IWebElement SuccessSaveArea => FindById("successSaveArea");
		public IWebElement SaveAreaButton => FindByXPath("//*[contains(@onclick,'SaveArea')]");
		public IWebElement AddSeatButton => FindByXPath("//*[contains(@onclick,'AddSeatToTable')]");
		public IWebElement DeleteEventButton => FindByXPath("//*[contains(@onclick, 'DeleteEvent')]");
		public IWebElement CloseEventAreaButton => FindByXPath("//*[contains(@onclick,'CloseEventAreaForm')]");

		public void SelectVenueFromDropDownToSort(string venue)
		{
			SelectFromDropDownByText(By.Id("venueListToSortEvents"), venue);
		}

		public void SelectEventFromDropDown(string title)
		{
			SelectFromDropDownByText(By.Id("eventListOnEdit"), title);

			new WebDriverWait(Driver, TimeSpan.FromSeconds(4)).Until<bool>((d) =>
			{
				if (TitleInput.GetAttribute("value").Equals(title, StringComparison.Ordinal))
					return true;

				return false;
			});
		}

		public void EventIsNotRepresentInSystem(string venue, string title)
		{
			GoToEventEditPage();
			SelectVenueFromDropDownToSort(venue);

			var xPath = $"//*[@id='eventListOnEdit']//option[contains(text(), '{title}')]";
			FindByXPath(xPath).Click();
		}

		public void SelectLayoutFromDropDown(string layout)
		{
			SelectFromDropDownByText(By.Id("layoutList"), layout);
		}

		public void SelectVenueFromDropDown(string venue)
		{
			SelectFromDropDownByText(By.Id("venueList"), venue);
		}

		public void AcceptAlert()
		{
			Driver.SwitchTo().Alert().Accept();
		}

		public void GoToEventEditPage()
		{
			Open(ConfigurationManager.AppSettings["SiteUrl"] + "Event/Edit");
		}

		public void AddSeatToTable()
		{
			AddSeatButton.Click();
			var lastRow = SeatTable.FindElement(By.XPath("//tbody//tr[position()=last()-1]//input[contains(@id,'Row')]"));
			var lastRowNumber = int.Parse(lastRow.GetAttribute("value"));
			var inputs = SeatTable.FindElements(By.XPath
				("//tbody//tr[position()=last()]//input[contains(@name,'Row') or contains(@name,'Number')]")).ToList();
			inputs.ForEach(x =>
			{
				var nextRow = lastRowNumber + 1;
				x.SendKeys(nextRow.ToString());
			});
		}
	}
}
