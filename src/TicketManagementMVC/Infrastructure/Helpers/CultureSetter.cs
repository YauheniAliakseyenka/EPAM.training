using System.Web;
using System.Web.Mvc;

namespace TicketManagementMVC.Helpers
{
	internal class CultureSetter
	{
		public static void Set(string culture, Controller controller)
		{
			HttpCookie cookie = controller.Request.Cookies["_culture"];

			if (cookie != null)
				cookie.Value = culture;
			else
			{
				cookie = new HttpCookie("_culture");
				cookie.Value = culture;
			}
			controller.Response.Cookies.Add(cookie);
		}
	}
}