using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SharedLib
{
    public class MassTransitHostedService : IHostedService
	{
		protected readonly IBusControl _bus;
		protected readonly ILogger<MassTransitHostedService> _logger;

		public MassTransitHostedService(IBusControl bus, ILogger<MassTransitHostedService> logger)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_bus = bus ?? throw new ArgumentNullException(nameof(bus));
		}

		public virtual async Task StartAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation($"Begin: StartAsync");

			try
			{
				await _bus.StartAsync(cancellationToken);
			}
			catch (Exception ex)
			{
				_logger.LogCritical("Cannot start", ex);
			}

			_logger.LogInformation($"End: StartAsync");
		}

		public async Task StopAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation($"Begin: StopAsync");
			
			await _bus.StopAsync(cancellationToken);

			_logger.LogInformation($"End: StopAsync");
		}
	}
}
