using System;
using WcfWebHost.App_Start;

namespace WcfWebHost
{
	public class Global : System.Web.HttpApplication
	{
        protected void Application_Start(object sender, EventArgs e)
        {
            AutofacContainer.Config();
		}
	}
}