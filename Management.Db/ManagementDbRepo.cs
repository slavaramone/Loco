using Contracts.Exceptions;
using Contracts.Requests;
using Management.Db.Entities;
using MassTransit.Initializers;
using Microsoft.EntityFrameworkCore;
using SharedLib.Exceptions;
using SharedLib.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Management.Db
{
    public class ManagementDbRepo : IManagementDbRepo
	{
		private readonly ManagementDbContext _db;

		public ManagementDbRepo(ManagementDbContext db)
		{
			_db = db;
		}

		/// <remarks>
		/// Хэширование пароля по алгоритму
		/// https://docs.microsoft.com/ru-ru/aspnet/core/security/data-protection/consumer-apis/password-hashing?view=aspnetcore-3.1
		/// </remarks>
		public async Task<User> GetUserWithRoles(string login, string password)
		{
			var user = await _db.Users.Include(x => x.UserToRoles)
				.SingleOrDefaultAsync(x => x.Login == login);

			if (user == null)
            {
				throw new UserNotFoundException();
			}

			string passwordHashed = Crypto.HashPassword(password, user.PasswordSalt);

			if (passwordHashed.Equals(user.PasswordHash) && user.IsActive)
			{
				return user;
			}

			throw new UserNotFoundException();
		}

		public async Task<List<Loco>> GetLocos(bool isOnlyActive)
		{
			var query = _db.Locos.Where(x => x.MapItemId.HasValue);
			if (isOnlyActive)
			{
				query = query.Where(x => x.IsActive);
			}
			var entities = await query.ToListAsync();
			return entities;
		}

		public async Task<List<User>> GetUsers()
		{
			var entities = await _db.Users.Where(x => x.IsActive)
				.ToListAsync();
			return entities;
		}

		public async Task<List<Loco>> GetLocosWithCameras(LocoInfosRequest filter)
		{
			var locos = _db.Locos.Include(x => x.Cameras)
				.Where(x => x.MapItemId.HasValue)
				.AsQueryable();
			if (filter.LocoIds.Any())
			{
				locos = locos.Where(x => filter.LocoIds.Contains(x.MapItemId.Value));
			}

			return await locos.ToListAsync();
		}

        public async Task<List<Loco>> GetLocosWithCamerasAndSensors(LocoInfosRequest filter)
        {
            var locos = _db.Locos.Where(x => x.IsActive)
				.Include(x => x.Cameras)
                .Include(x => x.SensorGroups)
                .ThenInclude(y => y.Sensors)
                .AsQueryable();
            if (filter.LocoIds.Any())
            {
                locos = locos.Where(x => filter.LocoIds.Contains(x.MapItemId.Value));
            }
            return await locos.ToListAsync();
        }

		public async Task<List<Shunter>> GetShunters()
		{
			var entities = await _db.Shunters.Where(x => x.MapItemId.HasValue)
				.ToListAsync();
			return entities;
		}

		public async Task<List<Sensor>> GetSensorsWithSensorGroup()
		{
			var entity = await _db.Sensors.Include(x => x.SensorGroup)
				.ToListAsync();
			return entity;
		}

		public async Task<Sensor> GetSensorWithSensorGroup(Guid fuelSensorId)
		{
			var entity = await _db.Sensors.Include(x => x.SensorGroup)
				.SingleOrDefaultAsync(x => x.FuelSensorId == fuelSensorId);
			return entity;
		}

		public async Task<Dictionary<double, double>> GetFuelLevelCalibrations(Guid sensorId)
		{
			var dic = await _db.FuelLevelCalibrations.Where(x => x.FuelLevelSensorId == sensorId)
				.ToDictionaryAsync(x => x.RawValue, y => y.CalibratedValue);
			return dic;
		}

		public async Task<Loco> GetLocoByApiKey(string apiKey)
		{
			var loco = await _db.LocoApiKeys.Include(x => x.Loco)
				.Where(x => x.ApiKey.Equals(apiKey) && x.Loco.MapItemId.HasValue)
				.Select(x => x.Loco)
				.SingleOrDefaultAsync();
			return loco;
		}

		public async Task<Guid> GetLocoMapItemIdById(Guid id)
		{
			var mapItemId = await _db.Locos.SingleOrDefaultAsync(x => x.Id == id && x.MapItemId.HasValue)
				.Select(x => x.MapItemId.Value);
			return mapItemId;
		}

		public async Task<Guid> GetLocoSensorId(Guid locoId)
		{
			var sensorGroup = await _db.SensorGroups.Include(x => x.Sensors)
				.FirstOrDefaultAsync(x => x.LocoId == locoId);
			if (sensorGroup is null)
			{
				throw new NotFoundException(locoId.ToString());
			}
			var sensor = sensorGroup.Sensors.FirstOrDefault();
			if (sensor is null || !sensor.FuelSensorId.HasValue)
            {
				throw new NotFoundException(sensorGroup.Id.ToString());
			}
			return sensor.FuelSensorId.Value;
		}

		public async Task<List<LocoVideoStream>> GetLocoVideoStreams(Guid locoId)
        {
			var entities = await _db.LocoVideoStreams.Where(x => x.Loco.MapItemId == locoId)
				.ToListAsync();
			return entities;
        }

		public async Task<Guid> AddUser(User entity)
		{
			_db.Users.Add(entity);
			await _db.SaveChangesAsync();
			return entity.Id;
		}

		public async Task<User> UpdateUser(Guid id, bool? isActive, string newPassword)
		{
			var user = await GetUserById(id);

			if (isActive.HasValue)
			{
				user.IsActive = isActive.Value;
			}

			if (!string.IsNullOrEmpty(newPassword))
			{
				user.PasswordHash = Crypto.HashPassword(newPassword, user.PasswordSalt);
			}

			_db.Update(user);
			await _db.SaveChangesAsync();
			return user;
		}

		public async Task<Loco> UpdateLoco(Guid id, string name, bool isActive)
		{
			var loco = await GetLocoById(id);

			loco.IsActive = isActive;

			if (!string.IsNullOrEmpty(name))
			{
				loco.Name = name;
			}

			_db.Update(loco);
			await _db.SaveChangesAsync();
			return loco;
		}

		public async Task<User> DeleteUser(Guid id)
		{
			var user = await GetUserById(id);

			_db.Remove(user);
			await _db.SaveChangesAsync();
			return user;
		}

		public async Task<Guid> AddFuelLevelCalibration(FuelLevelCalibration entity)
		{
			_db.FuelLevelCalibrations.Add(entity);
			await _db.SaveChangesAsync();
			return entity.Id;
		}

		public async Task DeleteCalibrations(Guid sensorId)
		{
			var entities = await _db.FuelLevelCalibrations.Where(x => x.FuelLevelSensorId == sensorId)
				.ToListAsync();
			foreach (var entity in entities)
			{
				_db.Remove(entity);
			}

			await _db.SaveChangesAsync();
		}

		private async Task<User> GetUserById(Guid id)
        {
			var entity = await _db.Users.SingleOrDefaultAsync(x => x.Id == id);
			if (entity is null)
			{
				throw new NotFoundException(id.ToString());
			}
			return entity;
		}

		private async Task<Loco> GetLocoById(Guid id)
		{
			var entity = await _db.Locos.SingleOrDefaultAsync(x => x.MapItemId == id);
			if (entity is null)
			{
				throw new NotFoundException(id.ToString());
			}
			return entity;
		}
	}
}