using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TicketManagementMVC.Infrastructure.Authentication
{
	public class ApplicationUserManager: UserManager<User, string>
	{
		public ApplicationUserManager(IUserStore<User, string> store)
			: base(store)
		{
			configurate(this);
		}

		private static void configurate(UserManager<User, string> manager)
		{
			manager.UserValidator = new UserValidator<User>(manager)
			{
				AllowOnlyAlphanumericUserNames = false,
				RequireUniqueEmail = true
			};
		}
	}
}