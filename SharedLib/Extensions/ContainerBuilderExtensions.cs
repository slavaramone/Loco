using Autofac;
using AutoMapper;
using MassTransit;
using Microsoft.Extensions.Options;
using SharedLib.Options;
using System;
using System.Reflection;

namespace SharedLib.Extensions
{
	public static class ContainerBuilderExtensions
	{
		public static void ConfigureAutoMapper(this ContainerBuilder builder, Type profileType)
		{
			builder.Register(componentContext =>
				{
					var mapperConfiguration = new MapperConfiguration(cfg =>
					{
						cfg.AllowNullCollections = false;
						cfg.AllowNullDestinationValues = true;

						cfg.AddMaps(profileType);
					});

					mapperConfiguration.AssertConfigurationIsValid();

					return mapperConfiguration.CreateMapper();
				})
				.SingleInstance();
		}


		public static void ConfigureMassTransitNoEndpoint(this ContainerBuilder builder)
		{
			builder.AddMassTransit(x =>
			{
				x.UsingRabbitMq((context, cfg) =>
				{
					var options = context.GetRequiredService<IOptions<RabbitMQOptions>>();
					cfg.Host(options.Value.Uri);
				});
			});
		}
	}
}