using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TicketManagementMVC.Helpers
{
	internal class DisplayBalance
	{
		public static string Get(decimal amount)
		{
			return string.Format("${0:N2}", amount);
		}
	}
}