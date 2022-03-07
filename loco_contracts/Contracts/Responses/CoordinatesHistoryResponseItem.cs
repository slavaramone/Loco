using System;

namespace Contracts.Responses
{
	/// <summary>
	/// Элемент ответ истории координат локо
	/// </summary>
	public class CoordinatesHistoryResponseItem
	{
		/// <summary>
		/// Время измерения координат
		/// </summary>
		public DateTimeOffset Timestamp { get; set; }

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
		/// Средняя скорость
		/// </summary>
		public double? Speed { get; set; }

		/// <summary>
		/// Макс скорость за интервал
		/// </summary>
		public double? MaxSpeed { get; set; }

		/// <summary>
		/// Мин скорость за интервал
		/// </summary>
		public double? MinSpeed { get; set; }
	}
}
