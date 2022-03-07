using Autofac;
using Notification.Db;

namespace Management.Db
{
	public class RepositoriesModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<NotificationDbRepo>()
				.As<INotificationDbRepo>()
				.InstancePerLifetimeScope();
		}
	}
}