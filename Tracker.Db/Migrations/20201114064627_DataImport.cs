using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using Tracker.Db.Entities;

namespace Tracker.Db.Migrations
{
    public partial class DataImport : Migration
    {
		private readonly TrackerDbContext _db;

		public DataImport(TrackerDbContext db)
		{
			_db = db;
		}

		protected override void Up(MigrationBuilder migrationBuilder)
        {
			var fuelLevels = _db.FuelLevels.OrderByDescending(x => x.ReportDateTime)
				.ToList();
			var trackerIds = fuelLevels.Select(x => x.TrackerId)
				.Distinct()
				.ToList();
            foreach (var trackerId in trackerIds)
            {
				var existingFuelSensor = _db.FuelSensors.FirstOrDefault(x => x.TrackerId.Equals(trackerId));
				if (existingFuelSensor is null)
                {
					_db.FuelSensors.Add(new FuelSensor
					{
						Name = trackerId,
						TrackerId = trackerId
					});
				}
			}
			_db.SaveChanges();

			var fuelSensors = _db.FuelSensors.ToList();

			var fuelSensorRawDataList = new List<FuelSensorRawData>();
			foreach (var fuelLevel in fuelLevels)
			{
				var fuelSensor = fuelSensors.Find(x => x.TrackerId.Equals(fuelLevel.TrackerId));
				if (fuelSensor != null)
				{
					var newFuelSensorRawData = new FuelSensorRawData
					{
						CreationDateTime = DateTimeOffset.Now,
						ReportDateTime = fuelLevel.ReportDateTime,
						FuelSensorId = fuelSensor.Id,
						RawValue = fuelLevel.RawValue,
					};
					fuelSensorRawDataList.Add(newFuelSensorRawData);
				}
			}
			foreach (var fuelSensorRawData in fuelSensorRawDataList)
			{
				_db.FuelSensorRawData.Add(fuelSensorRawData);
			}
			_db.SaveChanges();
		}

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
