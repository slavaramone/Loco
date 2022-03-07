using Contracts.Requests;
using Management.Db.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Management.Db
{
    public interface IManagementDbRepo
    {
        Task<User> GetUserWithRoles(string login, string password);

        Task<List<User>> GetUsers();

        Task<List<Loco>> GetLocos(bool isOnlyActive);

        Task<List<Loco>> GetLocosWithCamerasAndSensors(LocoInfosRequest filter);

        Task<Guid> GetLocoMapItemIdById(Guid id);

        Task<List<Shunter>> GetShunters();

        Task<List<Sensor>> GetSensorsWithSensorGroup();

        Task<Sensor> GetSensorWithSensorGroup(Guid fuelSensorId);

        Task<Guid> GetLocoSensorId(Guid locoId);

        Task<Dictionary<double, double>> GetFuelLevelCalibrations(Guid sensorId);

        Task<Loco> GetLocoByApiKey(string apiKey);

        Task<List<LocoVideoStream>> GetLocoVideoStreams(Guid locoId);

        Task<Guid> AddUser(User entity);

        Task<User> UpdateUser(Guid id, bool? isActive, string newPassword);

        Task<Loco> UpdateLoco(Guid id, string name, bool isActive);

        Task<User> DeleteUser(Guid id);

        Task<Guid> AddFuelLevelCalibration(FuelLevelCalibration entity);

        Task DeleteCalibrations(Guid sensorId);
    }
}
