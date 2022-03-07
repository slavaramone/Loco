using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NLog.Extensions.Logging;
using System;
using System.Linq;

namespace SharedLib.Extensions
{
    public static class DbContextConfigurationExtension
    {
        public static IServiceCollection ConfigureDbContext<TDbContext>(this IServiceCollection services, string connectionString) where TDbContext : DbContext
        {
            services.AddDbContext<TDbContext>(options =>
            {
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                options.UseNpgsql(connectionString);
                options.EnableSensitiveDataLogging(true);
                options.UseLoggerFactory(new NLogLoggerFactory());
            });

            var optionsBuilder = new DbContextOptionsBuilder<TDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            using (var db = (TDbContext)Activator.CreateInstance(typeof(TDbContext), new object[] { optionsBuilder.Options }))
            {
                if (db.Database.GetPendingMigrations().Any())
                {
                    db.Database.Migrate();
                }
            }
            return services;
        }
    }
}
