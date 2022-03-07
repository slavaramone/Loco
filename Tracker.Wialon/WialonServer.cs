using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Contracts;
using MassTransit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SharedLib.Options;
using SharedLib.Tcp;
using Tracker.Wialon.Messages;
using Tracker.Wialon.Tcp;

namespace Tracker.Wialon
{
	public class WialonServer : TcpServer, ITcpListenerService
	{
		private readonly ILogger<WialonServer> _logger;
		private readonly IServiceProvider _serviceProvider;
		private readonly IPublishEndpoint _publishEndpoint;
		private readonly IRequestClient<FuelLevelDataMessage> _fuelLevelRequestClient;

		public WialonServer(ILogger<WialonServer> logger, ILoggerFactory loggerFactory,
			IServiceProvider serviceProvider,
			IPublishEndpoint publishEndpoint,
			IRequestClient<FuelLevelDataMessage> fuelLevelRequestClient,
			IOptions<TcpListenerOptions> options)
			: base(options.Value.Port, loggerFactory.CreateLogger<TcpServer>())
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(serviceProvider));
			_serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
			_publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
			_fuelLevelRequestClient = fuelLevelRequestClient ?? throw new ArgumentNullException(nameof(fuelLevelRequestClient));
		}

		protected override void Init()
		{
			
		}

		protected override IProcessor CreateProcessorFor(TcpClient client)
		{
			_logger.LogTrace($"Accepted client");

			return new WialonProcessor(client, _serviceProvider, _publishEndpoint, _fuelLevelRequestClient);
		}
	}
}