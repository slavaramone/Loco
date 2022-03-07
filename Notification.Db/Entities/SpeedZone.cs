using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Notification.Db.Entities
{
	[Table("SpeedZones")]
	public class SpeedZone
	{
		public Guid Id { get; set; }

		public double LatitudeTopLeft { get; set; }

		public double LongitudeTopLeft { get; set; }

		public double LatitudeTopRight { get; set; }

		public double LongitudeTopRight { get; set; }

		public double LatitudeBottomRight { get; set; }

		public double LongitudeBottomRight { get; set; }

		public double LatitudeBottomLeft { get; set; }

		public double LongitudeBottomLeft { get; set; }

		public double MaxSpeed { get; set; }
	}
}
