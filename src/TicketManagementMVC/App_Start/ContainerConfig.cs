using Autofac;
using Autofac.Integration.Mvc;
using BusinessLogic.DiContainer;
using Hangfire;
using Microsoft.AspNet.Identity;
using System.Configuration;
using System.Reflection;
using System.Web.Mvc;
using TicketManagementMVC.Infrastructure.Authentication;
using TicketManagementMVC.Infrastructure.BackgroundWorker;

namespace TicketManagementMVC.App_Start
{
	internal class ContainerConfig
	{
		public static void Config()
		{
			var builder = new ContainerBuilder();
			builder.RegisterControllers(Assembly.GetExecutingAssembly());

			builder.RegisterType<UserStore>().As<IUserStore<User, string>>().InstancePerLifetimeScope();
			builder.RegisterType<ApplicationUserManager>().AsSelf().InstancePerLifetimeScope();
			builder.RegisterType<SeatLocker>().As<ISeatLocker>().InstancePerLifetimeScope();

			builder.RegisterModule(new WebModule()
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString
            });			

			var container = builder.Build();			
			DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

			//set hangfire resolver
			GlobalConfiguration.Configuration.UseAutofacActivator(container, true);
		}
	}
}