using MassTransit;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace SharedLib
{
    public class MassTransitHostedServiceTcp : MassTransitHostedService
	{
		private readonly ITcpListener _tcpListener;

		public MassTransitHostedServiceTcp(IBusControl bus, ILogger<MassTransitHostedService> logger, ITcpListener tcpListener) : base(bus, logger)
		{
			_tcpListener = tcpListener;
		}

		public override Task StartAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation($"Begin: StartAsync");

			_tcpListener.Listen();

			_logger.LogInformation($"End: StartAsync");
			return Task.CompletedTask;
		}
	}
}
