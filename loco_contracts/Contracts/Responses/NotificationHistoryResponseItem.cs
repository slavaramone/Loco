using Contracts.Enums;
using System;

namespace Contracts.Responses
{
	/// <summary>
	/// Элемент ответа истории уведомлений
	/// </summary>
	public class NotificationHistoryResponseItem
	{
		/// <summary>
		/// Время уведомления
		/// </summary>
		public DateTimeOffset Timestamp { get; set; }


		/// <summary>
		/// Тип уведомления
		/// </summary>
		public NotificationType Type { get; set; }

		/// <summary>
		/// Важность уведомления
		/// </summary>
		public Severity Severity { get; set; }

		/// <summary>
		/// Метаданные
		/// </summary>
		public object Metadata { get; set; }
	}
}
