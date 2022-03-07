using System;

namespace Tracker.Wialon.Messages
{
	public class ShortDataPacketMessage : BaseMessage, ICrc16
	{
		internal DateTime? Date { get; set; }

		internal TimeSpan? Time { get; set; }

		public DateTimeOffset TrackDateTimeUtc { get; internal set; }

		public double? Latitude { get; set; }

		public double? Longitude { get; set; }

		public int? Speed { get; set; }

		public int? Heading { get; set; }

		public int? Altitude { get; set; }

		public int? Satellites { get; set; }

		public string Crc16 { get; }
	}
}