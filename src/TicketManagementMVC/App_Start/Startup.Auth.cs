using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;
using TicketManagementMVC.Infrastructure.Authentication;

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
				Provider = new CookieAuthenticationProvider
				{
					OnValidateIdentity = SecurityStampValidator.
					OnValidateIdentity<ApplicationUserManager, User, int>(
						validateInterval: TimeSpan.FromMinutes(30),
						regenerateIdentityCallback: (manager, user) => manager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie),
						getUserIdCallback: (id) => id.GetUserId<int>())
				}
			});
        }
    }
}