using AutoMapper;
using Contracts;
using Contracts.Enums;
using Contracts.Requests;
using Contracts.Responses;
using GeoLibrary.Model;
using MassTransit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Notification.Db.Options;
using SharedLib.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Notification.Db.Consumers
{
	public class TrackerDataConsumer : IConsumer<TrackerDataMessage>
    {
		private int MetersPerKm = 1000;

		private readonly INotificationDbRepo _repo;
        private readonly IMapper _mapper;
        private readonly IRequestClient<InitMapDataRequest> _initMapDataRequest;
        private readonly ILogger<TrackerDataConsumer> _logger;
        private readonly WarningOptions _warningOptions;

        public TrackerDataConsumer(IMapper mapper, INotificationDbRepo repo, ILogger<TrackerDataConsumer> logger,
            IRequestClient<InitMapDataRequest> initMapDataRequest, IOptions<WarningOptions> warningOptions)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _initMapDataRequest = initMapDataRequest ?? throw new ArgumentNullException(nameof(initMapDataRequest));
            _warningOptions = warningOptions.Value ?? throw new ArgumentNullException(nameof(warningOptions.Value));
        }

        public async Task Consume(ConsumeContext<TrackerDataMessage> context)
        {
            _logger.LogInformation("Start consuming message");

            var existingMapItem = MassTransitHostedServiceDb.MapItems.Find(x => !string.IsNullOrEmpty(x.TrackerId) && x.TrackerId.Equals(context.Message.TrackerId));
            if (existingMapItem == null)
            {
                var response = await _initMapDataRequest.GetResponse<InitMapDataResponse>(new InitMapDataRequest());
                MassTransitHostedServiceDb.MapItems = response.Message.MapItems;
                existingMapItem = MassTransitHostedServiceDb.MapItems.Find(x => !string.IsNullOrEmpty(x.TrackerId) && x.TrackerId.Equals(context.Message.TrackerId));
                if (existingMapItem == null)
                {
                    throw new NotFoundException(context.Message.TrackerId);
                }
            }

            var messages = new List<NotificationMessage>();
            if (existingMapItem.Type == MapItemType.ShuntingLocomotive && context.Message.Speed > 0)
            {
				var locoPoint = new Point(context.Message.Longitude, context.Message.Latitude);

				var speedZoneResult = await IsZoneSpeedExceeded(context.Message.Speed.Value, locoPoint);
				if (speedZoneResult.IsExceeded)
				{
					var msg = new SpeedExceededNotificationMessage()
					{
						LocoId = existingMapItem.Id,
						Severity = Severity.Warning,
						Type = NotificationType.Speed,
						MaxSpeed = speedZoneResult.IsExceeded ? speedZoneResult.MaxSpeed : _warningOptions.SpeedKmPerHour,
						TrackDate = context.Message.TrackDate,
						Latitude = context.Message.Latitude,
						Longitude = context.Message.Longitude,
						Altitude = context.Message.Altitude,
						Speed = context.Message.Speed ?? 0,
					};

                    messages.Add(msg);
                    await context.Publish(msg);
                }
                
                var mapItems = MassTransitHostedServiceDb.MapItems.FindAll(x => x.Id != existingMapItem.Id);
                bool? isLocoApproachingToMapItem = null;
                foreach (var mapItem in mapItems)
                {                    
					var mapItemPoint = new Point(mapItem.Longitude, mapItem.Latitude);
					double distance = MetersPerKm * locoPoint.HaversineDistanceTo(mapItemPoint);
                    
                    if (!isLocoApproachingToMapItem.HasValue)
                    {
						isLocoApproachingToMapItem = IsLocoApproachingToMapItem(context.Message.TrackerId, locoPoint, mapItemPoint, distance);
                    }
                    
                    switch (mapItem.Type)
                    {
                        case MapItemType.Brake:
                            if (isLocoApproachingToMapItem.Value && distance <= _warningOptions.BreakMeters)
                            {
                                var msg = new NotificationMessage
                                {
                                    LocoId = existingMapItem.Id,
                                    Severity = Severity.Warning,
                                    Type = NotificationType.Brake,
									TrackDate = context.Message.TrackDate
								};
                                messages.Add(msg);

                                await context.Publish(msg);
                            }
                            break;

                        case MapItemType.Arrow:
                            if (isLocoApproachingToMapItem.Value && distance <= _warningOptions.ArrowMeters)
                            {
                                var msg = new NotificationMessage
                                {
                                    LocoId = existingMapItem.Id,
                                    Severity = Severity.Warning,
                                    Type = NotificationType.Arrow,
									TrackDate = context.Message.TrackDate
                                };
                                messages.Add(msg);

                                await context.Publish(msg);
                            }
                            break;

                        case MapItemType.Undefined:
                        default:
                            break;
                    }
                }

                if (messages.Any())
                {
					foreach (var msg in messages)
					{
                        var notification = _mapper.Map<Entities.Notification>(msg);
                        await _repo.AddNotification(notification);
                    }
                }
            }
            _logger.LogInformation("End consuming message");
        }

		private async Task<SpeedZoneResult> IsZoneSpeedExceeded(double speed, Point locoPoint)
		{
			var speedZones = await _repo.GetSpeedZones();
			foreach (var speedZone in speedZones)
			{
				var polygon = new Polygon(new[] { new LineString(new[] 
				{ 
					new Point(speedZone.LongitudeTopLeft, speedZone.LatitudeTopLeft), 
					new Point(speedZone.LongitudeTopRight, speedZone.LatitudeTopRight), 
					new Point(speedZone.LongitudeBottomRight, speedZone.LatitudeBottomRight), 
					new Point(speedZone.LongitudeBottomLeft, speedZone.LatitudeBottomLeft),
					new Point(speedZone.LongitudeTopLeft, speedZone.LatitudeTopLeft),
				})});
				bool isInsidePolygon = polygon.IsPointInside(new Point(locoPoint.Longitude, locoPoint.Latitude));

				if (isInsidePolygon && speed > speedZone.MaxSpeed)
				{
					return new SpeedZoneResult
					{
						IsExceeded = true,
						MaxSpeed = speedZone.MaxSpeed
					};
				}	
			}
			return new SpeedZoneResult
			{
				IsExceeded = false
			};
		}

		private bool IsLocoApproachingToMapItem(string locoTrackerId, Point newLocoPoint, Point mapItemPoint, double distance)
		{
            if (MassTransitHostedServiceDb.LocoTrackerIdToMapPoint.TryGetValue(locoTrackerId, out Point prevLocoPoint))
			{
				double prevDistance = MetersPerKm * prevLocoPoint.HaversineDistanceTo(mapItemPoint);
                MassTransitHostedServiceDb.LocoTrackerIdToMapPoint[locoTrackerId] = newLocoPoint;

                return distance < prevDistance;
            }
            else
			{
                MassTransitHostedServiceDb.LocoTrackerIdToMapPoint.TryAdd(locoTrackerId, newLocoPoint);
                return true;
			}
        }
    }
}
