using Autofac;
using Contracts;
using Contracts.Requests;
using Management.Ui.Consumers;
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

                    cfg.ReceiveEndpoint("Management.Ui", e =>
                    {
                        e.Durable = true;
                        e.Consumer<UiTrackerDataConsumer>(context);
                        e.Consumer<UiFuelLevelDataConsumer>(context);
                        e.Consumer<NotificationMessageConsumer>(context);
                    });

                    cfg.UseCustomValidation();
                    cfg.UseExceptionLogger();
                });

                x.AddRequestClient<DateAxisChartRequest>();
                x.AddRequestClient<LocoVideoStreamRequest>();
                x.AddRequestClient<UserListRequest>();
                x.AddRequestClient<UpdateUserRequest>();
                x.AddRequestClient<DeleteUserRequest>();
                x.AddRequestClient<AddUserRequest>();
                x.AddRequestClient<LocoAndSensorByApiKeyRequest>();
                x.AddRequestClient<LocoCoordReportRequest>();
                x.AddRequestClient<SensorFuelReportRequest>();
                x.AddRequestClient<FuelReportCalibrationRequest>();
                x.AddRequestClient<LocoInfosRequest>();
                x.AddRequestClient<LocoListRequest>();
                x.AddRequestClient<UploadCalibrationRequest>();
                x.AddRequestClient<ShunterListRequest>();
                x.AddRequestClient<NotificationListRequest>();
                x.AddRequestClient<InitMapDataRequest>();
                x.AddRequestClient<AuthRequest>();
                x.AddRequestClient<UpdateLocoRequest>();
                x.AddRequestClient<StaticObjectListRequest>();
                x.AddRequestClient<LocoHistoryRequest>();
				x.AddRequestClient<SpeedZoneRequest>();
            });
        }
    }
}
