using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace TicketManagementMVC.App_Start
{
	internal class BundleConfig
	{
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/bundles/eventmanager").
				Include("~/Scripts/jquery-{version}.js").
				Include("~/Scripts/eventmanager.workflow.min.js"));

			bundles.Add(new ScriptBundle("~/bundles/scripts").
				Include("~/Scripts/bootstrap.min.js").
				Include("~/Scripts/bootstrap-notify.min.js").
				Include("~/Scripts/cart.workflow.min.js").
				Include("~/Scripts/menu-navigation.min.js").
				Include("~/Scripts/jquery.timepicker.min.js").
				Include("~/Scripts/jquery-ui.min.js").
				Include("~/Scripts/datepicker-ru.js").
				Include("~/Scripts/datepicker-be.js"));

			bundles.Add(new StyleBundle("~/content/css").
				Include("~/Content/bootstrap.min.css").
				Include("~/Content/Site.min.css").
				Include("~/Content/jquery.timepicker.min.css").
				Include("~/Content/jquery-ui.min.css"));
		}
	}
}