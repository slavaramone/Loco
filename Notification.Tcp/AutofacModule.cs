using Autofac;
using SharedLib;
using SharedLib.Extensions;
using Module = Autofac.Module;

namespace Notification.Tcp
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
		}
	}
}
