using Autofac;
using Hangfire;
using Hangfire.Dashboard;
using Owin;
using System.Configuration;
using System.Web;

namespace TicketManagementMVC
{
    internal partial class Startup
    {
        public void ConfigureHangfire(IAppBuilder app)
        {
            GlobalConfiguration.Configuration.UseSqlServerStorage(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            app.UseHangfireServer();
            var options = new DashboardOptions
            {
                Authorization = new[] { new DashBoardAuthorizationFilter() }
            };
            app.UseHangfireDashboard("/hangfire", options);
        }

        internal class DashBoardAuthorizationFilter : IDashboardAuthorizationFilter
        {
            public bool Authorize(DashboardContext context)
            {
                if (HttpContext.Current.User.IsInRole("Event manager"))
                {
                    return true;
                }

                return false;
            }
        }
	}
}