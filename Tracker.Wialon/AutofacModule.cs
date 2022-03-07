using System.Globalization;
using Autofac;
using Contracts;
using CsvHelper.Configuration;
using MassTransit;
using Microsoft.Extensions.Options;
using SharedLib.Extensions;
using SharedLib.Options;
using Tracker.Wialon.CsvMaps;
using Tracker.Wialon.MessageHandlers;
using Tracker.Wialon.Messages;
using Tracker.Wialon.Tcp;

namespace Tracker.Wialon
{
	public class AutofacModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.ConfigureAutoMapper(typeof(AutoMapperProfile));

			builder.AddMassTransit(x =>
			{
				x.UsingRabbitMq((context, cfg) =>
				{
					var options = context.GetRequiredService<IOptions<RabbitMQOptions>>();
					cfg.Host(options.Value.Uri);
				});

				x.AddRequestClient<FuelLevelDataMessage>();
			});

			builder.RegisterType<WialonServer>()
				.As<ITcpListenerService>()
				.SingleInstance();

			builder.RegisterType<WialonPacketHandler>()
				.As<IPacketHandler>()
				.SingleInstance();

			builder.RegisterType<BlackBoxMessageHandler>()
				.As<IWialonMessageHandler<BlackBoxMessage>>()
				.InstancePerLifetimeScope();
			builder.RegisterType<ExtendedDataMessageHandler>()
				.As<IWialonMessageHandler<ExtendedDataMessage>>()
				.InstancePerLifetimeScope();
			builder.RegisterType<PingMessageHandler>()
				.As<IWialonMessageHandler<PingMessage>>()
				.InstancePerLifetimeScope();

			builder.RegisterGeneric(typeof(CsvWialonMessageHandler<>))
				.As(typeof(IWialonMessageHandler<>))
				.InstancePerLifetimeScope();

			builder.RegisterType<MessageHandlerFactory>()
				.As<IWialonMessageHandlerFactory>();

			var csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture) {HasHeaderRecord = false, Delimiter = ";"};
			csvConfiguration.RegisterClassMap<ShortDataPacketMessageMap>();
			csvConfiguration.RegisterClassMap<ExtendedDataMessageMap>();
			csvConfiguration.RegisterClassMap<LoginMessageMap>();
			csvConfiguration.MissingFieldFound = (strings, i, arg3) => { };


			builder.RegisterInstance<CsvConfiguration>(csvConfiguration)
				.SingleInstance();
		}
	}
}