using System;
using System.Collections.Generic;

namespace Tracker.Wialon.Messages
{
	public class ExtendedDataMessage : ShortDataPacketMessage
	{
		public string HDOP { get; set; }

		public int? Inputs { get; set; }

		public int? Outputs { get; set; }

		public string Adc { get; set; }

		public string Ibutton { get; set; }

		public Dictionary<string, object> Parameters { get; set; } = new Dictionary<string, object>();

		public int Crc16 { get; }
	}
}