using Contracts.Enums;
using Contracts.Requests;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using MassTransit.Pipeline.Filters;
using Tracker.Db.Entities;
using Contracts.Responses;

namespace Tracker.Db
{
    public class TrackerDbRepo : ITrackerDbRepo
    {
        private const string QueryDateFormat = "yyyy-MM-dd HH:mm:ss";

        private readonly TrackerDbContext _db;
        private readonly IDbConnection _connection;

        public TrackerDbRepo(TrackerDbContext db)
        {
            _db = db;
            _connection = _db.Database.GetDbConnection();
        }

        public async Task<List<(MapItem, RawGeoData)>> GetMapItemsWithLatestGeoData()
        {
            var items = await _db.MapItems
                .Select(item => new
                {
                    MapItem = item,
                    LastRawGeoData = item.RawGeoData.OrderByDescending(d => d.TrackDate)
                        .FirstOrDefault(x => x.TrackDate != DateTimeOffset.MinValue)
                }).ToListAsync();

            return items.Select(x => (x.MapItem, x.LastRawGeoData)).ToList();
        }

        public async Task<List<MapItem>> GetLocoMapItems(List<Guid> locoIds)
        {
            var mapItems = _db.MapItems.AsQueryable();
            if (locoIds != null && locoIds.Any())
            {
                mapItems = mapItems.Where(x => x.Type == MapItemType.ShuntingLocomotive && locoIds.Contains(x.Id));
            }

            return await mapItems.ToListAsync();
        }

        public async Task<MapItem> GetMapItemByTrackerId(string trackerId)
        {
            var mapItem = await _db.MapItems.SingleOrDefaultAsync(x => x.TrackerId.Equals(trackerId));
            return mapItem;
        }

        public async Task<List<(MapItem, RawGeoData)>> GetStaticMapItemsWithLatestGeoData()
        {
            var items = await _db.MapItems
                .Where(x => x.IsStatic)
                .Select(item => new
                {
                    MapItem = item,
                    LastRawGeoData = item.RawGeoData.OrderByDescending(d => d.TrackDate)
                        .FirstOrDefault(x => x.TrackDate != DateTimeOffset.MinValue)
                }).ToListAsync();

            return items.Select(x => (x.MapItem, x.LastRawGeoData)).ToList();
        }

        public async Task<List<string>> GetDynamicMapItemTrackIds()
        {
            var trackIds = await _db.MapItems.Where(x => !x.IsStatic)
                .Select(x => x.TrackerId)
                .ToListAsync();
            return trackIds;
        }

        public async Task<DateTimeOffset> GetLatestTrackDateTime(Guid mapItemId)
        {
            DateTimeOffset trackDateTime = await _db.RawGeoData.Where(x => x.MapItemId == mapItemId)
                .OrderByDescending(x => x.TrackDate)
                .Select(x => x.TrackDate)
                .FirstOrDefaultAsync();
            return trackDateTime;
        }

        public async Task<List<RawGeoData>> GetRawGeoDataByFilter(LocoCoordReportRequest filter)
        {
            var query = _db.RawGeoData.Where(x => x.MapItem.Type == MapItemType.ShuntingLocomotive)
                .OrderBy(x => x.TrackDate)
                .AsQueryable();
            if (filter.LocoIds != null && filter.LocoIds.Any())
            {
                query = query.Where(x => filter.LocoIds.Contains(x.MapItemId));
            }

            if (filter.DateTimeFrom.HasValue)
            {
                query = query.Where(x => x.TrackDate >= filter.DateTimeFrom.Value);
            }

            if (filter.DateTimeTo.HasValue)
            {
                query = query.Where(x => x.TrackDate <= filter.DateTimeTo.Value);
            }

            if (filter.Skip.HasValue)
            {
                query = query.Skip(filter.Skip.Value);
            }

            if (filter.Take.HasValue)
            {
                query = query.Take(filter.Take.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<List<FuelLevel>> GetLatestFuelLevels()
        {
            var trackerIds = await _db.FuelLevels.Select(x => x.TrackerId)
                .Distinct()
                .ToListAsync();
            var result = new List<FuelLevel>();
            var query = _db.FuelLevels.OrderByDescending(x => x.ReportDateTime)
                .AsQueryable();
            foreach (var trackerId in trackerIds)
            {
                var fuelLevel = await query.FirstOrDefaultAsync(x => x.TrackerId == trackerId);
                result.Add(fuelLevel);
            }
            return result;
        }
        
        public async Task<List<FuelSensorRawData>> GetLatestFuelSensorRawData()
        {
            var fuelSensorIds = await _db.FuelSensors.Select(x => x.Id)
                .Distinct()
                .ToListAsync();
            var result = new List<FuelSensorRawData>();
            var query = _db.FuelSensorRawData.OrderByDescending(x => x.ReportDateTime)
                .AsQueryable();
            foreach (Guid sensorId in fuelSensorIds)
            {
                var fuelLevel = await query.FirstOrDefaultAsync(x => x.FuelSensorId == sensorId);
                result.Add(fuelLevel);
            }
            return result;
        }

        public async Task<Dictionary<Guid, FuelSensorRawData>> GetLatestFuelSensorsRawData(IEnumerable<Guid> sensorIds)
        {
            if (sensorIds == null)
                throw new ArgumentNullException(nameof(sensorIds));
            if (!sensorIds.Any())
                throw new InvalidOperationException($"Отсутсвуют идентификаторы в запросе, {nameof(sensorIds)}");

            var sensorsList = sensorIds.ToList();
            
            var rawData = _db
                .FuelSensorRawData
                .Where(x => sensorsList.Contains(x.FuelSensorId))
                .OrderByDescending(x => x.ReportDateTime)
                .GroupBy(x => x.FuelSensorId)
                .ToDictionary(x => x.Key, x => x.FirstOrDefault());

            return rawData;
        }

        public async Task<List<FuelSensorRawData>> GetFuelSensorRawDataByFilter(SensorFuelReportRequest filter)
        {
            var query = _db.FuelSensorRawData.OrderByDescending(x => x.ReportDateTime)
                .AsQueryable();
            if (filter.FuelSensorIds != null && filter.FuelSensorIds.Any())
            {
                query = query.Where(x => filter.FuelSensorIds.Contains(x.FuelSensorId));
            }

            if (filter.DateTimeFrom.HasValue)
            {
                query = query.Where(x => x.ReportDateTime >= filter.DateTimeFrom.Value);
            }

            if (filter.DateTimeTo.HasValue)
            {
                query = query.Where(x => x.ReportDateTime <= filter.DateTimeTo.Value);
            }

            if (filter.Skip.HasValue)
            {
                query = query.Skip(filter.Skip.Value);
            }

            if (filter.Take.HasValue)
            {
                query = query.Take(filter.Take.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<List<ChartItem>> GetSpeedChartItems(Guid locoId, DateTimeOffset startDate, DateTimeOffset endDate)
        {
            using (IDbConnection conn = _connection)
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                var startDateString = startDate.ToString(QueryDateFormat);
                var endDateString = endDate.ToString(QueryDateFormat);

                var parameters = new DynamicParameters(new Dictionary<string, object>
                {
                    { "@startDateString", startDateString },
                    { "@endDateString", endDateString },
                    { "@locoId", locoId }
                });
                string query = @"SELECT
                    avg(l.""Speed"") as Value,
                    max(l.""Speed"") as MaxValue,
                    min(l.""Speed"") as MinValue,
                    date_trunc('hour', l.""TrackDate"" ) + (((date_part('minute', l.""TrackDate"")::integer / 1::integer) * 1::integer)
                     || ' minutes')::interval AS TrackDate
                FROM ""RawGeoData"" as l
                JOIN ""MapItems"" m on l.""MapItemId"" = m.""Id""
                WHERE m.""Id"" = @locoId and l.""TrackDate"" > (select @startDateString::timestamp)
                    and l.""TrackDate"" < (select @endDateString::timestamp)
                GROUP BY TrackDate
                ORDER BY TrackDate";

                var itemsQueried = await _connection.QueryAsync<ChartItem>(query, parameters);
                var result = itemsQueried.ToList();

                return result;
            }
        }

		public async Task<List<CoordinatesHistoryResponseItem>> GetCoordinatesHistoryResponseItems(Guid locoId, DateTimeOffset startDate, DateTimeOffset endDate, TimeSpan interval)
		{
			using (IDbConnection conn = _connection)
			{
				if (conn.State != ConnectionState.Open)
				{
					conn.Open();
				}
				var startDateString = startDate.ToString(QueryDateFormat);
				var endDateString = endDate.ToString(QueryDateFormat);

				var parameters = new DynamicParameters(new Dictionary<string, object>
				{
					{ "@startDateString", startDateString },
					{ "@endDateString", endDateString },
					{ "@span", interval.TotalSeconds },
					{ "@locoId", locoId }
				});
				string query = @"SELECT
                    avg(l.""Latitude"") as Latitude,
                    avg(l.""Longitude"") as Longitude,
                    avg(l.""Altitude"") as Altitude,
                    avg(l.""Speed"") as Speed,
					max(l.""Speed"") as MaxSpeed,
					min(l.""Speed"") as MinSpeed,
                    date_trunc('hour', l.""TrackDate"" ) + (date_part('minute', l.""TrackDate"")::integer || ' minutes')::interval + (((date_part('second', l.""TrackDate"")::integer / @span::integer) * @span::integer) || ' seconds')::interval AS Timestamp
                FROM ""RawGeoData"" as l
                JOIN ""MapItems"" m on l.""MapItemId"" = m.""Id""
                WHERE m.""Id"" = @locoId and l.""TrackDate"" > (select @startDateString::timestamp)
                    and l.""TrackDate"" < (select @endDateString::timestamp)
                GROUP BY Timestamp
                ORDER BY Timestamp";

				var itemsQueried = await _connection.QueryAsync<CoordinatesHistoryResponseItem>(query, parameters);
				var result = itemsQueried.ToList();

				return result;
			}
		}

		public async Task<List<ChartItem>> GetFuelChartItems(Guid sensorId, DateTimeOffset startDate, DateTimeOffset endDate)
        {
            using (IDbConnection conn = _connection)
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                var startDateString = startDate.ToString(QueryDateFormat);
                var endDateString = endDate.ToString(QueryDateFormat);
                var parameters = new DynamicParameters(new Dictionary<string, object>
                {
                    { "@startDateString", startDateString },
                    { "@endDateString", endDateString },
                    { "@sensorId", sensorId }
                });

                string query = @"SELECT
                        avg(l.""RawValue"") as Value,
                        date_trunc('hour', l.""ReportDateTime"" ) + (((date_part('minute', l.""ReportDateTime"")::integer / 1::integer) * 1::integer)
                         || ' minutes')::interval AS TrackDate
                    FROM ""FuelSensorRawData"" as l
                    WHERE l.""FuelSensorId"" = @sensorId and l.""ReportDateTime"" > (select @startDateString::timestamp) and l.""ReportDateTime"" < (select @endDateString::timestamp) 
                    GROUP BY TrackDate
                    ORDER BY TrackDate;";

                var itemsQueried = await _connection.QueryAsync<ChartItem>(query, parameters);
                var result = itemsQueried.ToList();

                    return result;
            }
        }

		public async Task<List<FuelHistoryResponseItem>> GetFuelHistoryResponseItems(List<Guid> sensorIds, DateTimeOffset startDate, DateTimeOffset endDate, TimeSpan interval)
		{
			using (IDbConnection conn = _connection)
			{
				if (conn.State != ConnectionState.Open)
				{
					conn.Open();
				}
				var startDateString = startDate.ToString(QueryDateFormat);
				var endDateString = endDate.ToString(QueryDateFormat);

				var sensorIdsWithQuotes = sensorIds.Select(x => "'" + x + "'");
				string sensorIdsAsStr = "(" + string.Join(",", sensorIdsWithQuotes) + ")";
				var parameters = new DynamicParameters(new Dictionary<string, object>
				{
					{ "@startDateString", startDateString },
					{ "@endDateString", endDateString },
					{ "@span", interval.TotalSeconds }
				});

				string query = @"SELECT
                        avg(l.""RawValue"") as Value,
                        max(l.""RawValue"") as MaxValue,
                        min(l.""RawValue"") as MinValue,
						date_trunc('hour', l.""ReportDateTime"" ) + (date_part('minute', l.""ReportDateTime"")::integer || ' minutes')::interval + (((date_part('second', l.""ReportDateTime"")::integer / @span::integer) * @span::integer) || ' seconds')::interval AS Timestamp
                    FROM ""FuelSensorRawData"" as l
                    WHERE l.""FuelSensorId"" IN " + sensorIdsAsStr + @" and l.""ReportDateTime"" > (select @startDateString::timestamp) and l.""ReportDateTime"" < (select @endDateString::timestamp) 
                    GROUP BY Timestamp
                    ORDER BY Timestamp;";

				var itemsQueried = await _connection.QueryAsync<FuelHistoryResponseItem>(query, parameters);
				var result = itemsQueried.ToList();

				return result;
			}
		}

		public async Task<List<string>> GetFuelSensorTrackerIds()
		{
			var ids = await _db.FuelSensors.Select(x => x.TrackerId)
				.ToListAsync();
			return ids;
		}

		public async Task<FuelSensor> GetFuelSensor(string trackerId)
		{
			var entity = await _db.FuelSensors.FirstOrDefaultAsync(x => x.TrackerId.Equals(trackerId));
			return entity;
		}

		public async Task<Guid> AddPrecisionGeoPoint(PrecisionGeoData entity)
        {
            _db.PrecisionGeoData.Add(entity);
            await _db.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<Guid> AddRawGeoPoint(RawGeoData entity)
        {
            _db.RawGeoData.Add(entity);
            await _db.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<Guid> AddFuelLevel(FuelLevel entity)
        {
            _db.FuelLevels.Add(entity);
            await _db.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<Guid> AddFuelSensorRawData(FuelSensorRawData fuelSensorRawData)
        {
            _db.FuelSensorRawData.Add(fuelSensorRawData);
            await _db.SaveChangesAsync();
            return fuelSensorRawData.Id;
        }
    }
}