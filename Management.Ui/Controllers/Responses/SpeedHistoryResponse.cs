using System;

namespace Management.Ui.Controllers
{
	/// <summary>
	/// Ответ истории скорости локо
	/// </summary>
	public class SpeedHistoryResponse
	{
		/// <summary>
		/// Время измерения скорости
		/// </summary>
		public DateTimeOffset Timestamp { get; set; }

		/// <summary>
		/// Скорость
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
