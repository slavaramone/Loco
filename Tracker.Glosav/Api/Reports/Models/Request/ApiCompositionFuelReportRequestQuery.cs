using System;
using System.Collections.Generic;
using System.Text;

namespace Tracker.Glosav.Api.Reports.Models.Request
{
	public class ApiCompositionFuelReportRequestQuery
	{
		/// <summary>
		/// ID устройства
		/// </summary>
		public int DeviceId { get; set; }

		/// <summary>
		/// ID оператора
		/// </summary>
		public int OperatorId { get; set; }

		/// <summary>
		/// Конец временного интервала
		/// </summary>
		public DateTime ToTime { get; set; }

		/// <summary>
		/// Начало временного интервала
		/// </summary>
		public DateTime FromTime { get; set; }

		/// <summary>
		/// Окно сглаживания, минут
		/// </summary>
		public int SmoothingWindowMinutes { get; set; }
	}
}
