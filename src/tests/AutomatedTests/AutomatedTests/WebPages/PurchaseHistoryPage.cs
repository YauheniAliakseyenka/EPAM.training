using OpenQA.Selenium;
using System.Configuration;

namespace AutomatedTests.WebPages
{
	public class PurchaseHistoryPage : AbstractWebPage
	{
		public PurchaseHistoryPage(IWebDriver driver) : base(driver) { }

		public IWebElement OrderTable => FindByXPath("//div[@class='row']//table[contains(@class, 'table')]");

		public void Open()
		{
			var url = ConfigurationManager.AppSettings["SiteUrl"] + "Account/PurchaseHistory";
			base.Open(url);
		}
	}
}
