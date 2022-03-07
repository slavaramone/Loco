using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using SharedLib.Filters;
using System;
using System.IO;

namespace SharedLib.Extensions
{
    public static class SwaggerConfigurationExtensions
    {
        public static IServiceCollection ConfigureSwagger(this IServiceCollection services, string apiTitle, string xmlDocFileName)
        {
            services.AddSwaggerGen(options =>
            {
                options.EnableAnnotations();
                options.DocumentFilter<LowercaseDocumentFilter>();

                options.SwaggerDoc("v1", new OpenApiInfo { Title = apiTitle, Version = "v1" });
                options.CustomSchemaIds(type => type.FullName);

                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlDocFileName));

                options.AddSecurityDefinition("Api key", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.ApiKey,
                    Name = "x-api-key",
                    In = ParameterLocation.Header
                });

                options.SchemaFilter<EnumSchemaFilter>();
            });
            return services;
        }

        public static IApplicationBuilder UseSwaggerConfiguration(this IApplicationBuilder app, string name)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", name);
                c.RoutePrefix = string.Empty;
            });
            return app;
        }
    }
}
