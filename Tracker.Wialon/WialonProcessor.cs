using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Contracts;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SharedLib.Tcp;
using Tracker.Wialon.Messages;
using Tracker.Wialon.Tcp;

namespace Tracker.Wialon
{
	public class WialonProcessor : IProcessor
	{
		private readonly TcpClient _client;
		private readonly ILogger<WialonProcessor> _logger;
		private readonly IPacketHandler _packetHandler;
		private readonly IPublishEndpoint _publishEndpoint;
		private readonly IRequestClient<FuelLevelDataMessage> _fuelLevelRequestClient;
		private string DeviceId { get; set; }

		public WialonProcessor(TcpClient client,
			IServiceProvider serviceProvider,
			IPublishEndpoint publishEndpoint,
			IRequestClient<FuelLevelDataMessage> fuelLevelRequestClient)
		{
			_client = client ?? throw new ArgumentNullException(nameof(client));
			_logger = serviceProvider.GetService<ILogger<WialonProcessor>>();
			_packetHandler = serviceProvider.GetService<IPacketHandler>();
			_publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
			_fuelLevelRequestClient = fuelLevelRequestClient ?? throw new ArgumentNullException(nameof(fuelLevelRequestClient));
		}

		public async Task ProcessRequest(CancellationToken cancellationToken)
		{
			byte[] buffer = new byte[25000];

			using (_client)
			{
				if (_client.Client.Poll(0, SelectMode.SelectRead))
				{
					byte[] buff = new byte[25000];
					if (_client.Client.Receive(buff, SocketFlags.Peek) == 0)
					{
						buff = null;
						return;
					}

					buff = null;
				}

				using (var stream = _client.GetStream())
				{
					string ascii = string.Empty;
					var sb = new StringBuilder();

					while (!cancellationToken.IsCancellationRequested)
					{
						var amountReadTask = stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken);
						
						if (amountReadTask.IsFaulted || amountReadTask.IsCanceled)
						{
							break;
						}

						var amountRead = amountReadTask.Result;
						if (amountRead == 0)
						{
							break;
						}

						ascii = Encoding.ASCII.GetString(buffer, 0, amountRead);
						sb.Append(ascii);
						_logger.LogTrace($"data received: DeviceId:{DeviceId} : {ascii}");

						if (ascii.EndsWith("\r\n"))
						{
							_packetHandler.HandlePacket(sb.ToString(), (msg) => { HandleMessage(msg); });
							sb.Clear();
						}
					}
				}

				_client.Close();
			}
		}

		private void Send(string dataStr)
		{
			var bytes = Encoding.ASCII.GetBytes(dataStr);
			_client.Client.Send(bytes);
			_logger.LogTrace($"Send packet: {dataStr}");
		}

		private void HandleMessage(BaseMessage msg)
		{
			if (msg is PingMessage)
			{
				Send("#AP#\r\n");
			}

			if (msg is LoginMessage loginMessage)
			{
				DeviceId = loginMessage.DeviceId;

				_logger.LogTrace($"Device:{DeviceId}, Message: {msg.Payload}");

				Send("#AL#1\r\n");
			}

			if (msg is ExtendedDataMessage extDataMessage)
			{
				ProcessExtendedMessage(extDataMessage);
				Send($"#AD#{1}\r\n");
			}

			if (msg is BlackBoxMessage blackBoxMessage)
			{
				var packetsCount = blackBoxMessage.Payload.Count(x => x == '|');

				var dataToSend = $"#AB#{packetsCount}\r\n";

				foreach (var extendedDataMessage in blackBoxMessage.Messages)
				{
					ProcessExtendedMessage(extendedDataMessage);
				}

				_client.Client.Send(Encoding.ASCII.GetBytes(dataToSend));

				_logger.LogTrace($"Responded: {dataToSend}");
			}
		}

		private Task ProcessExtendedMessage(ExtendedDataMessage extendedDataMessage)
		{
			if (extendedDataMessage.Latitude.HasValue && extendedDataMessage.Longitude.HasValue)
			{
				try
				{
					_publishEndpoint.Publish(new TrackerDataMessage()
					{
						Latitude = extendedDataMessage.Latitude.Value,
						Longitude = extendedDataMessage.Longitude.Value,
						TrackDate = extendedDataMessage.TrackDateTimeUtc,
						Heading = extendedDataMessage.Heading,
						Speed = extendedDataMessage.Speed,
						TrackerId = DeviceId
					});
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, $"Ошибка отправки уровня топлива в шину");
				}
			}

			if (extendedDataMessage?.Parameters?.Any(x => x.Key.StartsWith("dut_", StringComparison.InvariantCultureIgnoreCase)) ?? false)
			{
				foreach (var parameter in extendedDataMessage?.Parameters?.Where(x =>
					x.Key.StartsWith("dut_", StringComparison.InvariantCultureIgnoreCase)))
				{
					try
					{
						_publishEndpoint.Publish<FuelLevelDataMessage>(new
						{
							TrackerId = $"{this.DeviceId}_{parameter.Key}",
							FuelLevel = parameter.Value,
							ReportDateTime = extendedDataMessage.TrackDateTimeUtc
						});
					}
					catch (Exception ex)
					{
						_logger.LogError(ex, $"Ошибка отправки уровня топлива в шину");
					}
				}
			}

			return Task.CompletedTask;
		}
	}
}