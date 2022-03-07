using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using SharedLib;
using Tracker.Wialon.Tcp;

namespace Tracker.Wialon
{
	public class MassTransitHostedServiceTcp : MassTransitHostedService
	{
		private readonly ITcpListenerService _tcpListenerService;

		public MassTransitHostedServiceTcp(IBusControl bus, ILogger<MassTransitHostedService> logger,
			ITcpListenerService tcpListenerService)
			: base(bus, logger)
		{
			_tcpListenerService = tcpListenerService;
		}

		public override Task StartAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation($"Begin: StartAsync");

			_tcpListenerService.Start(cancellationToken);

			_logger.LogInformation($"End: StartAsync");
			
			return Task.CompletedTask;
		}
	}
}