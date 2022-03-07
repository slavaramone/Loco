using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using NLog.Extensions.Logging;
using SharedLib.Extensions;
using System.Threading.Tasks;

namespace Notification.Tcp
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
                    services.ConfigureMassTransitTcpConsoleOptions(context.Configuration);

                    services.ConfigureLogging(context.Configuration);
                })
                .ConfigureContainer<ContainerBuilder>((context, builder) =>
                {
                    builder.RegisterModule<AutofacModule>();
                })
                .UseConsoleLifetime()
                .RunConsoleAsync();
        }
    }
}
