using Contracts.Enums;
using System;

namespace Management.Ui.Controllers
{
	/// <summary>
	/// Ответ истории уведомлений
	/// </summary>
	public class NotificationHistoryResponse
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
