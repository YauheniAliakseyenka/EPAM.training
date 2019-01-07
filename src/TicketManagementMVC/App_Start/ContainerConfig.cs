using Autofac;
using Autofac.Integration.Mvc;
using BusinessLogic.DTO;
using BusinessLogic.Services;
using BusinessLogic.Services.EventServices;
using BusinessLogic.Services.UserServices;
using DataAccess;
using Hangfire;
using Microsoft.AspNet.Identity;
using System.Configuration;
using System.Reflection;
using System.Web.Mvc;
using TicketManagementMVC.Infrastructure.Authentication;

namespace TicketManagementMVC.App_Start
{
	internal class ContainerConfig
	{
		public static void Config()
		{
			var builder = new ContainerBuilder();
			builder.RegisterControllers(Assembly.GetExecutingAssembly());
			builder.RegisterType<WorkUnit>().As<IWorkUnit>().WithParameter("connectionString",
				ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

			builder.RegisterType<UserStore>().As<IUserStore<User, string>>();
			builder.RegisterType<UserManager<User, string>>().AsSelf();

			//services
			builder.RegisterType<EventService>().As<IEventService>();
			builder.RegisterType<EventAreaService>().As<IStoreService<EventAreaDto, int>>();
			builder.RegisterType<EventSeatService>().As<IStoreService<EventSeatDto, int>>();
			builder.RegisterType<UserService>().As<IUserService>();
			builder.RegisterType<VenueService>().As<IStoreService<VenueDto, int>>();
			builder.RegisterType<LayoutService>().As<IStoreService<LayoutDto, int>>();
			builder.RegisterType<AreaService>().As<IStoreService<AreaDto, int>>();
			builder.RegisterType<SeatService>().As<IStoreService<SeatDto, int>>();
			builder.RegisterType<CartService>().As<ICartService>();
			builder.RegisterType<OrderService>().As<IOrderService>();
			builder.RegisterType<EmailService>().As<IEmailService>();

			var container = builder.Build();			
			DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

			//set up hangfire resolver
			GlobalConfiguration.Configuration.UseAutofacActivator(container);
		}
	}
}