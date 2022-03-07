using System;

namespace Management.Ui.Controllers
{
	/// <summary>
	/// Ответ данных графика скорости и уровню топлива
	/// </summary>
	public class DateAxisChartResponse
	{
		/// <summary>
		/// Дата Пример 2020-09-18T11:52:45.876795
		/// </summary>
		public DateTimeOffset DateTime { get; set; }

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
