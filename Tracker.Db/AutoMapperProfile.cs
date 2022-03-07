using AutoMapper;
using Contracts;
using Contracts.Enums;
using Contracts.Requests;
using Contracts.Responses;
using SharedLib.Calculators;
using System;
using System.Collections.Generic;
using Tracker.Db.Entities;

namespace Tracker.Db
{
    public class AutoMapperProfile : Profile
	{
		private readonly ICalibrator _calibrator;

		public AutoMapperProfile(ICalibrator calibrator)
        {
			_calibrator = calibrator;
        }

		public AutoMapperProfile()
		{
			CreateMap<RawGeoData, LocoReportCoordItemContract>()
				.ForMember(dest => dest.LocoId, exp => exp.MapFrom(src => src.MapItemId))
				.ForMember(dest => dest.Speed, exp => exp.MapFrom(src => src.Speed.HasValue ? Math.Truncate(src.Speed.Value) : (double?)null))
				.ForMember(dest => dest.TrackDateTime, exp => exp.MapFrom(src => src.TrackDate.DateTime));

			CreateMap<FuelLevel, SensorReportFuelItemContract>()
				.ForMember(dest => dest.FuelSensorId, opt => opt.Ignore());

			CreateMap<FuelSensorRawData, SensorReportFuelItemContract>();

			CreateMap<FuelLevelDataMessage, FuelLevelDataCalibrationMessage>()
				.ForMember(dest => dest.FuelSensorId, opt => opt.Ignore());

			CreateMap<FuelLevelDataMessage, FuelLevel>()
				.ForMember(dest => dest.Id, opt => opt.Ignore())
				.ForMember(dest => dest.TrackerId, exp => exp.MapFrom(src => src.TrackerId))
				.ForMember(dest => dest.RawValue, exp => exp.MapFrom(src => src.FuelLevel))
				.ForMember(dest => dest.CreationDateTime, exp => exp.MapFrom(src => DateTimeOffset.Now))
				.ForMember(dest => dest.ReportDateTime, exp => exp.MapFrom(src => src.ReportDateTime));

			CreateMap<(FuelLevelDataMessage msg, Guid fuelSensorId), FuelSensorRawData>()
				.ForMember(dest => dest.Id, opt => opt.Ignore())
				.ForMember(dest => dest.FuelSensorId, exp => exp.MapFrom(src => src.fuelSensorId))
				.ForMember(dest => dest.FuelSensor, opt => opt.Ignore())
				.ForMember(dest => dest.RawValue, exp => exp.MapFrom(src => src.msg.FuelLevel))
				.ForMember(dest => dest.CreationDateTime, exp => exp.MapFrom(src => DateTimeOffset.Now))
				.ForMember(dest => dest.ReportDateTime, exp => exp.MapFrom(src => src.msg.ReportDateTime));

			CreateMap<(TrackerDataMessage msg, MapItem mapItem), UiTrackerDataMessage>()
				.ForMember(dest => dest.Id, exp => exp.MapFrom(src => src.mapItem.Id))
				.ForMember(dest => dest.MapItemName, exp => exp.MapFrom(src => src.mapItem.Name))
				.ForMember(dest => dest.MapItemType, exp => exp.MapFrom(src => src.mapItem.Type))
				.ForPath(dest => dest.TrackerDataMessage.TrackerId, exp => exp.MapFrom(src => src.msg.TrackerId))
				.ForPath(dest => dest.TrackerDataMessage.DataType, exp => exp.MapFrom(src => src.msg.DataType))
				.ForPath(dest => dest.TrackerDataMessage.Latitude, exp => exp.MapFrom(src => src.msg.Latitude))
				.ForPath(dest => dest.TrackerDataMessage.Longitude, exp => exp.MapFrom(src => src.msg.Longitude))
				.ForPath(dest => dest.TrackerDataMessage.Altitude, exp => exp.MapFrom(src => src.msg.Altitude))
				.ForPath(dest => dest.TrackerDataMessage.Speed, exp => exp.MapFrom(src => src.msg.Speed))
				.ForPath(dest => dest.TrackerDataMessage.Heading, exp => exp.MapFrom(src => src.msg.Heading))
				.ForPath(dest => dest.TrackerDataMessage.TrackDate, exp => exp.MapFrom(src => src.msg.TrackDate));

			CreateMap<(TrackerDataMessage msg, Guid mapItemId), RawGeoData>()
				.ForMember(dest => dest.Id, opt => opt.Ignore())
				.ForMember(dest => dest.MapItem, opt => opt.Ignore())
				.ForMember(dest => dest.MapItemId, opt => opt.Ignore())
				.ForMember(dest => dest.PrecisionGeoData, opt => opt.Ignore())
				.ForMember(dest => dest.MapItem, opt => opt.Ignore())
				.ForMember(dest => dest.MapItemId, exp => exp.MapFrom(src => src.mapItemId))
				.ForMember(dest => dest.CreationDate, exp => exp.MapFrom(src => DateTimeOffset.Now))
				.ForMember(dest => dest.Latitude, exp => exp.MapFrom(src => src.msg.Latitude))
				.ForMember(dest => dest.Longitude, exp => exp.MapFrom(src => src.msg.Longitude))
				.ForMember(dest => dest.Altitude, exp => exp.MapFrom(src => src.msg.Altitude))
				.ForMember(dest => dest.TrackDate, exp => exp.MapFrom(src => src.msg.TrackDate))
				.ForMember(dest => dest.Speed, exp => exp.MapFrom(src => src.msg.Speed))
				.ForMember(dest => dest.Heading, exp => exp.MapFrom(src => src.msg.Heading));

			CreateMap<TrackerDataMessage, PrecisionGeoData>()
				.ForMember(dest => dest.Id, opt => opt.Ignore())
				.ForMember(dest => dest.RawGeoData, opt => opt.Ignore())
				.ForMember(dest => dest.RawGeoDataId, opt => opt.Ignore())
				.ForMember(dest => dest.CreationDate, exp => exp.MapFrom(y => DateTimeOffset.Now));

			CreateMap<(MapItem mapItem, RawGeoData rawGeoData), StaticMapItemContract>()
				.ForMember(dest => dest.Id, exp => exp.MapFrom(src => src.mapItem.Id))
				.ForMember(dest => dest.Type, exp => exp.MapFrom(src => src.mapItem.Type))
				.ForMember(dest => dest.Name, exp => exp.MapFrom(src => src.mapItem.Name))
				.ForMember(dest => dest.Latitude, exp => exp.MapFrom(src => src.rawGeoData.Latitude))
				.ForMember(dest => dest.Longitude, exp => exp.MapFrom(src => src.rawGeoData.Longitude))
				.ForMember(dest => dest.Altitude, exp => exp.MapFrom(src => src.rawGeoData.Altitude));

			CreateMap<(MapItem mapItem, RawGeoData rawGeoData), MapItemContract>()
				.ForMember(dest => dest.Id, exp => exp.MapFrom(src => src.mapItem.Id))
				.ForMember(dest => dest.Type, exp => exp.MapFrom(src => src.mapItem.Type))
				.ForMember(dest => dest.Name, exp => exp.MapFrom(src => src.mapItem.Name))
				.ForMember(dest => dest.Latitude, exp => exp.MapFrom(src => src.rawGeoData.Latitude))
				.ForMember(dest => dest.Longitude, exp => exp.MapFrom(src => src.rawGeoData.Longitude))
				.ForMember(dest => dest.Altitude, exp => exp.MapFrom(src => src.rawGeoData.Altitude))
				.ForMember(dest => dest.TrackerId, exp => exp.MapFrom(src => src.mapItem.TrackerId))
				.ForMember(dest => dest.Heading, exp => exp.MapFrom(src => src.rawGeoData.Heading))
				.ForMember(dest => dest.Speed, exp => exp.MapFrom(src => src.rawGeoData.Speed))
				.ForMember(dest => dest.TrackDate, exp => exp.MapFrom(src => src.rawGeoData.TrackDate));

			CreateMap<MapItem, StaticMapItemContract>()
				.ForMember(dest => dest.Latitude, exp => exp.MapFrom(y => y.RawGeoData[0].Latitude))
				.ForMember(dest => dest.Longitude, exp => exp.MapFrom(y => y.RawGeoData[0].Longitude))
				.ForMember(dest => dest.Altitude, exp => exp.MapFrom(y => y.RawGeoData[0].Altitude));

			CreateMap<FuelLevel, FuelLevelContract>()
				.ForMember(dest => dest.Id, opt => opt.Ignore())
				.ForMember(dest => dest.FuelSensorId, opt => opt.Ignore());

			CreateMap<FuelSensorRawData, FuelLevelContract>();

			CreateMap<(FuelLevelDataMessage msg, Guid mapItemId), UiFuelLevelDataMessage>()
				.ForMember(dest => dest.LocoId, exp => exp.MapFrom(src => src.mapItemId))
				.ForMember(dest => dest.CurrentValue, exp => exp.MapFrom(src => Math.Round(src.msg.FuelLevel, 0)))
				.ForMember(dest => dest.ReportDateTime, exp => exp.MapFrom(src => src.msg.ReportDateTime));

			CreateMap<ChartItem, LocoChartItemContract>()
				.ForMember(dest => dest.DateTime, exp => exp.MapFrom(y => y.TrackDate));

			CreateMap<LocoHistoryRequest, LocoCoordReportRequest>()
				.ForMember(dest => dest.LocoIds, exp => exp.MapFrom(src => new List<Guid> { src.LocoId }))
				.ForMember(dest => dest.Skip, opt => opt.Ignore())
				.ForMember(dest => dest.Take, opt => opt.Ignore());

			CreateMap<(LocoHistoryRequest req, Guid fuelSensorId), SensorFuelReportRequest>()
				.ForMember(dest => dest.FuelSensorIds, exp => exp.MapFrom(src => new List<Guid> { src.fuelSensorId }))
				.ForMember(dest => dest.DateTimeFrom, exp => exp.MapFrom(src => src.req.DateTimeFrom))
				.ForMember(dest => dest.DateTimeTo, exp => exp.MapFrom(src => src.req.DateTimeTo))
				.ForMember(dest => dest.Skip, opt => opt.Ignore())
				.ForMember(dest => dest.Take, opt => opt.Ignore());

			CreateMap<LocoHistoryRequest, NotificationListRequest>()
				.ForMember(dest => dest.LocoIds, exp => exp.MapFrom(src => new List<Guid> { src.LocoId }))
				.ForMember(dest => dest.NotificationTypes, exp => exp.MapFrom(src => new List<NotificationType> { NotificationType.Speed }))
				.ForMember(dest => dest.Severities, exp => exp.MapFrom(src => new List<Severity> { Severity.Warning }))
				.ForMember(dest => dest.Sort, exp => exp.MapFrom(src => new SortFilterContract[]
				{
					new SortFilterContract
					{
						By = "date",
						Order = "desc"
					}
				}))
				.ForMember(dest => dest.Skip, opt => opt.Ignore())
				.ForMember(dest => dest.Take, opt => opt.Ignore());
		}
	}
}
