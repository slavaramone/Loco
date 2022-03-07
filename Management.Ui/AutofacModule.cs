using Autofac;
using AutoMapper;
using Contracts.Requests;
using Management.Ui.Consumers;
using Management.Ui.Hubs;
using Management.Ui.Services;
using MassTransit;
using Microsoft.Extensions.Options;
using SharedLib.Extensions;
using SharedLib.Options;
using SharedLib.Security;

namespace Management.Ui
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.ConfigureAutoMapper(typeof(AutoMapperProfile));
           
            builder.AddMassTransit(x =>
            {
                x.AddConsumers(System.Reflection.Assembly.GetExecutingAssembly());

                x.UsingRabbitMq((context, cfg) =>
                {
                    var options = context.GetRequiredService<IOptions<RabbitMQOptions>>();
                    cfg.Host(options.Value.Uri);

                    cfg.ReceiveEndpoint("Management.Ui", e =>
                    {
                        e.Durable = true;
                        e.Consumer<UiTrackerDataConsumer>(context);
                        e.Consumer<NotificationMessageConsumer>(context);
                    });

                    cfg.UseCustomValidation();
                    cfg.UseExceptionLogger();
                });

                x.AddRequestClient<LocoInfosRequest>();
                x.AddRequestClient<InitMapDataRequest>();
                x.AddRequestClient<AuthRequest>();
            });

            builder.Register(ctx =>
            {
                return new ArmHub(ctx.Resolve<IRequestClient<InitMapDataRequest>>(), ctx.Resolve<IMapper>());
            })
            .SingleInstance();

            builder.RegisterType<ArchiveService>()
                .As<IArchiveService>()
                .SingleInstance();

            builder.RegisterType<ArchiveService>()
                .As<IArchiveService>()
                .SingleInstance();

            builder.RegisterType<ClaimsAccessor>()
                .As<IClaimsAccessor>()
                .SingleInstance();

            builder.RegisterType<JwtFactory>()
                .SingleInstance();
        }
    }
}
