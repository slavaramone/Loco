using Microsoft.EntityFrameworkCore.Migrations;
using Notification.Db.Entities;

namespace Notification.Db.Migrations
{
    public partial class AddedSpeedZonesData : Migration
    {
		private readonly NotificationDbContext _db;

		public AddedSpeedZonesData(NotificationDbContext db)
		{
			_db = db;
		}

		protected override void Up(MigrationBuilder migrationBuilder)
        {
			_db.SpeedZones.Add(new SpeedZone
			{
				LatitudeTopLeft = 55.878380, LongitudeTopLeft = 37.602575,
				LatitudeTopRight = 55.879720, LongitudeTopRight = 37.606797,
				LatitudeBottomRight = 55.879221, LongitudeBottomRight = 37.607083,
				LatitudeBottomLeft = 55.878264, LongitudeBottomLeft = 37.602658,
				MaxSpeed = 25
			});

			_db.SpeedZones.Add(new SpeedZone
			{
				LatitudeTopLeft = 55.879720, LongitudeTopLeft = 37.606797,
				LatitudeTopRight = 55.880681, LongitudeTopRight = 37.614784,
				LatitudeBottomRight = 55.879794, LongitudeBottomRight = 37.614963,
				LatitudeBottomLeft = 55.879221, LongitudeBottomLeft = 37.607083,
				MaxSpeed = 20
			});

			_db.SpeedZones.Add(new SpeedZone
			{
				LatitudeTopLeft = 55.880681, LongitudeTopLeft = 37.614784,
				LatitudeTopRight = 55.880367, LongitudeTopRight = 37.616096,
				LatitudeBottomRight = 55.880243, LongitudeBottomRight = 37.616021,
				LatitudeBottomLeft = 55.879794, LongitudeBottomLeft = 37.614963,
				MaxSpeed = 15
			});

			_db.SpeedZones.Add(new SpeedZone
			{
				LatitudeTopLeft = 55.880367, LongitudeTopLeft = 37.616096,
				LatitudeTopRight = 55.880437, LongitudeTopRight = 37.616972,
				LatitudeBottomRight = 55.880313, LongitudeBottomRight = 37.617008,
				LatitudeBottomLeft = 55.880243, LongitudeBottomLeft = 37.616021,
				MaxSpeed = 5
			});

			_db.SpeedZones.Add(new SpeedZone
			{
				LatitudeTopLeft = 55.880437, LongitudeTopLeft = 37.616972,
				LatitudeTopRight = 55.881494, LongitudeTopRight = 37.619843,
				LatitudeBottomRight = 55.880260, LongitudeBottomRight = 37.620059,
				LatitudeBottomLeft = 55.880313, LongitudeBottomLeft = 37.617008,
				MaxSpeed = 15
			});

			_db.SaveChanges();
		}

		protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
