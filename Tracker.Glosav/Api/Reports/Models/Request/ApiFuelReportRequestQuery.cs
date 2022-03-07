using System;

namespace Tracker.Glosav.Api.Reports.Models.Request
{
	public class ApiFuelReportRequestQuery
	{
		/// <summary>
		/// Конец временного интервала
		/// </summary>
		public DateTime ToTime { get; set; }

		/// <summary>
		/// Начало временного интервала
		/// </summary>
		public DateTime FromTime { get; set; }

		/// <summary>
		/// Суммарные данные по всем объектам в отчете
		/// </summary>
		public bool Sum { get; set; }
	}
}
