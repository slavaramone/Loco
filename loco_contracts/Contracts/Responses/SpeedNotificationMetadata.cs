namespace Contracts.Responses
{
	/// <summary>
	/// Метаданные уведомлений о превышении скорости
	/// </summary>
	public class SpeedNotificationMetadata
	{
		/// <summary>
		/// Макс скорость разрешенная на участке
		/// </summary>
		public double? MaxSpeed { get; set; }

		/// <summary>
		/// Текущая скорость
		/// </summary>
		public double? ActualSpeed { get; set; }
	}
}
