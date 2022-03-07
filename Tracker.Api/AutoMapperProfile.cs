using AutoMapper;
using Contracts;
using Contracts.Enums;
using Tracker.Controllers;

namespace Tracker.Api
{
	public class AutoMapperProfile : Profile
	{
		public AutoMapperProfile()
		{
			CreateMap<(TrackRequest request, string trackerId), TrackerDataMessage>()
				.ForMember(dest => dest.Heading, opt => opt.MapFrom(src => src.request.Heading))
				.ForMember(dest => dest.Speed, opt => opt.MapFrom(src => src.request.Speed))
				.ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.request.Latitude))
				.ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.request.Longitude))
				.ForMember(dest => dest.Altitude, opt => opt.MapFrom(src => src.request.Altitude))
				.ForMember(dest => dest.TrackerId, opt => opt.MapFrom(src => src.trackerId))
				.ForMember(dest => dest.DataType, exp => exp.MapFrom(y => TrackerDataType.Raw))
				.ForMember(dest => dest.TrackDate, exp => exp.MapFrom(src => src.request.DateTime));
		}
	}
}