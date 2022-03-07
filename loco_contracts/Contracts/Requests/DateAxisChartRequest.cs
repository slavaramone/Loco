using Contracts.Enums;
using System;

namespace Contracts.Requests
{
	/// <summary>
	/// Запрос данных графика скорости и уровню топлива
	/// </summary>
	public class DateAxisChartRequest
	{
		/// <summary>
		/// Id локомотива
		/// </summary>
		public Guid LocoId { get; set; }

		/// <summary>
		/// Начало периода
		/// </summary>
		public DateTimeOffset StartDateTime { get; set; }

		/// <summary>
		/// Конец периода
		/// </summary>
		public DateTimeOffset EndDateTime { get; set; }

		/// <summary>
		/// Тип графика (дата-скорость, дата-уровень топлива и тд)
		/// </summary>
		public ChartType Type { get; set; }

		/// <summary>
		/// Получать данные как есть
		/// </summary>
		public bool? IsRawData { get; set; }
    }
}
