using System.Text.Json.Serialization;

namespace Tracker.Glosav.Api.Monitoring.Models.Request
{
	public class ApiMonitoringMessagesRequestPayload
	{
		[JsonPropertyName("devices")]
		public ApiDeviceModel[] Devices { get; set; }
	}
}
