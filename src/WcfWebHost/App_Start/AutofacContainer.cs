using Autofac;
using Autofac.Integration.Wcf;
using BusinessLogic.DiContainer;
using Hangfire;
using System.Configuration;
using WcfBusinessLogic.Core.Contracts.Services;
using WcfBusinessLogic.Core.Services;
using WcfWebHost.Services;
using WcfWebHost.Services.EmailService;
using WcfWebHost.Services.SeatLockerService;

namespace WcfWebHost.App_Start
{
    public static class AutofacContainer
    {
		public static void Config()
		{
			var builder = new ContainerBuilder();

			builder.RegisterModule(new WebModule()
			{
				ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString
			});

            builder.RegisterType<WcfEventService>().As<IWcfEventService>().UseWcfSafeRelease();
			builder.RegisterType<WcfEventAreaService>().As<IWcfEventAreaService>().UseWcfSafeRelease();
            builder.RegisterType<WcfPurchaseService>().As<IWcfPurchaseService>().UseWcfSafeRelease();
			builder.RegisterType<WcfVenueService>().As<IWcfVenueService>().UseWcfSafeRelease();
			builder.RegisterType<WcfLayoutService>().As<IWcfLayoutService>().UseWcfSafeRelease();
			builder.RegisterType<SeatLocker>().As<ISeatLocker>().InstancePerLifetimeScope();
			builder.RegisterType<EmailService>().As<IEmailService>().InstancePerLifetimeScope();

			var container = builder.Build();

            GlobalConfiguration.Configuration.UseAutofacActivator(container, true);
            AutofacHostFactory.Container = container;
        }
    }
}
