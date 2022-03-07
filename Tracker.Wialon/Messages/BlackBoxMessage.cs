using System.Collections.Generic;

namespace Tracker.Wialon.Messages
{
	public class BlackBoxMessage : BaseMessage
	{
		public List<ExtendedDataMessage> Messages { get; set; }
	}
}