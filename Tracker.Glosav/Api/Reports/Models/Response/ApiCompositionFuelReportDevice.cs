using System.Text.Json.Serialization;

namespace Tracker.Glosav.Api.Reports.Models.Response
{
	public class ApiCompositionFuelReportDevice
	{
		[JsonPropertyName("deviceId")]
		public int DeviceId { get; set; }

		[JsonPropertyName("operatorId")]
		public int OperatorId { get; set; }
	}
}
