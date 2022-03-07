using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Tracker.Glosav.Api.Reports.Models.Response
{
	public class ApiCompositionFuelReportTank
	{
		[JsonPropertyName("0")]
		public Dictionary<string, ApiCompositionFuelReportSensor[]> Sensors { get; set; }
	}
}
