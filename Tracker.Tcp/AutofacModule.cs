using Autofac;
using SharedLib;
using SharedLib.Extensions;
using SharedLib.Protocols.Azimuth;
using Module = Autofac.Module;

namespace Tracker.Tcp
{
    public class AutofacModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.ConfigureAutoMapper(typeof(AutoMapperProfile));

			builder.ConfigureMassTransitNoEndpoint();

			builder.RegisterType<TcpListenerService>()
				.As<ITcpListener>()
				.SingleInstance();

			builder.RegisterType<AzimuthMessageParser>()
				.As<IAzimuthMessageParser>()
				.SingleInstance();
		}
	}
}
