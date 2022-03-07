using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NLog.Web;

namespace Tracker.Api
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateWebHostBuilder(args).Build().Run();
		}

		private static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.UseStartup<Startup>()
				.ConfigureAppConfiguration((context, builder) =>
				{
					builder
						.AddJsonFile("appsettings.json", optional: false)
						.AddEnvironmentVariables();
				})
				.ConfigureLogging((context, loggingBuilder) =>
				{
					loggingBuilder.ClearProviders();
					loggingBuilder.AddNLog();
				})
				.UseNLog();
	}
}