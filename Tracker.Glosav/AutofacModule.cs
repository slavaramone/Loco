using Autofac;
using Contracts;
using Contracts.Events;
using GreenPipes;
using MassTransit;
using Microsoft.Extensions.Options;
using SharedLib.Extensions;
using SharedLib.Filters;
using SharedLib.Options;
using Tracker.Glosav.Consumers;
using Module = Autofac.Module;

namespace Tracker.Glosav
{
    public class AutofacModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.ConfigureAutoMapper(typeof(AutoMapperProfile));

			builder.AddMassTransit(x =>
			{
				x.AddConsumers(ThisAssembly);

				x.AddPublishMessageScheduler();

				x.UsingRabbitMq((context, cfg) =>
				{
					var options = context.GetRequiredService<IOptions<RabbitMQOptions>>();
					cfg.Host(options.Value.Uri);

					cfg.UseInMemoryScheduler();

					// запрос на обновление данных
					cfg.ReceiveEndpoint("Glosav.ReceiveGlosavDevices",
						e =>
						{
							e.PurgeOnStartup = true;
							e.ConfigureConsumer<ReceiveGlosavDevicesConsumer>(context);
						});
					cfg.ReceiveEndpoint("Glosav.ScheduledReceiveGlosavDevices",
						e => { e.ConfigureConsumer<ReceiveGlosavDevicesConsumer>(context); });

					cfg.ReceiveEndpoint("Glosav.ReceiveGlosavFuel", e =>
					{
						e.PurgeOnStartup = true;
						e.ConfigureConsumer<ReceiveGlosavFuelConsumer>(context);
					});
					cfg.ReceiveEndpoint("Glosav.ScheduledReceiveGlosavFuel",
						e => { e.ConfigureConsumer<ReceiveGlosavFuelConsumer>(context); });

					cfg.ReceiveEndpoint(e =>
					{
						e.Handler<MapItemsReceived>(
							ctx => ctx.ConsumeCompleted,
							handler => handler.UseLatest(lt => lt.Created = LatestFilter<MapItemsReceived>.SetLatest));

						e.ConfigureConsumer<MapItemsReceivedConsumer>(context);
					});

					cfg.UseCustomValidation();
					cfg.UseExceptionLogger();
				});

				x.AddRequestClient<TrackerDataMessage>();
				x.AddRequestClient<FuelLevelDataMessage>();
			});
		}
	}
}
