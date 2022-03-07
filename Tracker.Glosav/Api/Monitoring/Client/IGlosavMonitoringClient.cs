using System.Threading.Tasks;
using Tracker.Glosav.Api.Monitoring.Models.Request;
using Tracker.Glosav.Api.Monitoring.Models.Response;

namespace Tracker.Glosav.Api.Monitoring.Client
{
	public interface IGlosavMonitoringClient
	{
		/// <summary>
		/// Получить список последних сообщений мониторинга для набора устройств
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		Task<ApiMonitoringMessagesResponse> GetLastMonitoringMessages(ApiMonitoringMessagesRequestPayload request);
	}
}
