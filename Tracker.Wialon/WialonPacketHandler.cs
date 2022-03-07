using System;
using System.Globalization;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Tracker.Wialon.Messages;

namespace Tracker.Wialon
{
	public class WialonPacketHandler : IPacketHandler
	{
		private readonly IWialonMessageHandlerFactory _messageHandlerFactory;
		private readonly ILogger<WialonPacketHandler> _logger;

		public WialonPacketHandler(IWialonMessageHandlerFactory messageHandlerFactory, ILogger<WialonPacketHandler> logger)
		{
			_messageHandlerFactory = messageHandlerFactory ?? throw new ArgumentNullException(nameof(messageHandlerFactory));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public void HandlePacket(string packetData, Action<BaseMessage> action)
		{
			_logger.LogTrace($"HandlePacket: {packetData}");

			if (string.IsNullOrWhiteSpace(packetData))
				throw new Exception($"Packet data is null:{packetData}");

			var packetTypeStr = packetData.Substring(1, packetData.IndexOf("#", 1, StringComparison.Ordinal) - 1);

			var packetType = Enum.Parse<PacketType>(packetTypeStr);

			var message = packetData.Substring(packetTypeStr.Length + 2, packetData.Length - packetTypeStr.Length - 2);

			var messageHandler = _messageHandlerFactory.GetMessageHander(packetType);

			messageHandler.HandleMessage(message, action);
		}
	}
}