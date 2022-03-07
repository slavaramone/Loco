using System;

namespace Management.Ui.Controllers
{
	/// <summary>
	/// Ответ истории топлива
	/// </summary>
	public class FuelHistoryResponse
	{
		/// <summary>
		/// Время измерения показаний топлива
		/// </summary>
		public DateTimeOffset Timestamp { get; set; }

		/// <summary>
		/// Значение показания
		/// </summary>
		public double Value { get; set; }

		/// <summary>
		/// Макс значение показания за интервал
		/// </summary>
		public double MaxValue { get; set; }

		/// <summary>
		/// Мин значение показания за интервал
		/// </summary>
		public double MinValue { get; set; }
	}
}
