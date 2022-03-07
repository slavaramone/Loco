using Autofac;
using Autofac.Extensions.DependencyInjection;
using Management.Ui.Hubs;
using Management.Ui.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharedLib.Extensions;
using SharedLib.Options;
using System;
using System.Text.Json.Serialization;

namespace Management.Ui
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
                 })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                }); ;

            services.AddSignalR(hubOptions =>
            {
                hubOptions.ClientTimeoutInterval = TimeSpan.FromSeconds(5);
            })
            .AddJsonProtocol(options =>
            {
                options.PayloadSerializerOptions.Converters
                   .Add(new JsonStringEnumConverter());
            });

            services.AddLogging();

            services.ConfigureMassTransitAspNetOptions(Configuration);
            services.Configure<VideoOptions>(Configuration.GetSection(VideoOptions.Video));
            services.Configure<AuthOptions>(Configuration.GetSection(AuthOptions.Auth));
            services.Configure<NotificationOptions>(Configuration.GetSection(NotificationOptions.Notification));

            services.ConfigureAuthentication(Configuration);

            services.ConfigureSwagger("Management.Ui service", "Management.Ui.xml");

            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterAssemblyModules(typeof(Startup).Assembly);
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
            
            app.UseForwardedHeaders();

            app.UseAuthentication();

            app.UseSwaggerConfiguration("Management.Ui service v1");

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                Action<HttpConnectionDispatcherOptions> options = opt =>
                {
                    opt.ApplicationMaxBufferSize = 64;
                    opt.TransportMaxBufferSize = 64;
                    opt.Transports = HttpTransportType.WebSockets;
                };
                endpoints.MapHub<ArmHub>("/armhub", options);
                endpoints.MapHub<DispatcherHub>("/dispatcherhub", options);
            });
            app.UseMvc();
        }
    }
}
