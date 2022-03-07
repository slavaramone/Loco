using System.Collections.Generic;

namespace Contracts.Responses
{
	/// <summary>
	/// Ответ истории координат локо
	/// </summary>
	public class CoordinatesHistoryResponse
	{
		/// <summary>
		/// Элементы ответа
		/// </summary>
		public List<CoordinatesHistoryResponseItem> Items { get; set; }
	}
}
