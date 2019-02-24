using Autofac;
using Autofac.Extensions.DependencyInjection;
using BusinessLogic.DiContainer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using User.WebApi.Infrastructure.AuthManager;

namespace User.WebApi.App_Start
{
	internal class AutofacConfig
	{
		public static IContainer Config(IServiceCollection services, IConfiguration configuration)
		{
			var builder = new ContainerBuilder();

			builder.Populate(services);
			builder.RegisterType<AuthManager>().As<IAuthManager>().InstancePerLifetimeScope();
			builder.RegisterModule(new WebModule()
			{
				ConnectionString = configuration.GetSection("ConnectionStrings:ConnectionString").Value
			});

			return builder.Build();
		}
	}
}
