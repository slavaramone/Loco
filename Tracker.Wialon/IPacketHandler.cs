using System;
using System.Threading.Tasks;
using Tracker.Wialon.Messages;

namespace Tracker.Wialon
{
	public interface IPacketHandler
	{
		void HandlePacket(string packetData, Action<BaseMessage> action);
	}
}