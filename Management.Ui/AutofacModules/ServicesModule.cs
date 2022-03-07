using Autofac;
using Management.Ui.Services;
using SharedLib.Security;

namespace Management.Ui.AutofacModules
{
    public sealed class ServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<JwtFactory>()
                .SingleInstance();


            builder.RegisterType<ExcelService>()
                .As<IExcelService>()
                .SingleInstance();

            builder.RegisterType<ClaimsAccessor>()
                .As<IClaimsAccessor>()
                .SingleInstance();

            builder
                .RegisterType<ArchiveService>()
                .As<IArchiveService>()
                .SingleInstance();
        }
    }
}
