using Autofac;
using Contracts.Requests;
using MassTransit;
using Microsoft.Extensions.Options;
using Notification.Db.Consumers;
using SharedLib.Extensions;
using SharedLib.Options;

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

                    cfg.ReceiveEndpoint("Notification.Db", e =>
                    {
                        e.Durable = true;
                        e.ConfigureConsumer<TrackerDataConsumer>(context);
                        e.ConfigureConsumer<NotificationListConsumer>(context);
                        e.ConfigureConsumer<SpeedZoneConsumer>(context);
                    });

                    cfg.UseCustomValidation();
                    cfg.UseExceptionLogger();
                });

                x.AddRequestClient<InitMapDataRequest>();
            });
        }
    }
}
