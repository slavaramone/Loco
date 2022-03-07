namespace Contracts
{
	/// <summary>
	/// Метаданные события по превышению скорости
	/// </summary>
	public class NotificationSpeedMetadata
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
		public double? Speed { get; set; }

		/// <summary>
		/// Скорость
		/// </summary>
		public double? SpeedLimit { get; set; }
	}
}
