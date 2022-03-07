using System;
using System.Security.Cryptography;

namespace Notification.Db.Entities
{
	public class SpeedExceededNotification : Notification
	{
		public double Latitude { get; set; }
		
		public double Longitude { get; set; }
		
		public double Altitude { get; set; }
		
		public double Speed { get; set; }
		
		public double MaxSpeed { get; set; }
	}
}