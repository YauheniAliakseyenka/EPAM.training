using Hangfire;
using Hangfire.Dashboard;
using Owin;
using System.Configuration;

namespace WcfWebHost
{
    internal partial class Startup
    {
        public void ConfigureHangfire(IAppBuilder app)
        {
            GlobalConfiguration.Configuration.UseSqlServerStorage(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            app.UseHangfireServer();
            app.UseHangfireDashboard("/hangfire");
        }
	}
}