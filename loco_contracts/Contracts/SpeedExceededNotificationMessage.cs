namespace Contracts
{
	public class SpeedExceededNotificationMessage : NotificationMessage
	{
		/// <summary>
		/// Широта
		/// </summary>
		public double Latitude { get; set; }
		
		/// <summary>
		/// Долгота
		/// </summary>
		public double Longitude { get; set; }
		
		/// <summary>
		/// Высота
		/// </summary>
		public double? Altitude { get; set; }
		
		/// <summary>
		/// Скорость
		/// </summary>
		public double Speed { get; set; }
		
		/// <summary>
		/// Макс скорость разрешенная на участке
		/// </summary>
		public double? MaxSpeed { get; set; }
	}
}