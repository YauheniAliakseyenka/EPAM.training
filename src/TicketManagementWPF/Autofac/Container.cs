using Autofac;
using Autofac.Integration.Wcf;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using TicketManagementWPF.Helpers.WindowManagement;
using TicketManagementWPF.Infrastructure;
using TicketManagementWPF.Infrastructure.Utils.Identity;
using TicketManagementWPF.Infrastructure.Utils.UserManagement;
using TicketManagementWPF.VenueService;
using TicketManagementWPF.ViewModels;
using User.WebApi.Helper;

namespace TicketManagementWPF.Autofac
{
	public static class AutofacContainer
	{
		private static IContainer _instance;
        public static T Resolve<T>() where T : class => _instance.Resolve<T>();

        public static void Config()
		{
			var builder = new ContainerBuilder();

            #region View models

            builder.RegisterType<LoginViewModel>().AsSelf().InstancePerDependency();
            builder.RegisterType<MainViewModel>().AsSelf().InstancePerDependency();
            builder.RegisterType<MainMenuViewModel>().AsSelf().InstancePerDependency();
            builder.RegisterType<VenueManagementViewModel>().AsSelf().InstancePerDependency();
            builder.RegisterType<VenueViewModel>().AsSelf().InstancePerDependency();
            builder.RegisterType<LayoutMapViewModel>().AsSelf().InstancePerDependency();
            builder.RegisterType<AreaMapViewModel>().AsSelf().InstancePerDependency();
            builder.RegisterType<ProfileViewModel>().AsSelf().InstancePerDependency();
            builder.RegisterType<SeatMapViewModel>().AsSelf().InstancePerDependency();
            builder.RegisterType<UserViewModel>().AsSelf().InstancePerDependency();
            builder.RegisterType<UserManagementViewModel>().AsSelf().InstancePerDependency();

            #endregion
            
            builder.RegisterType<WindowHelper>().As<IWindowHelper>().InstancePerDependency();
            builder.RegisterType<UserIdentity>().As<IUserIdentity>().InstancePerDependency();
			builder.RegisterType<UserManagement>().As<IUserManagement>().
                                                   As<IUserProfile>().
                                                   As<IAuth>().InstancePerDependency();
			builder.RegisterType<Mediator>().As<IMediator>().SingleInstance();
			builder.RegisterType<UserWebApiHelper>().As<IUserWebApiHelper>().WithParameter("defaultUri",
					  ConfigurationManager.AppSettings["UserWebApiBaseAddress"]).InstancePerLifetimeScope();
			

			var wcfServicesCredentials = AuthSettings.Settings.Credentials["WcfServices"];
            builder.Register(x => new ChannelFactory<IWcfVenueService>("WSHttpBinding_IWcfVenueService")).SingleInstance();
            builder.Register(x =>
            {
                var factory = x.Resolve<ChannelFactory<IWcfVenueService>>();
                var clientCredentials = (ClientCredentials)factory.Endpoint.EndpointBehaviors.SingleOrDefault(y => y.GetType() == typeof(ClientCredentials));
                clientCredentials.UserName.UserName = wcfServicesCredentials.Username;
                clientCredentials.UserName.Password = wcfServicesCredentials.Password;
                return factory.CreateChannel();
            }).As<IWcfVenueService>().UseWcfSafeRelease();

			_instance = builder.Build();
        }
	}
}
