using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using SharedLib.Converters;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Tracker.Glosav.Api.Reports.Models.Request;
using Tracker.Glosav.Api.Reports.Models.Response;
using Tracker.Glosav.Helpers;

namespace Tracker.Glosav.Api.Reports.Client
{
	public class GlosavReportsClient : IGlosavReportsClient
	{
		private readonly HttpClient _client;
		private readonly ILogger<GlosavReportsClient> _logger;
		private const string MediaType = "application/json";

		public GlosavReportsClient(HttpClient client, ILogger<GlosavReportsClient> logger)
		{
			_client = client ?? throw new ArgumentNullException(nameof(client));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task<ApiFuelReportResponse> GetFuelReport(ApiFuelReportRequestQuery query, ApiFuelReportRequestPayload payload)
		{
			const string path = "reports/fuel/devices";

			var options = new JsonSerializerOptions();
			options.Converters.Add(new Iso8601DateTimeConverter());

			var content = new StringContent(JsonSerializer.Serialize(payload, options), Encoding.UTF8, MediaType);

			var param = new Dictionary<string, string>()
			{
				{ "toTime", query.ToTime.ToString(GlosavFormatHelper.GlosavDateTimeFormat) },
				{ "fromTime", query.FromTime.ToString(GlosavFormatHelper.GlosavDateTimeFormat) },
				{ "sum", query.Sum.ToString().ToLower() },
			};

			var url = new Uri(QueryHelpers.AddQueryString($"{_client.BaseAddress}{path}", param));

			try
			{
				var response = await _client.PostAsync(url, content);

				response.EnsureSuccessStatusCode();

				await using var responseStream = await response.Content.ReadAsStreamAsync();

				return await JsonSerializer.DeserializeAsync<ApiFuelReportResponse>(responseStream);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Ошибка выполнения запроса к Glosav-кластеру");
				throw;
			}
		}

		public async Task<ApiCompositionFuelReportResponse> GetCompositionFuelReport(ApiCompositionFuelReportRequestQuery query)
		{
			string path = $"compositions/fuel/operators/{query.OperatorId}/devices/{query.DeviceId}";

			var param = new Dictionary<string, string>()
			{
				{ "toTime", query.ToTime.ToString(GlosavFormatHelper.GlosavDateTimeFormat) },
				{ "fromTime", query.FromTime.ToString(GlosavFormatHelper.GlosavDateTimeFormat) },
				{ "smoothingWindow ", query.SmoothingWindowMinutes.ToString() },
			};

			var url = new Uri(QueryHelpers.AddQueryString($"{_client.BaseAddress}{path}", param));

			try
			{
				var response = await _client.GetAsync(url);

				response.EnsureSuccessStatusCode();

				await using var responseStream = await response.Content.ReadAsStreamAsync();

				return await JsonSerializer.DeserializeAsync<ApiCompositionFuelReportResponse>(responseStream);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Ошибка выполнения запроса к Glosav-кластеру");
				throw;
			}
		}
	}
}
