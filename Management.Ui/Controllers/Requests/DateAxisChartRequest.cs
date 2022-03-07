using System;
using System.ComponentModel.DataAnnotations;

namespace Management.Ui.Controllers
{
    /// <summary>
    /// Запрос данных графика скорости и уровню топлива
    /// </summary>
    public class DateAxisChartRequest
	{
		/// <summary>
		/// Начало периода Пример 2020-09-18T11:52:45.876795
		/// </summary>
		[Required]
		public DateTimeOffset? StartDateTime { get; set; }

		/// <summary>
		/// Конец периода Пример 2020-09-18T11:52:45.876795
		/// </summary>
		[Required]
		public DateTimeOffset EndDateTime { get; set; }
	}
}
