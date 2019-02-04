using OpenQA.Selenium;
using System.Configuration;
using System.Linq;
using System.Threading;

namespace AutomatedTests.WebPages
{
	public class HomePage : AbstractWebPage
	{
		public HomePage(IWebDriver driver) : base(driver) { }

		public IWebElement EventsList => FindByCss(".event-list");
		public IWebElement SeatMap => FindById("seatMap");
		public IWebElement SearchButton => FindByXPath("//*[contains(@value, 'Search')]");
		public IWebElement UserProfileContainer => FindByCss(".profile-left");
		public IWebElement ManageEventsMenu => FindById("eventmanager-dropdown");
		public IWebElement LoginErrorMessages => FindById("errorMessagesLogin");
		public IWebElement FilterInput => FindById("filterInput");
		public IWebElement AccountBalance => FindByCss(".balance");
		public string Title => Driver.Title;
		public string CurrentUrl => Driver.Url;
		
		public void Open()
        {
			var url = ConfigurationManager.AppSettings["SiteUrl"];
			base.Open(url);
		}

		public void GoToUrl(string url)
		{
			HelperMethods.GoToUrl(url);
		}

		public void SignIn(string username, string password)
		{
			HelperMethods.SignIn(username, password);
		}

		public void SelectFilterOption(string option)
		{
			string xPath = $"//select[@id='filterList']//option[contains(@value,'{option}')]";
			FindByXPath(xPath).Click();
		}

		public void LockSeats()
		{
            var seats = FindElementsByXPath("//*[@id='seatMap']//*[contains(@class,'seat-available') and contains(@class, 'seat-seatmap')]").ToList();			
			seats[seats.IndexOf(seats.First())].Click();
			seats[seats.IndexOf(seats.Last())].Click();
			Thread.Sleep(300);
		}

		public void SelectEvent(string title)
		{
			var selectEvent = FindByXPath($"//div[contains(text(),'{title}')]/parent::a");
			GoToUrl(selectEvent.GetAttribute("href"));
		}
	}
}
