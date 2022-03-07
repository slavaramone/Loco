using System.ComponentModel;

namespace Tracker.Wialon
{
	public enum PacketType
	{
		[Description("Login packet")]
		L = 1,

		[Description("Answer to the login packet")]
		AL,

		[Description("Short data packet")] 
		SD,

		[Description("Answer to the short data packet")]
		ASD,
		[Description("Extended data packet")] 
		D,

		[Description("Answer to the extended data packet")]
		AD,
		[Description("Black box packet")]
		B,

		[Description("Answer to the black box packet")]
		AB,

		[Description("Ping packet ")]
		P,

		[Description("Answer to the ping packet")]
		AP,

		[Description("Firmware packet")]
		US,

		[Description("Configuration packet")]
		UC,

		[Description("Message to/from the driver")]
		M,

		[Description("Answer to the message from the driver")]
		AM,

		[Description("Query snapshot command")]
		QI,
		[Description("Snapshot packet")] I,

		[Description("Answer to the snapshot packet")]
		AI,

		[Description("Query DDD file command")]
		QT,

		[Description("DDD file information packet")]
		IT,

		[Description("Answer to the DDD file information packet")]
		AIT,

		[Description("DDD file block packet")]
		T,

		[Description("Answer to the DDD file block packet")]
		AT,
	}
}