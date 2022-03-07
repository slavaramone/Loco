using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using NLog.Extensions.Logging;
using SharedLib.Extensions;
using System.Reflection;
using System.Threading.Tasks;

namespace Management.Db
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
                    services.ConfigureMassTransitConsoleOptions(context.Configuration);

                    services.ConfigureDbContext<ManagementDbContext>(context.Configuration["Db:ConnectionString"]);

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
