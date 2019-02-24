using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;

namespace TicketManagementMVC
{
	internal partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
			app.UseCookieAuthentication(new CookieAuthenticationOptions
			{
				AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
				LoginPath = new PathString("/"),
				ExpireTimeSpan = TimeSpan.FromMinutes(60)
			});
		}
    }
}