using Microsoft.EntityFrameworkCore.Migrations;
using Notification.Db.Entities;
using System.Linq;

namespace Notification.Db.Migrations
{
    public partial class ChangedSpeedZones : Migration
    {
		private readonly NotificationDbContext _db;

		public ChangedSpeedZones(NotificationDbContext db)
		{
			_db = db;
		}

		protected override void Up(MigrationBuilder migrationBuilder)
        {
			var speedZone20 = _db.SpeedZones.FirstOrDefault(x => x.LatitudeTopLeft == 55.87972 && x.LongitudeTopLeft == 37.606797 && x.MaxSpeed == 20);
			if (speedZone20 != null)
			{
				speedZone20.LatitudeTopRight = 55.880200;
				speedZone20.LongitudeTopRight = 37.608482;

				speedZone20.LatitudeBottomRight = 55.879461;
				speedZone20.LongitudeBottomRight = 37.608961;

				_db.Update(speedZone20);

				_db.SpeedZones.Add(new SpeedZone
				{
					LatitudeTopLeft = 55.880200,
					LongitudeTopLeft = 37.608482,

					LatitudeTopRight = 55.880681,
					LongitudeTopRight = 37.614784,

					LatitudeBottomRight = 55.879794,
					LongitudeBottomRight = 37.614963,

					LatitudeBottomLeft = 55.879461,
					LongitudeBottomLeft = 37.608961,
					MaxSpeed = 25
				});
			}

			_db.SaveChanges();
		}

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
