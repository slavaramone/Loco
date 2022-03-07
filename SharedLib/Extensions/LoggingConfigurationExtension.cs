using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace SharedLib.Extensions
{
    public static class LoggingConfigurationExtension
    {
        public static IServiceCollection ConfigureLogging(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.SetMinimumLevel(LogLevel.Trace);
                loggingBuilder.AddNLog(configuration);
            });
            return services;
        }
    }
}
