using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(TicketManagementMVC.Startup))]

namespace TicketManagementMVC
{
    internal partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
			this.ConfigureAuth(app);
        }
	}
}
