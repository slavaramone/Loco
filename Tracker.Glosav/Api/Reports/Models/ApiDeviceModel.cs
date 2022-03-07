using System.Text.Json.Serialization;

namespace Tracker.Glosav.Api.Reports.Models
{
	public class ApiDeviceModel
	{
		[JsonPropertyName("deviceId")]
		public int DeviceId { get; set; }

		[JsonPropertyName("operatorId")]
		public int OperatorId { get; set; }
	}
}
