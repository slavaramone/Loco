using Autofac;
using Contracts.Requests;
using MassTransit;
using Microsoft.Extensions.Options;
using SharedLib.Extensions;
using SharedLib.Options;
using Tracker.Db.Consumers;

namespace Management.Db.AutofacModules
{
	public sealed class MassTransitModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.AddMassTransit(x =>
			{
				x.AddConsumers(ThisAssembly);

				x.UsingRabbitMq((context, cfg) =>
				{
					var options = context.GetRequiredService<IOptions<RabbitMQOptions>>();

					cfg.Host(options.Value.Uri);

					cfg.ReceiveEndpoint("Tracker.Db", e =>
					{
						e.Durable = true;
						e.ConfigureConsumer<TrackerDataConsumer>(context);
						e.ConfigureConsumer<FuelLevelDataConsumer>(context);
						e.ConfigureConsumer<InitMapDataRequestConsumer>(context);
						e.ConfigureConsumer<LocoCoordReportConsumer>(context);
						e.ConfigureConsumer<SensorFuelReportConsumer>(context);
						e.ConfigureConsumer<LocoChartConsumer>(context);
						e.ConfigureConsumer<SensorTrackerIdsConsumer>(context);
						e.ConfigureConsumer<StaticObjectListConsumer>(context);
						e.ConfigureConsumer<LocoHistoryConsumer>(context);
					});

					cfg.ReceiveEndpoint("Tracker.Db.ReceiveMapItems", e =>
					{
						e.ConfigureConsumer<ReceiveMapItemsConsumer>(context);
					});

					cfg.UseCustomValidation();
					cfg.UseExceptionLogger();
				});

				x.AddRequestClient<LocoInfosRequest>();
				x.AddRequestClient<NotificationListRequest>();
			});
		}
	}
}
