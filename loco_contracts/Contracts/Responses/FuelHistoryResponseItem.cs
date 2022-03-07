using System;

namespace Contracts.Responses
{
	/// <summary>
	/// Элемент ответа истории топлива
	/// </summary>
	public class FuelHistoryResponseItem
	{
		/// <summary>
		/// Время измерения показаний топлива
		/// </summary>
		public DateTimeOffset Timestamp { get; set; }

		/// <summary>
		/// Среднее значение показания
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
