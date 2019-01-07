using Hangfire.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TicketManagementMVC.Infrastructure.Attributes
{
	internal class DashBoardAuthorizationFilter : IDashboardAuthorizationFilter
	{
		public bool Authorize(DashboardContext context)
		{
			if (HttpContext.Current.User.IsInRole("User"))
			{
				return true;
			}

			return false;
		}
	}
}