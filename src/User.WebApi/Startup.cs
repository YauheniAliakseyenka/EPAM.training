using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
               AddControllersAsServices().
               AddJsonOptions(options =>
               {
                   options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                   options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
               });

            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new Info
                {
                    Title = "User WebAPI",
                    Version = "v1"
                });
                x.AddSecurityDefinition(
                    "Bearer",
                    new ApiKeyScheme
                    {
                        In = "header",
                        Description = "Please enter JWT with Bearer into field",
                        Name = "Authorization",
                        Type = "apiKey"
                    });
                x.AddSecurityRequirement(
                    new Dictionary<string, IEnumerable<string>> {
                        {
                            "Bearer",
                            Enumerable.Empty<string>()
                        },
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

				//set swagger page as home page
				x.RoutePrefix = string.Empty;
            });

            app.UseAuthentication();
			app.UseHttpsRedirection();
			app.UseMvc();
		}
	}
}
