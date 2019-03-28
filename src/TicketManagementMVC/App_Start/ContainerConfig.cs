using Autofac;
using Autofac.Integration.Mvc;
using System.Reflection;
using System.ServiceModel;
using System.Web.Mvc;
using TicketManagementMVC.Infrastructure.Authentication;
using TicketManagementMVC.EventService;
using System.Linq;
using Autofac.Integration.Wcf;
using System.ServiceModel.Description;
using TicketManagementMVC.PurchaseService;
using TicketManagementMVC.EventAreaService;
using TicketManagementMVC.VenueService;
using TicketManagementMVC.LayoutService;
using System;
using User.WebApi.Helper;
using System.Configuration;

namespace TicketManagementMVC.App_Start
{
	internal class ContainerConfig
	{
		public static void Config()
		{
			var builder = new ContainerBuilder();
			builder.RegisterControllers(Assembly.GetExecutingAssembly());
			
			builder.RegisterType<CustomUserManager>().AsSelf().InstancePerLifetimeScope();

			var wcfServicesCredentials = AuthSettings.Settings.Credentials["WcfServices"];

			if (wcfServicesCredentials == null)
				throw new NullReferenceException();

            builder.RegisterType<CustomUserManager>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<UserWebApiHelper>().As<IUserWebApiHelper>().WithParameter("defaultUri",
                      ConfigurationManager.AppSettings["UserWebApiBaseAddress"]).InstancePerLifetimeScope();
			builder.Register(x => new ChannelFactory<IWcfEventService>("WSHttpBinding_IWcfEventService")).SingleInstance();
			builder.Register(x =>
			{
				var factory = x.Resolve<ChannelFactory<IWcfEventService>>();
				var clientCredentials = (ClientCredentials)factory.Endpoint.EndpointBehaviors.SingleOrDefault(y => y.GetType() == typeof(ClientCredentials));
				clientCredentials.UserName.UserName = wcfServicesCredentials.Username;
				clientCredentials.UserName.Password = wcfServicesCredentials.Password;
				return factory.CreateChannel();
			}).As<IWcfEventService>().UseWcfSafeRelease();

			builder.Register(x => new ChannelFactory<IWcfPurchaseService>("WSHttpBinding_IWcfPurchaseService")).SingleInstance();
			builder.Register(x =>
			{
				var factory = x.Resolve<ChannelFactory<IWcfPurchaseService>>();
				var clientCredentials = (ClientCredentials)factory.Endpoint.EndpointBehaviors.SingleOrDefault(y => y.GetType() == typeof(ClientCredentials));
				clientCredentials.UserName.UserName = wcfServicesCredentials.Username;
				clientCredentials.UserName.Password = wcfServicesCredentials.Password;
				return factory.CreateChannel();
			}).As<IWcfPurchaseService>().UseWcfSafeRelease();

			builder.Register(x => new ChannelFactory<IWcfEventAreaService>("WSHttpBinding_IWcfEventAreaService")).SingleInstance();
			builder.Register(x =>
			{
				var factory = x.Resolve<ChannelFactory<IWcfEventAreaService>>();
				var clientCredentials = (ClientCredentials)factory.Endpoint.EndpointBehaviors.SingleOrDefault(y => y.GetType() == typeof(ClientCredentials));
				clientCredentials.UserName.UserName = wcfServicesCredentials.Username;
				clientCredentials.UserName.Password = wcfServicesCredentials.Password;
				return factory.CreateChannel();
			}).As<IWcfEventAreaService>().UseWcfSafeRelease();

			builder.Register(x => new ChannelFactory<IWcfVenueService>("WSHttpBinding_IWcfVenueService")).SingleInstance();
			builder.Register(x =>
			{
				var factory = x.Resolve<ChannelFactory<IWcfVenueService>>();
				var clientCredentials = (ClientCredentials)factory.Endpoint.EndpointBehaviors.SingleOrDefault(y => y.GetType() == typeof(ClientCredentials));
				clientCredentials.UserName.UserName = wcfServicesCredentials.Username;
				clientCredentials.UserName.Password = wcfServicesCredentials.Password;
				return factory.CreateChannel();
			}).As<IWcfVenueService>().UseWcfSafeRelease();

			builder.Register(x => new ChannelFactory<IWcfLayoutService>("WSHttpBinding_IWcfLayoutService")).SingleInstance();
			builder.Register(x =>
			{
				var factory = x.Resolve<ChannelFactory<IWcfLayoutService>>();
				var clientCredentials = (ClientCredentials)factory.Endpoint.EndpointBehaviors.SingleOrDefault(y => y.GetType() == typeof(ClientCredentials));
				clientCredentials.UserName.UserName = wcfServicesCredentials.Username;
				clientCredentials.UserName.Password = wcfServicesCredentials.Password;
				return factory.CreateChannel();
			}).As<IWcfLayoutService>().UseWcfSafeRelease();

			var container = builder.Build();			
			DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
		}
	}
}