using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog.Extensions.Logging;
using SharedLib.Extensions;
using SharedLib.Options;

namespace Tracker.Wialon
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
						.Configure<TcpListenerOptions>(options =>
							context.Configuration.GetSection(TcpListenerOptions.TcpListener).Bind(options))
						.AddHostedService<MassTransitHostedServiceTcp>();

					services.ConfigureLogging(context.Configuration);
				})
				.ConfigureContainer<ContainerBuilder>((context, builder) => { builder.RegisterModule<AutofacModule>(); })
				.UseConsoleLifetime()
				.RunConsoleAsync();
		}
	}
}