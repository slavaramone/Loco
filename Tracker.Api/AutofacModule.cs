using System.Reflection;
using Autofac;
using Contracts;
using MassTransit;
using Microsoft.Extensions.Options;
using SharedLib.Extensions;
using SharedLib.Options;
using Module = Autofac.Module;

namespace Tracker.Api
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

                x.AddRequestClient<TrackerDataMessage>();
            });
        }
	}
}
