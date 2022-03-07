using Autofac;
using AutoMapper;
using System;
using Assembly = System.Reflection.Assembly;

namespace Management.Db.AutofacModules
{
    public sealed class AutoMapperModule : Module
    {
        internal static void ApplyMapperConfiguration(IMapperConfigurationExpression expression, Assembly assembly)
        {
            expression.AllowNullCollections = false;
            expression.AllowNullDestinationValues = true;

            expression.AddMaps(assembly);
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder
                .Register(_ =>
                {
                    var mapperConfiguration = new MapperConfiguration(expression => ApplyMapperConfiguration(expression, ThisAssembly));

                    mapperConfiguration.AssertConfigurationIsValid();

                    return mapperConfiguration.CreateMapper();
                })
                .SingleInstance();
        }
    }
}
