using AutoMapper;
using Contracts;
using Contracts.Commands;
using Contracts.Events;
using Contracts.Responses;
using MassTransit;
using Microsoft.Extensions.Logging;
using SharedLib.Converters;
using SharedLib.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tracker.Glosav.Api.Monitoring.Client;
using Tracker.Glosav.Api.Monitoring.Models;
using Tracker.Glosav.Api.Monitoring.Models.Request;
using Tracker.Glosav.Api.Monitoring.Models.Response;
using Tracker.Glosav.Helpers;

namespace Tracker.Glosav.Consumers
{
    /// <summary>
    /// Потребитель реализует функциональность получения данных по устройствам из Glosav-кластера
    /// </summary>
    public class ReceiveGlosavDevicesConsumer : IConsumer<ReceiveGlosavDevices>
	{
		private readonly IGlosavMonitoringClient _glosavMonitoringClient;
		private readonly ILogger<ReceiveGlosavDevicesConsumer> _logger;
		private readonly IRequestClient<TrackerDataMessage> _client;
		private readonly IMapper _mapper;

		public ReceiveGlosavDevicesConsumer(IRequestClient<TrackerDataMessage> client, IGlosavMonitoringClient glosavMonitoringClient, IMapper mapper, ILogger<ReceiveGlosavDevicesConsumer> logger)
		{
			_client = client ?? throw new ArgumentNullException(nameof(client));
			_glosavMonitoringClient = glosavMonitoringClient ?? throw new ArgumentNullException(nameof(glosavMonitoringClient));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		}

		public async Task Consume(ConsumeContext<ReceiveGlosavDevices> context)
		{
			var devices = await LatestFilter<MapItemsReceived>.Context.ConfigureAwait(false);
			var glosavTrackerIds = devices?.Message.MapItemTrackIds.Where(TrackerIdConverter.IsTrackerIdMatchGlosavTemplate).ToList();

			// Если конфигурация запрашиваемых устройств была получена, отправляется запрос в Glosav-кластер
			if (glosavTrackerIds != null && glosavTrackerIds.Any())
			{
				var request = new ApiMonitoringMessagesRequestPayload
				{
					Devices = glosavTrackerIds.Select(x =>
					{
						var (operatorId, deviceId) = TrackerIdConverter.TrackerIdToGlosavIdentifier(x);
						return new ApiDeviceModel
						{
							DeviceId = deviceId,
							OperatorId = operatorId
						};
					}).ToArray()
				};

				try
				{
					var result = await _glosavMonitoringClient.GetLastMonitoringMessages(request);

					foreach (var monMsg in result.MonitoringMessages)
					{
						string trackerId = TrackerIdConverter.GlosavIdentifierToTrackerId(monMsg.Device.OperatorId, monMsg.Device.DeviceId);
						if (GlosavMonitoringHostedService.DevicesToLastMessage.ContainsKey(trackerId))
						{
							var lastMonMsg = GlosavMonitoringHostedService.DevicesToLastMessage[trackerId];
							if (lastMonMsg.DateTime == monMsg.DateTime)
                            {
								GlosavMonitoringHostedService.DevicesToLastMessage[trackerId] = monMsg;
								continue;
							}

							GlosavMonitoringHostedService.DevicesToLastMessage[trackerId] = monMsg;
						}
						else
                        {
							GlosavMonitoringHostedService.DevicesToLastMessage.Add(trackerId, monMsg);
						}

						var mtMsg = _mapper.Map<TrackerDataMessage>(monMsg);
						var response = await _client.GetResponse<TrackerDataResponse>(mtMsg, context.CancellationToken);
					}
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, "Не удалось получить данные из Glosav-кластера");
				}
			}
		}
	}
}
