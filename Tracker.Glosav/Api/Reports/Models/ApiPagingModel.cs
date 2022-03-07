using System.Text.Json.Serialization;

namespace Tracker.Glosav.Api.Reports.Models
{
	public class ApiPagingModel
	{
		[JsonPropertyName("page")]
		public int Page { get; set; }

		[JsonPropertyName("pageSize")]
		public int PageSize { get; set; }
	}
}
