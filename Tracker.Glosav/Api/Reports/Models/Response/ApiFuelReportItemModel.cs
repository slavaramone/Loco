using System.Text.Json.Serialization;

namespace Tracker.Glosav.Api.Reports.Models.Response
{
	public class ApiFuelReportItemModel
	{
		[JsonPropertyName("deviceId")]
		public int DeviceId { get; set; }

		[JsonPropertyName("operatorId")]
		public int OperatorId { get; set; }

		[JsonPropertyName("stopFuelLevel")]
		public double StopFuelLevel { get; set; }
	}
}
