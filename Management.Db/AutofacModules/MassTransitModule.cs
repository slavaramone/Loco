using Autofac;
using Management.Db.Consumers;
using MassTransit;
using Microsoft.Extensions.Options;
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
                x.AddConsumers(System.Reflection.Assembly.GetExecutingAssembly());

                x.UsingRabbitMq((context, cfg) =>
                {
                    var options = context.GetRequiredService<IOptions<RabbitMQOptions>>();
                    cfg.Host(options.Value.Uri);

                    cfg.ReceiveEndpoint("Management.Db", e =>
                    {
                        e.Durable = true;

                        e.ConfigureConsumer<CalibrationConsumer>(context);
                        e.ConfigureConsumer<UserListConsumer>(context);
                        e.ConfigureConsumer<UpdateUserConsumer>(context);
                        e.ConfigureConsumer<DeleteUserConsumer>(context);
                        e.ConfigureConsumer<AddUserConsumer>(context);
                        e.ConfigureConsumer<AuthConsumer>(context);
                        e.ConfigureConsumer<LocoInfosConsumer>(context);
                        e.ConfigureConsumer<LocoListConsumer>(context);
                        e.ConfigureConsumer<ShunterListConsumer>(context);
                        e.ConfigureConsumer<FuelLevelDataCalibrationConsumer>(context);
                        e.ConfigureConsumer<UploadCalibrationConsumer>(context);
                        e.ConfigureConsumer<LocoMapItemIdByApiKeyConsumer>(context);
                        e.ConfigureConsumer<LocoVideoStreamConsumer>(context);
                        e.ConfigureConsumer<FuelReportCalibrationConsumer>(context);
                        e.ConfigureConsumer<UpdateLocoConsumer>(context);
                    });

                    cfg.UseExceptionLogger();
                });
            });
        }
    }
}
