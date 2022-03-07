using System.Collections.Generic;

namespace Contracts.Responses
{
	/// <summary>
	/// Ответ истории топлива
	/// </summary>
	public class FuelHistoryResponse
	{
		/// <summary>
		/// Элементы ответа
		/// </summary>
		public List<FuelHistoryResponseItem> Items { get; set; }
	}
}
