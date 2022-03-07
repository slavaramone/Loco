using System.Collections.Generic;

namespace Contracts.Responses
{
    /// <summary>
    /// Ответ списка уведомлений
    /// </summary>
    public class NotificationListResponse
    {
		/// <summary>
		/// Список описаний уведомлений
		/// </summary>
		public List<NotificationListItemContract> NotificationList { get; set; } = new List<NotificationListItemContract>();

        /// <summary>
		/// Всего элементов в списке (для пагинации)
		/// </summary>

		public int Total { get; set; }
	}
}
