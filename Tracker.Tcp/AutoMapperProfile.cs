using System;
using AutoMapper;
using Contracts;
using Contracts.Enums;
using Microsoft.AspNetCore.Authentication;
using SharedLib.Protocols.Azimuth;

namespace Tracker.Tcp
{
    public class AutoMapperProfile : Profile
    {
		public AutoMapperProfile()
		{
			CreateMap<(AzimuthLocationReportPayload payload, string trackerId), TrackerDataMessage>()
				.ForMember(dest => dest.DataType, exp => exp.MapFrom(y => TrackerDataType.Raw))
				.ForMember(dest => dest.TrackerId, exp => exp.MapFrom(y => y.trackerId))
				.ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.payload.Latitude))
				.ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.payload.Longitude))
				.ForMember(dest => dest.Altitude, opt => opt.MapFrom(src => src.payload.Altitude))
				.ForMember(dest => dest.Heading, opt => opt.MapFrom(src => src.payload.Heading))
				.ForMember(dest => dest.Speed, opt => opt.MapFrom(src => src.payload.Speed))
				.ForMember(dest => dest.TrackDate, opt => opt.MapFrom(src => src.payload.ReportDate));
		}
	}
}
