using Contracts.Enums;
using Newtonsoft.Json;
using System;
using System.Text.Json;

namespace Contracts.Responses
{
	/// <summary>
	/// Контракт элемента списка уведомлений
	/// </summary>
	public class NotificationListItemContract
	{
		/// <summary>
		/// Id уведомления
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Важность удомления
		/// </summary>
		public Severity Severity { get; set; }

		/// <summary>
		/// Тип уведомления
		/// </summary>
		public NotificationType Type { get; set; }

		/// <summary>
		/// Дата создания
		/// </summary>
		public DateTimeOffset CreationDateTime { get; set; }

		/// <summary>
		/// Сообщение
		/// </summary>
		public string Message { get; set; }

		/// <summary>
		/// Метаданные
		/// </summary>
		public object Metadata { get; set; }
	}
}
