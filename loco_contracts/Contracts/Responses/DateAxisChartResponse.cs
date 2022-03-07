using System.Collections.Generic;

namespace Contracts.Responses
{
	/// <summary>
	/// Ответ данных графика скорости и уровню топлива
	/// </summary>
	public class DateAxisChartResponse
	{
		/// <summary>
		/// Список значений для графика
		/// </summary>
		public List<LocoChartItemContract> ChartItems { get; set; }
	}
}
