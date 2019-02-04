using Autofac;
using Autofac.Integration.Mvc;
using BusinessLogic.DiContainer;
using Hangfire;
using Microsoft.AspNet.Identity;
using System.Configuration;
using System.Reflection;
using System.Web.Mvc;
using TicketManagementMVC.Infrastructure.Authentication;
using TicketManagementMVC.Infrastructure.WebServices.Interfaces;
using TicketManagementMVC.Infrastructure.WebServices.Servcies;

namespace TicketManagementMVC.App_Start
{
	internal class ContainerConfig
	{
		public static void Config()
		{
			var builder = new ContainerBuilder();
			builder.RegisterControllers(Assembly.GetExecutingAssembly());

			builder.RegisterType<UserStore>().As<IUserStore<User, int>>().InstancePerLifetimeScope();
			builder.RegisterType<ApplicationUserManager>().AsSelf().InstancePerLifetimeScope();
			builder.RegisterType<SeatLocker>().As<ISeatLocker>().InstancePerLifetimeScope();
			builder.RegisterType<EmailService>().As<IEmailService>().InstancePerLifetimeScope();

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