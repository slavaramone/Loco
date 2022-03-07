using Microsoft.Extensions.Logging;
using SharedLib.Converters;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Tracker.Glosav.Api.Monitoring.Models.Request;
using Tracker.Glosav.Api.Monitoring.Models.Response;

namespace Tracker.Glosav.Api.Monitoring.Client
{
	public class GlosavMonitoringClient : IGlosavMonitoringClient
	{
		private readonly HttpClient _client;
		private readonly ILogger<GlosavMonitoringClient> _logger;
		private const string MediaType = "application/json";

		public GlosavMonitoringClient(HttpClient client, ILogger<GlosavMonitoringClient> logger)
		{
			_client = client ?? throw new ArgumentNullException(nameof(client));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task<ApiMonitoringMessagesResponse> GetLastMonitoringMessages(ApiMonitoringMessagesRequestPayload request)
		{
			const string path = "navdata/last";

			var options = new JsonSerializerOptions();
			options.Converters.Add(new Iso8601DateTimeConverter());

			var content = new StringContent(JsonSerializer.Serialize(request, options), Encoding.UTF8, MediaType);

			try
			{
				var response = await _client.PostAsync(path, content);

				response.EnsureSuccessStatusCode();

				await using var responseStream = await response.Content.ReadAsStreamAsync();

				return await JsonSerializer.DeserializeAsync<ApiMonitoringMessagesResponse>(responseStream);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Ошибка выполнения запроса к Glosav-кластеру");
				throw;
			}
		}
	}
}
