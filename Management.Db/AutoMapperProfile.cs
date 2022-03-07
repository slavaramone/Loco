using AutoMapper;
using Contracts;
using Contracts.Requests;
using Management.Db.Entities;
using SharedLib.Security;
using SharedLib.Calculators;
using System;
using System.Linq;

namespace Management.Db
{
	public class AutoMapperProfile : Profile
    {
		public AutoMapperProfile()
		{
			CreateMap<Sensor, Contracts.Responses.SensorContract>();
			CreateMap<SensorGroup, Contracts.Responses.SensorGroupContract>();
			CreateMap<Camera, Contracts.Responses.CameraContract>();
			CreateMap<Loco, Contracts.Responses.LocoContract>()
				.ForMember(dest => dest.Id, exp => exp.MapFrom(src => src.MapItemId.Value));

			CreateMap<Loco, Contracts.Responses.LocoListItemContract>()
				.ForMember(dest => dest.Id, exp => exp.MapFrom(src => src.MapItemId.Value));

			CreateMap<(CalibrationResult res, DateTimeOffset reportDate, Guid mapItemId), UiFuelLevelDataMessage>()
				.ForMember(dest => dest.LocoId, exp => exp.MapFrom(src => src.mapItemId))
				.ForMember(dest => dest.CurrentValue, exp => exp.MapFrom(src => src.res.CalibratedValue))
				.ForMember(dest => dest.ReportDateTime, exp => exp.MapFrom(src => src.reportDate));

			CreateMap<Shunter, Contracts.Responses.ShunterListItemContract>()
				.ForMember(dest => dest.Id, exp => exp.MapFrom(src => src.MapItemId.Value));

			CreateMap<User, Contracts.Responses.AuthResponse>()
				.ForMember(dest => dest.UserId, exp => exp.MapFrom(src => src.Id))
				.ForMember(dest => dest.Login, exp => exp.MapFrom(src => src.Login))
				.ForMember(dest => dest.FirstName, exp => exp.MapFrom(src => src.FirstName))
				.ForMember(dest => dest.LastName, exp => exp.MapFrom(src => src.LastName))
				.ForMember(dest => dest.UserRoles, exp => exp.MapFrom(src => src.UserToRoles.Select(x => x.UserRole).ToList()));

			CreateMap<AddUserRequest, User>()
				.ForMember(dest => dest.Id, opt => opt.Ignore())
				.ForMember(dest => dest.CreationDateTimeUtc, exp => exp.MapFrom(src => DateTimeOffset.Now))
				.ForMember(dest => dest.PasswordSalt, opt => opt.Ignore())
				.ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
				.ForMember(dest => dest.UserToRoles, opt => opt.Ignore())
				.ForMember(dest => dest.IsActive, exp => exp.MapFrom(src => true))
				.AfterMap((src, dest) =>
				{
					if (string.IsNullOrEmpty(src.Login))
                    {
						dest.Login = $"{src.LastName} {src.FirstName}";
                    }

					var salt = Crypto.GenerateSalt();
					dest.PasswordSalt = Convert.ToBase64String(salt);
					dest.PasswordHash = Crypto.HashPassword(src.Password, dest.PasswordSalt);

					dest.UserToRoles = src.Roles.Select(x => new UserToRole
					{
						CreationDateTimeUtc = DateTimeOffset.Now,
						UserRole = x
					}).ToList();
				});

			CreateMap<(User user, AddUserRequest req), Contracts.Responses.AddUserResponse>()
				.ForMember(dest => dest.Id, exp => exp.MapFrom(src => src.user.Id))
				.ForMember(dest => dest.FirstName, exp => exp.MapFrom(src => src.user.FirstName))
				.ForMember(dest => dest.LastName, exp => exp.MapFrom(src => src.user.LastName))
				.ForMember(dest => dest.Login, exp => exp.MapFrom(src => src.user.Login))
				.ForMember(dest => dest.Password, exp => exp.MapFrom(src => src.req.Password))
				.ForMember(dest => dest.Roles, opt => opt.Ignore())
				.AfterMap((src, dest) =>
				{
					dest.Roles = src.user.UserToRoles.Select(x => x.UserRole)
						.ToList();
				});

			CreateMap<User, Contracts.Responses.DeleteUserResponse>();
			CreateMap<User, Contracts.Responses.UpdateUserResponse>();
			CreateMap<User, Contracts.Responses.UserContract>();

			CreateMap<(FuelLevelContract fuelLevel, double calibratedValue, double maxValue, Guid locoId), CalibratedFuelLevelContract>()
				.ForMember(dest => dest.Id, exp => exp.MapFrom(src => src.fuelLevel.Id))
				.ForMember(dest => dest.FuelSensorId, exp => exp.MapFrom(src => src.fuelLevel.FuelSensorId))
				.ForMember(dest => dest.RawValue, exp => exp.MapFrom(src => src.fuelLevel.RawValue))
				.ForMember(dest => dest.ReportDateTime, exp => exp.MapFrom(src => src.fuelLevel.ReportDateTime))
				.ForMember(dest => dest.CalibratedValue, exp => exp.MapFrom(src => src.calibratedValue))
				.ForMember(dest => dest.MaxValue, exp => exp.MapFrom(src => src.maxValue))
				.ForMember(dest => dest.LocoId, exp => exp.MapFrom(src => src.locoId));

			CreateMap<LocoVideoStream, Contracts.Responses.LocoVideoStreamContract>();

			CreateMap<Loco, Contracts.Responses.UpdateLocoResponse>();
		}
	}
}
