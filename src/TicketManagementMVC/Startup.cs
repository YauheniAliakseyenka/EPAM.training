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
			this.ConfigureAuth(app);
            this.ConfigureHangfire(app);
        }
	}
}
