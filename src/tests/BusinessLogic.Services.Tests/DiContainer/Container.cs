using Autofac;
using BusinessLogic.DiContainer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services.Tests.DiContainer
{
    internal class Container
    {
        public static IContainer GetContainer()
        {
            var builder = new ContainerBuilder();
			
            builder.RegisterModule(new WebModule()
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["testConnectionString"].ConnectionString
            });

            return builder.Build();
        }
    }
}
