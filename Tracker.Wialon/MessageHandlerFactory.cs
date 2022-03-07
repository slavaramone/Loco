using System;
using Microsoft.Extensions.DependencyInjection;
using Tracker.Wialon.Messages;

namespace Tracker.Wialon
{
	public class MessageHandlerFactory : IWialonMessageHandlerFactory
	{
		private readonly IServiceProvider _serviceProvider;

		public MessageHandlerFactory(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider ?? throw new ArgumentNullException();
		}

		public IWialonMessageHandler<BaseMessage>
			GetMessageHander(PacketType packetType)
		{
			switch (packetType)
			{
				case PacketType.L:
					return _serviceProvider.GetService<IWialonMessageHandler<LoginMessage>>();
				case PacketType.SD:
					return _serviceProvider.GetService<IWialonMessageHandler<ShortDataPacketMessage>>();
				case PacketType.P:
					return _serviceProvider.GetService<IWialonMessageHandler<PingMessage>>();
				case PacketType.B:
					return _serviceProvider.GetService<IWialonMessageHandler<BlackBoxMessage>>();
				case PacketType.D:
					return _serviceProvider.GetService<IWialonMessageHandler<ExtendedDataMessage>>();
			}

			throw new NotImplementedException($"Message type:{packetType} not implemented");
		}
	}
}