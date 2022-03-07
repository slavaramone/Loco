using Tracker.Wialon.Messages;

namespace Tracker.Wialon
{
	public interface IWialonMessageHandlerFactory
	{
		IWialonMessageHandler<BaseMessage>
			GetMessageHander(PacketType packetType);
	}
}