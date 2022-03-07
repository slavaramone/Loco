using AutoMapper;
using Contracts;
using Contracts.Enums;
using SharedLib.Converters;
using Tracker.Glosav.Api.Monitoring.Models.Response;
using Tracker.Glosav.Helpers;

namespace Tracker.Glosav
{
    public class AutoMapperProfile : Profile
	{
		public AutoMapperProfile()
		{
			CreateMap<ApiMonitoringMessageModel, TrackerDataMessage>()
				.ForMember(dest => dest.TrackerId, exp => exp.MapFrom(src => TrackerIdConverter.GlosavIdentifierToTrackerId(src.Device.OperatorId, src.Device.DeviceId)))
				.ForMember(dest => dest.DataType, exp => exp.MapFrom(src => TrackerDataType.Raw))
				.ForMember(dest => dest.Heading, exp => exp.MapFrom(src => src.Direction))
				.ForMember(dest => dest.TrackDate, exp => exp.MapFrom(src => src.DateTime));
		}
	}
}
