using Autofac;
using Tracker.Db;

namespace Management.Db
{
    public class RepositoriesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<TrackerDbRepo>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}