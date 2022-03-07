using Contracts.Requests;
using Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tracker.Db.Entities;

namespace Tracker.Db
{
    public interface ITrackerDbRepo
    {
        Task<List<(MapItem, RawGeoData)>> GetMapItemsWithLatestGeoData();

        Task<List<MapItem>> GetLocoMapItems(List<Guid> locoIds);

        Task<MapItem> GetMapItemByTrackerId(string trackerId);

        Task<List<(MapItem, RawGeoData)>> GetStaticMapItemsWithLatestGeoData();

        Task<List<string>> GetDynamicMapItemTrackIds();

        Task<List<RawGeoData>> GetRawGeoDataByFilter(LocoCoordReportRequest filter);

        Task<List<FuelSensorRawData>> GetFuelSensorRawDataByFilter(SensorFuelReportRequest filter);

        Task<DateTimeOffset> GetLatestTrackDateTime(Guid mapItemId);

        Task<List<FuelLevel>> GetLatestFuelLevels();

		Task<Dictionary<Guid, FuelSensorRawData>> GetLatestFuelSensorsRawData(IEnumerable<Guid> sensorIds);

        Task<List<ChartItem>> GetSpeedChartItems(Guid locoId, DateTimeOffset startDate, DateTimeOffset endDate);

		Task<List<CoordinatesHistoryResponseItem>> GetCoordinatesHistoryResponseItems(Guid locoId, DateTimeOffset startDate, DateTimeOffset endDate, TimeSpan interval);

		Task<List<FuelHistoryResponseItem>> GetFuelHistoryResponseItems(List<Guid> sensorIds, DateTimeOffset startDate, DateTimeOffset endDate, TimeSpan interval);

		Task<List<ChartItem>> GetFuelChartItems(Guid sensorId, DateTimeOffset startDate, DateTimeOffset endDate);

        Task<List<string>> GetFuelSensorTrackerIds();

        Task<List<FuelSensorRawData>> GetLatestFuelSensorRawData();

		Task<FuelSensor> GetFuelSensor(string trackerId);

		Task<Guid> AddPrecisionGeoPoint(PrecisionGeoData entity);

        Task<Guid> AddRawGeoPoint(RawGeoData entity);

        Task<Guid> AddFuelLevel(FuelLevel entity);

        Task<Guid> AddFuelSensorRawData(FuelSensorRawData fuelSensorRawData);
    }
}
