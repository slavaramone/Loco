using Autofac;
using MassTransit;
using Microsoft.Extensions.Options;
using SharedLib.Extensions;
using SharedLib.Options;
using System.Reflection;
using Tracker.Db.Consumers;
using Module = Autofac.Module;

namespace Tracker.Db
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.ConfigureAutoMapper(typeof(AutoMapperProfile));

            builder.AddMassTransit(x =>
            {
                x.AddConsumers(Assembly.GetExecutingAssembly());

                x.UsingRabbitMq((context, cfg) =>
                {
                    var options = context.GetRequiredService<IOptions<RabbitMQOptions>>();
                    
                    cfg.Host(options.Value.Uri);

                    cfg.ReceiveEndpoint("Tracker.Db", e =>
                    {
                        e.Durable = true;
                        e.ConfigureConsumer<TrackerDataConsumer>(context);
                        e.ConfigureConsumer<InitMapDataRequestConsumer>(context);
                    });

                    cfg.ReceiveEndpoint("Tracker.Db.ReceiveMapItems", e =>
                    {
                        e.PurgeOnStartup = true;
                        e.ConfigureConsumer<ReceiveMapItemsConsumer>(context);
                    });

                    cfg.UseCustomValidation();
                    cfg.UseExceptionLogger();
                });
            });

            builder.RegisterType<TrackerDbRepo>()
                .As<ITrackerDbRepo>()
                .InstancePerDependency();
        }
    }
}
