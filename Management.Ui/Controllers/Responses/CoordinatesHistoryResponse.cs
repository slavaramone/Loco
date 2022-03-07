using System;

namespace Management.Ui.Controllers
{
	/// <summary>
	/// Ответ истории координат локо
	/// </summary>
	public class CoordinatesHistoryResponse
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
	}
}
