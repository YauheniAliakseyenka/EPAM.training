using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TicketManagementMVC.App_Start;
using TicketManagementMVC.Infrastructure;
using TicketManagementMVC.Infrastructure.Attributes;
using TicketManagementMVC.Models;

namespace TicketManagementMVC
{
	public class Global : System.Web.HttpApplication
	{
		protected void Application_Start(object sender, EventArgs e)
		{
			AreaRegistration.RegisterAllAreas();
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			ContainerConfig.Config();
			BundleConfig.RegisterBundles(BundleTable.Bundles);
			DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(ValidIntegerAttribute), typeof(ValidIntegerValidator));

			// override default "property value is not valid" error
			ClientDataTypeModelValidatorProvider.ResourceClassKey = "InvalidValueError";
			DefaultModelBinder.ResourceClassKey = "InvalidValueError";
		}
	}
}