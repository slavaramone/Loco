using System;
using System.Text.Json;
using AutoMapper;
using Contracts;
using Contracts.Enums;
using Contracts.Responses;
using MassTransit;
using SharedLib.Extensions;

namespace Notification.Db
{
	public class AutoMapperProfile : Profile
	{
		public AutoMapperProfile()
		{
			Func<SpeedExceededNotificationMessage, JsonDocument> func = (msg) => { return (JsonDocument) null; };

			CreateMap<NotificationMessage, Entities.Notification>()
				.ForMember(dest => dest.Id, opt => opt.Ignore())
				.ForMember(dest => dest.Severity, exp => exp.MapFrom(src => src.Severity))
				.ForMember(dest => dest.Type, exp => exp.MapFrom(src => src.Type))
				.ForMember(dest => dest.MapItemId, exp => exp.MapFrom(src => src.LocoId))
				.ForMember(dest => dest.CreationDateTime, exp => exp.MapFrom(src => src.TrackDate))
				.ForMember(dest => dest.Message,
					exp => exp.MapFrom(src => src.Type == NotificationType.Custom ? src.Message : src.Type.GetDescription()));

			CreateMap<SpeedExceededNotificationMessage, Entities.SpeedExceededNotification>()
				.IncludeBase<NotificationMessage, Entities.Notification>()
				.ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Latitude))
				.ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Longitude))
				.ForMember(dest => dest.Altitude, opt => opt.MapFrom(src => src.Altitude))
				.ForMember(dest => dest.Speed, opt => opt.MapFrom(src => src.Speed))
				.ForMember(dest => dest.MaxSpeed, opt => opt.MapFrom(src => src.MaxSpeed));


			CreateMap<Entities.Notification, NotificationListItemContract>()
				.ForMember(dest => dest.Message,
					exp => exp.MapFrom(src => src.Type == NotificationType.Custom ? src.Message : src.Type.GetDescription()))
				.ForMember(dest => dest.Metadata, exp => exp.Ignore());
			CreateMap<Entities.SpeedExceededNotification, NotificationListItemContract>()
				.IncludeBase<Entities.Notification, NotificationListItemContract>()
				.ForMember(dest => dest.Metadata, exp => exp.MapFrom((notification, contract, arg3, arg4) => new
				{
					Speed = notification.Speed,
					Latitude = notification.Latitude,
					Longitude = notification.Longitude,
					Altitude = notification.Altitude,
					MaxSpeed = notification.MaxSpeed
				}));

			CreateMap<GetNotificationsResult, NotificationListResponse>()
				.ForMember(dest => dest.NotificationList, exp => exp.MapFrom(src => src.Notifications));

			CreateMap<Entities.SpeedZone, SpeedZoneResponseItem>();
		}
	}
}