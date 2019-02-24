using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(WcfWebHost.Startup))]

namespace WcfWebHost
{
    internal partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
			this.ConfigureHangfire(app);
		}
    }
}
