using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Tracker.Glosav.Api.Reports.Models.Response
{
	public class ApiCompositionFuelDataModel
	{
		[JsonPropertyName("device")]
		public ApiCompositionFuelReportDevice DeviceItem { get; set; }

		[JsonPropertyName("tanks")]
		public ApiCompositionFuelReportTank Tanks { get; set; }
	}
}
