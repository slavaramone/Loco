using System;
using System.Text.Json.Serialization;
using Tracker.Glosav.Api.Monitoring.Models.Request;

namespace Tracker.Glosav.Api.Monitoring.Models.Response
{
	public class ApiMonitoringMessageModel
	{
		[JsonPropertyName("device")]
		public ApiDeviceModel Device { get; set; }

		[JsonPropertyName("tm")]
		public DateTimeOffset DateTime { get; set; }

		[JsonPropertyName("lat")]
		public double Latitude { get; set; }

		[JsonPropertyName("lon")]
		public double Longitude { get; set; }

		[JsonPropertyName("dir")]
		public double Direction { get; set; }

		[JsonPropertyName("alt")]
		public double Altitude { get; set; }

		[JsonPropertyName("spd")]
		public double Speed { get; set; }
	}
}
