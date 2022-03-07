using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Tracker.Glosav.Api.Reports.Models.Response
{
	public class ApiCompositionFuelReportResponse
	{
		[JsonPropertyName("data")]
		public ApiCompositionFuelDataModel[] Data { get; set; }
	}
}
