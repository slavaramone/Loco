using System.Collections.Generic;

namespace Contracts.Responses
{
	/// <summary>
	/// Ответ истории уведомлений
	/// </summary>
	public class NotificationHistoryResponse
	{
		/// <summary>
		/// Элементы ответа
		/// </summary>
		public List<NotificationHistoryResponseItem> Items { get; set; }
	}
}
