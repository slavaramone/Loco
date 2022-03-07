using Autofac;
using Contracts;
using Contracts.Events;
using Contracts.Requests;
using Contracts.Responses;
using GreenPipes;
using MassTransit;
using Microsoft.Extensions.Options;
using SharedLib.Extensions;
using SharedLib.Filters;
using SharedLib.Options;
using Tracker.Glosav.Consumers;

namespace Tracker.Glosav.AutofacModules
{
    public sealed class MassTransitModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
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
                    cfg.ReceiveEndpoint("Glosav.ReceiveGlosavDevices", e =>
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


                    // сообщение с устройствами для обновления
                    cfg.ReceiveEndpoint(e =>
                    {
                        e.PurgeOnStartup = true;
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
                x.AddRequestClient<SensorTrackerIdsRequest>();
            });
        }
    }
}
