using System.Text.Json.Serialization;

namespace Tracker.Glosav.Api.Reports.Models.Response
{
	public class ApiFuelReportResponse
	{
		[JsonPropertyName("data")]
		public ApiFuelReportItemModel[] ReportItems { get; set; }

		[JsonPropertyName("pageCount")]
		public int PageCount { get; set; }
	}
}
