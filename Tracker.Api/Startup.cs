using Autofac;
using Autofac.Extensions.DependencyInjection;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SharedLib.Extensions;
using SharedLib.Filters;
using SharedLib.Options;
using System;
using System.IO;

namespace Tracker.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddCors()
                .AddMvc(options =>
                {
                    options.EnableEndpointRouting = false;
                });

            services.ConfigureMassTransitAspNetOptions(Configuration);

            services.ConfigureSwagger("Tracker.Api service", "Tracker.Api.xml");

            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterModule(new AutofacModule());
            builder.Populate(services);

            var container = builder.Build();          
            return new AutofacServiceProvider(container);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseCors(cors => cors.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseAuthentication();

            app.UseRequestResponseLogging();

            app.UseSwaggerConfiguration("Tracker.Api service v1");

            app.UseRouting();
            app.UseMvc();
        }
    }
}
