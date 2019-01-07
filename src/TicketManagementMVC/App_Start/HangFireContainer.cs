using Autofac;
using Autofac.Integration.Mvc;
using BusinessLogic.Services;
using BusinessLogic.Services.EventServices;
using BusinessLogic.Services.UserServices;
using DataAccess;
using DataAccess.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using TicketManagementMVC.Infrastructure.Authentication;

namespace TicketManagementMVC.App_Start
{
	internal class HangFireContainer
	{
		public static IContainer GetContainer()
		{
			var builder = new ContainerBuilder();
			builder.RegisterControllers(Assembly.GetExecutingAssembly());

			builder.RegisterType<WorkUnit>().As<IWorkUnit>().WithParameter("connectionString",
				ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

			builder.RegisterType<UserStore>().As<IUserStore<User, string>>();
			builder.RegisterType<UserManager<User, string>>().AsSelf();

			builder.RegisterType<CartService>().As<ICartService>();
			builder.RegisterType<OrderService>().As<IOrderService>();
			builder.RegisterType<EventService>().As<IEventService>();
			builder.RegisterType<UserService>().As<IUserService>();

			return builder.Build();
		}
	}
}