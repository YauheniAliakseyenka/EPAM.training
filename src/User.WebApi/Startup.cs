using System;
using System.IO;
using System.Reflection;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using User.WebApi.App_Start;

namespace User.WebApi
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IContainer ApplicationContainer { get; private set; }
		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public IServiceProvider ConfigureServices(IServiceCollection services)
		{
			services.AddMvcCore().
			   AddJsonFormatters().
			   AddAuthorization().
			   AddFormatterMappings().
               AddApiExplorer().
			   SetCompatibilityVersion(CompatibilityVersion.Version_2_2).
			   AddControllersAsServices();
            
            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new Info
                {
                    Title = "User WebAPI",
                    Version = "v1"
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                x.IncludeXmlComments(xmlPath);
            });

			AuthConfig.Config(services, this.Configuration);
			this.ApplicationContainer = AutofacConfig.Config(services, this.Configuration);

			return ApplicationContainer.Resolve<IServiceProvider>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
            app.UseSwagger();
            app.UseStaticFiles();
            app.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint("/swagger/v1/swagger.json", "User WebAPI V1");
            });

            app.UseAuthentication();
			app.UseHttpsRedirection();
			app.UseMvc();
		}
	}
}
