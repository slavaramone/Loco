using System;
using System.Text.Json.Serialization;

namespace Tracker.Glosav.Api.Reports.Models.Response
{
	public class ApiCompositionFuelReportSensor
	{
		[JsonPropertyName("value")]
		public double Value { get; set; }

		[JsonPropertyName("smoothed")]
		public double Smoothed { get; set; }

		[JsonPropertyName("utc")]
		public DateTimeOffset ReportItemDateTime { get; set; }
	}
}
