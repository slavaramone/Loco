using System.Text.Json.Serialization;

namespace Tracker.Glosav.Api.Reports.Models.Request
{
	public class ApiFuelReportRequestPayload
	{
		[JsonPropertyName("devices")]
		public ApiDeviceModel[] Devices { get; set; }

		[JsonPropertyName("paging")]
		public ApiPagingModel Paging { get; set; }
	}
}
