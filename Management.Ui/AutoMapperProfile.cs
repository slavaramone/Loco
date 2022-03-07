using AutoMapper;
using Contracts;
using Contracts.Enums;
using Management.Ui.Controllers;
using Management.Ui.Models;
using Management.Ui.Services;
using SharedLib.Extensions;
using System;
using System.Collections.Generic;
using Autofac.Core.Lifetime;

namespace Management.Ui
{
	public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UiFuelLevelDataMessage, FuelLevelDataModel>();

            CreateMap<(FuelLevelContract contr, Guid mapItemId), FuelLevelDataModel>()
                .ForMember(dest => dest.LocoId, exp => exp.MapFrom(src => src.mapItemId))
                .ForMember(dest => dest.CurrentValue, exp => exp.MapFrom(src => src.contr.RawValue))
                .ForMember(dest => dest.ReportDateTime, exp => exp.MapFrom(src => src.contr.ReportDateTime));

            CreateMap<UiTrackerDataMessage, MapItemModel>()
                .ForMember(dest => dest.Latitude, exp => exp.MapFrom(src => src.TrackerDataMessage.Latitude))
                .ForMember(dest => dest.Longitude, exp => exp.MapFrom(src => src.TrackerDataMessage.Longitude))
                .ForMember(dest => dest.Altitude, exp => exp.MapFrom(src => src.TrackerDataMessage.Altitude))
                .ForMember(dest => dest.Speed, exp => exp.MapFrom(src => src.TrackerDataMessage.Speed))
                .ForMember(dest => dest.Heading, exp => exp.MapFrom(src => src.TrackerDataMessage.Heading))
                .ForMember(dest => dest.TrackDateTime, exp => exp.MapFrom(src => src.TrackerDataMessage.TrackDate))
                .ForMember(dest => dest.Type, exp => exp.MapFrom(src => src.MapItemType))
                .ForMember(dest => dest.Name, exp => exp.MapFrom(src => src.MapItemName))
                .ForMember(dest => dest.TrackerId, exp => exp.MapFrom(src => src.TrackerDataMessage.TrackerId));

			CreateMap<(NotificationMessage msg, int lifetime), NotificationModel>()
				.ForMember(dest => dest.LocoId, exp => exp.MapFrom(src => src.msg.LocoId))
				.ForMember(dest => dest.Severity, exp => exp.MapFrom(src => src.msg.Severity))
				.ForMember(dest => dest.Type, exp => exp.MapFrom(src => src.msg.Type))
				.ForMember(dest => dest.ExpirationDateTime, exp => exp.Ignore())
				.ForMember(dest => dest.Message,
					exp => exp.MapFrom(src => src.msg.Type == NotificationType.Custom ? src.msg.Message : src.msg.Type.GetDescription()))
				.ForMember(dest => dest.ExpirationDateTime, exp => exp.MapFrom(src => DateTimeOffset.Now.AddSeconds(src.lifetime)))
				.ForMember(dest=>dest.Metadata, opt=>opt.Ignore());

			CreateMap<(SpeedExceededNotificationMessage msg, int lifeTime), NotificationModel>()
				.ForMember(dest => dest.LocoId, exp => exp.MapFrom(src => src.msg.LocoId))
				.ForMember(dest => dest.Severity, exp => exp.MapFrom(src => src.msg.Severity))
				.ForMember(dest => dest.Type, exp => exp.MapFrom(src => src.msg.Type))
				.ForMember(dest => dest.ExpirationDateTime, exp => exp.Ignore())
				.ForMember(dest => dest.Message,
					exp => exp.MapFrom(src => src.msg.Type == NotificationType.Custom ? src.msg.Message : src.msg.Type.GetDescription()))
				.ForMember(dest => dest.ExpirationDateTime, exp => exp.MapFrom(src => DateTimeOffset.Now.AddSeconds(src.lifeTime)))
				.ForMember(dest => dest.Metadata, exp => exp.MapFrom((notification, contract, arg3, arg4) => new
				{
					Speed = notification.msg.Speed,
					Latitude = notification.msg.Latitude,
					Longitude = notification.msg.Longitude,
					Altitude = notification.msg.Altitude,
					MaxSpeed = notification.msg.MaxSpeed
				}));

			CreateMap<MapItemContract, MapItemModel>()
                .ForMember(dest => dest.TrackDateTime, exp => exp.MapFrom(src => src.TrackDate));

            CreateMap<AuthRequest, Contracts.Requests.AuthRequest>();

            CreateMap<string, Contracts.Requests.SortFilterContract>()
                .ConvertUsing((x, y) =>
                {
                    string[] sortBy = x.Split("-");

                    return new Contracts.Requests.SortFilterContract
                    {
                        By = sortBy[0],
                        Order = sortBy[1]
                    };
                });

            CreateMap<NotificationListRequest, Contracts.Requests.NotificationListRequest>();

            CreateMap<(UploadCalibrationFileRequest req, byte[] fileBytes), Contracts.Requests.UploadCalibrationRequest>()
                .ForMember(dest => dest.FileBytes, exp => exp.MapFrom(src => src.fileBytes))
                .ForMember(dest => dest.WorksheetName, exp => exp.MapFrom(src => src.req.WorksheetName))
                .ForMember(dest => dest.StartRow, exp => exp.MapFrom(src => src.req.StartRow))
                .ForMember(dest => dest.StartCol, exp => exp.MapFrom(src => src.req.StartCol))
                .ForMember(dest => dest.LeftSensorId, exp => exp.MapFrom(src => src.req.LeftSensorId))
                .ForMember(dest => dest.RightSensorId, exp => exp.MapFrom(src => src.req.RightSensorId));

            CreateMap<AddUserRequest, Contracts.Requests.AddUserRequest>();
            CreateMap<Contracts.Responses.AddUserResponse, AddUserResponse>();

            CreateMap<DeleteUserRequest, Contracts.Requests.DeleteUserRequest>();
            CreateMap<Contracts.Responses.DeleteUserResponse, DeleteUserResponse>();

            CreateMap<UpdateUserRequest, Contracts.Requests.UpdateUserRequest>();
            CreateMap<Contracts.Responses.UpdateUserResponse, UpdateUserResponse>();

            CreateMap<Contracts.Responses.UserContract, UserListResponse>();

            CreateMap<LocoCoordReportRequest, Contracts.Requests.LocoCoordReportRequest>();
            CreateMap<Contracts.Responses.LocoReportCoordItemContract, LocoCoordReportResponse>();

            CreateMap<(LocoFuelReportRequest req, List<Guid> fuelSensorIds), Contracts.Requests.SensorFuelReportRequest>()
                .ForMember(dest => dest.FuelSensorIds, exp => exp.MapFrom(src => src.fuelSensorIds))
                .ForMember(dest => dest.DateTimeFrom, exp => exp.MapFrom(src => src.req.DateTimeFrom))
                .ForMember(dest => dest.DateTimeTo, exp => exp.MapFrom(src => src.req.DateTimeTo))
                .ForMember(dest => dest.Skip, exp => exp.MapFrom(src => src.req.Skip))
                .ForMember(dest => dest.Take, exp => exp.MapFrom(src => src.req.Take));

            CreateMap<Contracts.Responses.LocoReportFuelItemContract, LocoReportFuelItemResponse>()
                .ForMember(dest => dest.ReportDateTime, exp => exp.MapFrom(src => src.ReportDateTime.DateTime));

            CreateMap<Contracts.Responses.LocoListItemContract, LocoListItem>();

            CreateMap<Contracts.Responses.LocoVideoStreamContract, LocoVideoStreamResponse>();

            CreateMap<DateAxisChartRequest, Contracts.Requests.DateAxisChartRequest>()
                .ForMember(dest => dest.LocoId, opt => opt.MapFrom((src, dest, destMember, context) => context.Items["LocoId"]))
                .ForMember(dest => dest.Type, opt => opt.MapFrom((src, dest, destMember, context) => context.Items["Type"]));

            CreateMap<LocoChartItemContract, DateAxisChartResponse>()
				.ForMember(dest => dest.Value, exp => exp.MapFrom(src => Math.Truncate(src.Value)));

            CreateMap<(UpdateLocoRequest req, Guid locoId), Contracts.Requests.UpdateLocoRequest>()
				.ForMember(dest => dest.Name, exp => exp.MapFrom(src => src.req.Name))
				.ForMember(dest => dest.IsActive, exp => exp.MapFrom(src => src.req.IsActive))
				.ForMember(dest => dest.Id, exp => exp.MapFrom(src => src.locoId));

            CreateMap<Contracts.Responses.UpdateLocoResponse, UpdateLocoResponse>();

            CreateMap<StaticMapItemContract, StaticObjectResponse>();

			CreateMap<(LocoHistoryRequest req, LocoHistoryType type), Contracts.Requests.LocoHistoryRequest>()
				.ForMember(dest => dest.LocoId, exp => exp.MapFrom(src => src.req.LocoId))
				.ForMember(dest => dest.DateTimeFrom, exp => exp.MapFrom(src => src.req.DateTimeFrom))
				.ForMember(dest => dest.DateTimeTo, exp => exp.MapFrom(src => src.req.DateTimeTo))
				.ForMember(dest => dest.Interval, exp => exp.MapFrom(src => src.req.Interval))
				.ForMember(dest => dest.Type, exp => exp.MapFrom(src => src.type));

			CreateMap<Contracts.Responses.CoordinatesHistoryResponseItem, CoordinatesHistoryResponse>();

			CreateMap<Contracts.Responses.FuelHistoryResponseItem, FuelHistoryResponse>()
				.ForMember(dest => dest.Value, exp => exp.MapFrom(src => Math.Truncate(src.Value)))
				.ForMember(dest => dest.MaxValue, exp => exp.MapFrom(src => Math.Truncate(src.MaxValue)))
				.ForMember(dest => dest.MinValue, exp => exp.MapFrom(src => Math.Truncate(src.MinValue)));

			CreateMap<Contracts.Responses.NotificationHistoryResponseItem, NotificationHistoryResponse>();
			CreateMap<Contracts.Responses.CoordinatesHistoryResponseItem, SpeedHistoryResponse>();
			CreateMap<Contracts.Responses.SpeedZoneResponseItem, SpeedZoneResponse>()
				.ForMember(dest => dest.Vertexes, exp => exp.MapFrom(src => new List<Point>
				{
					new Point { Latitude = src.LatitudeTopLeft, Longitude = src.LongitudeTopLeft },
					new Point { Latitude = src.LatitudeTopRight, Longitude = src.LongitudeTopRight },
					new Point { Latitude = src.LatitudeBottomRight, Longitude = src.LongitudeBottomRight },
					new Point { Latitude = src.LatitudeBottomLeft, Longitude = src.LongitudeBottomLeft }
				}));
		}
    }
}
