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
			bundles.Add(new ScriptBundle("~/bundles/mainJs").
				Include("~/Scripts/jquery-{version}.js").
				Include("~/Scripts/project-js.min.js").
				Include("~/Scripts/eventmanager.workflow.min.js"));

			bundles.Add(new ScriptBundle("~/bundles/otherJs").
				Include("~/Scripts/jquery.timepicker.min.js").
				Include("~/Scripts/jquery-ui.min.js").
				Include("~/Scripts/datepicker-ru.js").
				Include("~/Scripts/datepicker-be.js").
				Include("~/Scripts/bootstrap.min.js").
				Include("~/Scripts/bootstrap-notify.min.js"));

			bundles.Add(new StyleBundle("~/content/css").
				Include("~/Content/bootstrap.min.css").
				Include("~/Content/Site.min.css").
				Include("~/Content/jquery.timepicker.min.css").
				Include("~/Content/jquery-ui.min.css"));
		}
	}
}