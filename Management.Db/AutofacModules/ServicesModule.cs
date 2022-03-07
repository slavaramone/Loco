using Autofac;
using SharedLib.Calculators;

namespace Management.Db
{
    public class ServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<Calibrator>()
                .As<ICalibrator>()
                .SingleInstance();

            builder
                .RegisterType<ManagementDbRepo>()
                .As<IManagementDbRepo>()
                .InstancePerLifetimeScope();
        }
    }
}