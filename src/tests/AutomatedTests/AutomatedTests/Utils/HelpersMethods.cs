using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace AutomatedTests.Utils
{
	//Almost all of these methods are using to enter pretest data because app have no web api and 
	//I have to do it with a browser
	public class HelpersMethods
	{
		private static IWebDriver _driver;
		private DriverSelectors _selectors;

		public HelpersMethods(IWebDriver driver)
		{
			_driver = driver;
			_selectors = new DriverSelectors(driver);
		}
		
		public void GoToUrl(string url)
		{
            _driver.Navigate().GoToUrl(url);
		}

		public void DeleteTestEvent()
		{
			//go to edit page
			GoToUrl(ConfigurationManager.AppSettings["SiteUrl"] + "Event/Edit");

			//select venue from dropdown
			SelectFromDropDown(By.XPath("//select[@id='venueListToSortEvents']"), "Royal Albert Hall");

			//select event from dropdown
			SelectFromDropDown(By.XPath("//select[@id='eventListOnEdit']"), "test event");

			//wait for loaded event on a form
			new WebDriverWait(_driver, TimeSpan.FromSeconds(3)).Until<bool>((d) =>
			{
				var editAreaButton = FindByXPath("//*[contains(@onclick,'GetAreaToEdit')]");
				if (editAreaButton.Displayed)
					return true;

				return false;
			});

			//click delete button
			var deleteButton = FindByXPath("//*[contains(@onclick, 'DeleteEvent')]");
			deleteButton.Click();

			//accept alert
			CustomConditionals.WaitForAlert(_driver);
			_driver.SwitchTo().Alert().Accept();
		}

		public void CreateTestEvent()
		{
			var description = "test description";
			var title = "test event";

			//select site's language
			var dropdown = SelectFromDropDown(By.XPath("//select[@id='footerCulture']"), "English");
			var culture = dropdown.GetAttribute("value");

			var cultureInfo = new CultureInfo(culture, false);
			DateTimeFormatInfo format = cultureInfo.DateTimeFormat;
			var time = Convert.ToDateTime(DateTime.Today.AddHours(12).ToString(cultureInfo), format).ToString(format.ShortTimePattern);
			var date = Convert.ToDateTime(DateTime.Today.AddYears(5).ToString(cultureInfo), format).ToString(format.ShortDatePattern);

			//select create event menu
			GoToUrl(ConfigurationManager.AppSettings["SiteUrl"] + "Event/Create");

			CustomConditionals.SendedToField(_driver, By.Id("Title"), title);

            //select venue
			SelectFromDropDown(By.XPath("//*[@id='venueList']"), "Royal Albert Hall");

			CustomConditionals.SendedToField(_driver, By.Id("datepicker"), date);

			CustomConditionals.SendedToField(_driver, By.Id("timepicker"), time);

			CustomConditionals.SendedToField(_driver, By.Id("description"), description);

			FindByCss(".register-button").Click();
		}

		public void SetPriceToAreasOfCreatedTestEvent()
		{
            //select site's language
			SelectFromDropDown(By.XPath("//select[@id='footerCulture']"), "English");

			//go to edit page
			GoToUrl(ConfigurationManager.AppSettings["SiteUrl"] + "Event/Edit");

            //select venue from dropdown
			SelectFromDropDown(By.XPath("//select[@id='venueListToSortEvents']"), "Royal Albert Hall");
			Thread.Sleep(300);

			//select event from dropdown
			SelectFromDropDown(By.XPath("//select[@id='eventListOnEdit']"), "test event");

			//wait for loaded event on a form
			new WebDriverWait(_driver, TimeSpan.FromSeconds(3)).Until<bool>((d) =>
			{
				var editAreaButton = FindByXPath("//*[contains(@onclick,'GetAreaToEdit')]"); 
				if (editAreaButton.Displayed)
					return true;

				return false;
			});

            //set price to all areas to publish event
            var areasEditButtonsXPath = "//*[contains(@onclick, 'GetAreaToEdit')]";
            var areas = FindElementsByXpath(areasEditButtonsXPath);
			for (var i = 0; i < areas.Count(); i++)
			{
                var buttons = FindElementsByXpath(areasEditButtonsXPath).ToList();
                buttons[i].Click();
                CustomConditionals.SendedToField(_driver, By.Id("Price"), "5.25");
				CustomConditionals.ClickUntil(_driver, By.XPath("//*[contains(@onclick,'SaveArea')]"), By.Id("successSaveArea"));
				FindByXPath("//*[contains(@onclick,'CloseEventAreaForm')]").Click();
			}
		}

        public void LockSeatOfTestEvent()
        {
			//find event by title
			GoToUrl(ConfigurationManager.AppSettings["SiteUrl"]);
            var filterOption = FindByXPath("//select[@id='filterList']//option[contains(@value,'Title')]");
			filterOption.Click();
			CustomConditionals.SendedToField(_driver, By.Id("filterInput"), "test event");
			FindByXPath("//*[contains(@value, 'Search')]").Click();

			//lock a seat
			var selectEvent = FindByXPath("//div[contains(text(),'test event')]/parent::a");
            GoToUrl(selectEvent.GetAttribute("href"));
			var firstSeat = FindByXPath("//*[contains(@class,'seat-available') and contains(@class, 'seat-seatmap')]");
            firstSeat.Click();
			CustomConditionals.WaitForNotisfaction(_driver);
		}

		public void SignOut()
		{
			GoToUrl(ConfigurationManager.AppSettings["SiteUrl"]);
			var userMenu = FindByCss(".profile-right");
			userMenu.Click();

			var signOutId = "SignOut";
			new WebDriverWait(_driver, TimeSpan.FromSeconds(5)).
				Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id(signOutId)));
			FindById(signOutId).Click();
		}

		public void SignIn(string username, string password)
		{
			FindById("SignIn").Click();

            var usernameInputXpath = "//*[contains(@class,'login-form')]//*[@id='username']";
            CustomConditionals.SendedToField(_driver, By.XPath(usernameInputXpath), username);

            var passwordInputXPath = "//*[contains(@class,'login-form')]//*[@id='password']";
			CustomConditionals.SendedToField(_driver, By.XPath(passwordInputXPath), password);

			var loginButton = FindByXPath("//*[contains(@class, 'login-button')]");
			loginButton.Click();
			try
			{
				CustomConditionals.WaitForLoggedIn(_driver);
			}
			catch (WebDriverTimeoutException) { }
		}

		public void DeleteSeatsFromCart()
		{
			GoToUrl(ConfigurationManager.AppSettings["SiteUrl"] + "Account/Cart");
			var deleteButtons = FindElementsByXpath("//*[contains(@onclick, 'DeleteSeatFromCart')]");
			Thread.Sleep(150);
			deleteButtons.ToList().ForEach(x =>
			{
				x.Click();
				CustomConditionals.WaitForNotisfaction(_driver);
			});
		}

		public void RegisterUser(UserModel user)
		{
            GoToUrl(ConfigurationManager.AppSettings["SiteUrl"] + "Account/Registration");
			
			CustomConditionals.SendedToField(_driver, By.Id("UserName"), user.Username);

			CustomConditionals.SendedToField(_driver, By.Id("Email"), user.Email);

			CustomConditionals.SendedToField(_driver, By.Id("Password"), user.Password);

			CustomConditionals.SendedToField(_driver, By.Id("ConfirmPassword"), user.Password);

			var registerButton = FindByCss(".register-button");
			registerButton.Click();

			CustomConditionals.WaitForLoggedIn(_driver);
			SignOut();
		}

		public void AuthenticatedRefundPurchasedSeat()
		{
			GoToUrl(ConfigurationManager.AppSettings["SiteUrl"] + "Account/PurchaseHistory");
			var refundButtons = FindElementsByXpath("//*[contains(@onclick, 'Refund')]");
			Thread.Sleep(150);
			refundButtons.ToList().ForEach(x =>
			{
				x.Click();
				CustomConditionals.WaitForAlert(_driver);
				_driver.SwitchTo().Alert().Accept();
			});
		}

        private IWebElement SelectFromDropDown(By findBy, string byText)
        {
			return _selectors.SelectFromDropDownByText(findBy, byText);
        }

		private IWebElement FindByXPath(string xpath)
		{
			return _selectors.FindByXPath(xpath);
		}

		private IWebElement FindById(string id)
		{
			return _selectors.FindById(id);
		}

		private IWebElement FindByCss(string css)
		{
			return _selectors.FindByCss(css);
		}

        private IEnumerable<IWebElement> FindElementsByXpath(string xpath)
        {
            return _selectors.FindElementsByXpath(xpath);
        }
	}
}
