using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog.Extensions.Logging;
using Notification.Db.Options;
using SharedLib.Extensions;
using SharedLib.Options;
using System.Threading.Tasks;

namespace Notification.Db
{
    public class Program
    {
        static Task Main(string[] args)
        {
            return new HostBuilder()
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureHostConfiguration(config =>
                {
                    config.AddJsonFile("appsettings.json", optional: true);
                    config.AddEnvironmentVariables();
                })
                .ConfigureLogging(opts =>
                {
                    opts.AddNLog();
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddOptions()
                        .Configure<RabbitMQOptions>(options => context.Configuration.GetSection(RabbitMQOptions.RabbitMQ).Bind(options))
                        .Configure<WarningOptions>(options => context.Configuration.GetSection(WarningOptions.Warning).Bind(options))
                        .AddHostedService<MassTransitHostedServiceDb>();

                    services.ConfigureDbContext<NotificationDbContext>(context.Configuration["Db:ConnectionString"]);

                    services.ConfigureLogging(context.Configuration);
                })
                .ConfigureContainer<ContainerBuilder>((context, builder) =>
                {
                    builder.RegisterAssemblyModules(typeof(Program).Assembly);
                })
                .UseConsoleLifetime()
                .RunConsoleAsync();
        }
    }
}
