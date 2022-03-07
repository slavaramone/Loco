using AutoMapper;
using Contracts;
using Contracts.Enums;
using Contracts.Requests;
using Contracts.Responses;
using GeoLibrary.Model;
using MassTransit;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SharedLib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Tracker.Db.Entities;

namespace Tracker.Db.Consumers
{
	public class LocoHistoryConsumer : IConsumer<LocoHistoryRequest>
	{
		private readonly IMapper _mapper;
		private readonly ILogger<LocoHistoryConsumer> _logger;
		private readonly ITrackerDbRepo _repo;
		private readonly IRequestClient<LocoInfosRequest> _locoInfosRequest;
		private readonly IRequestClient<NotificationListRequest> _notificationListRequest;

		public LocoHistoryConsumer(IMapper mapper, ITrackerDbRepo repo, ILogger<LocoHistoryConsumer> logger, IRequestClient<LocoInfosRequest> locoInfosRequest,
			IRequestClient<NotificationListRequest> notificationListRequest)
		{
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_repo = repo ?? throw new ArgumentNullException(nameof(repo));
			_locoInfosRequest = locoInfosRequest ?? throw new ArgumentNullException(nameof(locoInfosRequest));
			_notificationListRequest = notificationListRequest ?? throw new ArgumentNullException(nameof(notificationListRequest));
		}

		public async Task Consume(ConsumeContext<LocoHistoryRequest> context)
		{
			_logger.LogInformation("Start consuming message");

			switch (context.Message.Type)
			{
				case LocoHistoryType.Coordinates:
					var coordResponse = await PrepareCoordinatesResponse(context.Message);
					await context.RespondAsync(coordResponse);
					break;
				case LocoHistoryType.Fuel:
					var fuelResponse = await PrepareFuelResponse(context.Message);
					await context.RespondAsync(fuelResponse);
					break;
				case LocoHistoryType.Notification:
					var notificationResponse = await PrepareNotificationResponse(context.Message);
					await context.RespondAsync(notificationResponse);
					break;
				default:
					break;
			}

			_logger.LogInformation("End consuming message");
		}

		private async Task<CoordinatesHistoryResponse> PrepareCoordinatesResponse(LocoHistoryRequest historyRequest)
		{
			var items = await _repo.GetCoordinatesHistoryResponseItems(historyRequest.LocoId, historyRequest.DateTimeFrom, historyRequest.DateTimeTo, historyRequest.Interval);

			return new CoordinatesHistoryResponse
			{
				Items = items
			};
		}

		private async Task<FuelHistoryResponse> PrepareFuelResponse(LocoHistoryRequest historyRequest)
		{
			var locoInfo = await _locoInfosRequest.GetResponse<LocoInfosResponse>(new LocoInfosRequest
			{
				LocoIds = new List<Guid> { historyRequest.LocoId }
			});

			var sensorIds = new List<Guid>();
			foreach (var sensorGroup in locoInfo.Message.Locos[0].SensorGroups)
			{					
				sensorIds.AddRange(sensorGroup.Sensors.Select(x => x.FuelSensorId));
			}

			var items = await _repo.GetFuelHistoryResponseItems(sensorIds, historyRequest.DateTimeFrom, historyRequest.DateTimeTo, historyRequest.Interval);
			return new FuelHistoryResponse
			{
				Items = items
			};
		}

		private async Task<NotificationHistoryResponse> PrepareNotificationResponse(LocoHistoryRequest historyRequest)
		{
			var notificationListFilter = _mapper.Map<NotificationListRequest>(historyRequest);
			var notificationListResponse = await _notificationListRequest.GetResponse<NotificationListResponse>(notificationListFilter);			

			var coordReportFilter = _mapper.Map<LocoCoordReportRequest>(historyRequest);
			var coords = await _repo.GetRawGeoDataByFilter(coordReportFilter);

			var notificationResponseItems = new List<NotificationHistoryResponseItem>();
			foreach (var dateTime in historyRequest.DateTimeFrom.EachDateIntervalStepTo(historyRequest.DateTimeTo, historyRequest.Interval))
			{
				var notications = notificationListResponse.Message.NotificationList
					.FindAll(x => x.Type == NotificationType.Speed && x.CreationDateTime >= dateTime && x.CreationDateTime < dateTime.Add(historyRequest.Interval));

				foreach (var notification in notications)
				{
					notificationResponseItems.Add(new NotificationHistoryResponseItem
					{
						Type = NotificationType.Speed,
						Severity = Severity.Warning,
						Metadata = notification.Metadata,
						Timestamp = dateTime
					});
				}
			}

			return new NotificationHistoryResponse
			{
				Items = notificationResponseItems
			};
		}
	}
}
