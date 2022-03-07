using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using NLog.Extensions.Logging;
using SharedLib.Extensions;
using SharedLib.Options;
using System;
using System.Threading.Tasks;
using Tracker.Glosav.Api.Monitoring.Client;
using Tracker.Glosav.Api.Reports.Client;
using Tracker.Glosav.Options;

namespace Tracker.Glosav
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
				.ConfigureLogging(opts => { opts.AddNLog(); })
				.ConfigureServices((context, services) =>
				{
					services.AddOptions()
						.Configure<RabbitMQOptions>(options => context.Configuration.GetSection(RabbitMQOptions.RabbitMQ).Bind(options))
						.AddHostedService<GlosavMonitoringHostedService>();

					services.Configure<GlosavMonitoringOptions>(context.Configuration.GetSection(GlosavMonitoringOptions.GlosavMonitoring));

					services.AddHttpClient<IGlosavMonitoringClient, GlosavMonitoringClient>((serviceProvider, client) =>
					{
						var options = serviceProvider.GetRequiredService<IOptions<GlosavMonitoringOptions>>().Value;

						client.BaseAddress = new Uri(options.GlosavApiBaseUrl);
						client.Timeout = options.RequestTimeout;
					});

					services.AddHttpClient<IGlosavReportsClient, GlosavReportsClient>((serviceProvider, client) =>
					{
						var options = serviceProvider.GetRequiredService<IOptions<GlosavMonitoringOptions>>().Value;

						client.BaseAddress = new Uri(options.GlosavApiBaseUrl);
						client.Timeout = options.RequestTimeout;
					});

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
