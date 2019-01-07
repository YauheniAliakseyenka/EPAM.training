using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using Hangfire;
using Hangfire.Dashboard;
using Microsoft.Owin;
using Owin;
using TicketManagementMVC.App_Start;
using TicketManagementMVC.Infrastructure.Attributes;

[assembly: OwinStartup(typeof(TicketManagementMVC.Startup))]

namespace TicketManagementMVC
{
    internal partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
			this.Configure(app);

			GlobalConfiguration.Configuration.UseAutofacActivator(HangFireContainer.GetContainer());
			GlobalConfiguration.Configuration.UseSqlServerStorage(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
			app.UseHangfireServer();
			var options = new DashboardOptions
			{
				Authorization = new[] { new DashBoardAuthorizationFilter() }
			};
			app.UseHangfireDashboard("/hangfire", options);
        }
	}
}
