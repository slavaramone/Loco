using Microsoft.Extensions.Logging;
using Tracker.Wialon.Messages;

namespace Tracker.Wialon.MessageHandlers
{
	public class PingMessageHandler : WialonMessageHandler<PingMessage>
	{
		public PingMessageHandler(ILogger<WialonMessageHandler<PingMessage>> logger) 
			: base(logger)
		{
			
		}

		public override PingMessage GetMessage(string ascii)
		{
			return new PingMessage();
		}
	}
}