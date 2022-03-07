using System.Text.Json.Serialization;

namespace Tracker.Glosav.Api.Monitoring.Models.Response
{
	public class ApiMonitoringMessagesResponse
	{
		[JsonPropertyName("data")]
		public ApiMonitoringMessageModel[] MonitoringMessages { get; set; }
	}
}
