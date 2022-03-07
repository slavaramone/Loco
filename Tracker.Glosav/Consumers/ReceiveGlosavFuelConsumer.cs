using AutoMapper;
using Contracts;
using Contracts.Commands;
using Contracts.Requests;
using Contracts.Responses;
using MassTransit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SharedLib;
using SharedLib.Converters;
using SharedLib.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tracker.Glosav.Api.Reports.Client;
using Tracker.Glosav.Api.Reports.Models;
using Tracker.Glosav.Api.Reports.Models.Request;
using Tracker.Glosav.Api.Reports.Models.Response;
using Tracker.Glosav.Helpers;
using Tracker.Glosav.Options;

namespace Tracker.Glosav.Consumers
{
	public class ReceiveGlosavFuelConsumer : IConsumer<ReceiveGlosavFuel>
	{
		private readonly IGlosavReportsClient _client;
		private readonly ILogger<ReceiveGlosavFuelConsumer> _logger;
		private readonly IMapper _mapper;
		private GlosavMonitoringOptions _options;
		private readonly IPublishEndpoint _publishEndpoint;
		private readonly IRequestClient<SensorTrackerIdsRequest> _sensorTrackerIdsClient;

		public ReceiveGlosavFuelConsumer(IMapper mapper, IOptions<GlosavMonitoringOptions> options, IPublishEndpoint publishEndpoint,
			ILogger<ReceiveGlosavFuelConsumer> logger, IGlosavReportsClient client,
			IRequestClient<SensorTrackerIdsRequest> sensorTrackerIdsClient)
		{
			_client = client ?? throw new ArgumentNullException(nameof(client));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_options = options?.Value ?? throw new ArgumentNullException(nameof(options));
			_publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
			_sensorTrackerIdsClient = sensorTrackerIdsClient ?? throw new ArgumentNullException(nameof(sensorTrackerIdsClient));
		}

		public async Task Consume(ConsumeContext<ReceiveGlosavFuel> context)
		{
			var mtResponse = await _sensorTrackerIdsClient.GetResponse<SensorTrackerIdsResponse>(new SensorTrackerIdsRequest());
			var glosavTrackerIds = mtResponse.Message.TrackerIds
				.Where(TrackerIdConverter.IsTrackerIdMatchGlosavTemplate)
				.ToList();

			if (glosavTrackerIds.Any())
			{
				var currentTimeUtc = DateTime.Now.ToUniversalTime();

				var devices = glosavTrackerIds.Select(x => TrackerIdConverter.TrackerIdToGlosavIdentifier(x))
					.Distinct()
					.ToList();

				foreach (var device in devices)
				{
					var query = new ApiCompositionFuelReportRequestQuery
					{
						DeviceId = device.DeviceId,
						OperatorId = device.OperatorId,
						FromTime = currentTimeUtc.Add(-_options.FuelTimeIntervalFilter),
						ToTime = currentTimeUtc,
						SmoothingWindowMinutes = (int) _options.FuelTimeIntervalFilter.TotalMinutes
					};

					try
					{
						try
						{
							var result = await _client.GetCompositionFuelReport(query);

							foreach (var data in result.Data)
							{
								var latestSensorItems = new List<ApiCompositionFuelReportSensor>();

								foreach (var sensor in data.Tanks.Sensors)
								{
									var latestSensorItem = sensor.Value
										.OrderByDescending(x => x.ReportItemDateTime)
										.FirstOrDefault();

									if (latestSensorItem != null)
									{
										latestSensorItems.Add(latestSensorItem);
									}
								}

								if (latestSensorItems.Any())
								{
									await _publishEndpoint.Publish<FuelLevelDataMessage>(new
									{
										TrackerId = TrackerIdConverter.GlosavIdentifierToTrackerId(device.OperatorId, device.DeviceId),
										FuelLevel = Math.Round(GetTanksAverageFuleLevel(latestSensorItems), 2),
										ReportDateTime = latestSensorItems.Max(x => x.ReportItemDateTime)
									});
								}
							}
						}
						catch (Exception ex)
						{
							_logger.LogError(ex, "Не удалось получить данные из Glosav-кластера");
						}

						// foreach (var data in result.Data)
						// {
						// 	foreach (var sensor in data.Tanks.Sensors)
						// 	{
						// 		foreach (var val in sensor.Value)
						// 		{
						// 			var trackerId = TrackerIdConverter.GlosavIdentifierToFuelTrackerId(data.DeviceItem.OperatorId,
						// 				data.DeviceItem.DeviceId, 0, Int32.Parse(sensor.Key));
						//
						// 			await _publishEndpoint.Publish<FuelLevelDataMessage>(new
						// 			{
						// 				TrackerId = trackerId,
						// 				FuelLevel = val.Value,
						// 				ReportDateTime = val.ReportItemDateTime
						// 			});
						// 		}
						// 	}
						// }
					}
					catch (Exception ex)
					{
						_logger.LogError(ex, "Не удалось получить данные из Glosav-кластера");
					}
				}
			}

			_logger.LogInformation("Не были получены идентификаторы для запроса уровня топлива");
		}

		private double GetTanksAverageFuleLevel(List<ApiCompositionFuelReportSensor> latestSensorItems)
		{
			if (latestSensorItems.Any())
			{
				return latestSensorItems.Average(x => x.Value);
			}

			return 0;
		}
	}
}