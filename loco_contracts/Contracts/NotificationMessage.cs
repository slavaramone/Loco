using Contracts.Enums;
using System;
using System.Text.Json;

namespace Contracts
{
    /// <summary>
    /// Уведомление
    /// </summary>
    public class NotificationMessage
    {
        /// <summary>
        /// Id локомотива
        /// </summary>
        public Guid LocoId { get; set; }

        /// <summary>
        /// Тип уведомления
        /// </summary>
        public Severity Severity { get; set; }

        /// <summary>
        /// Тип уведомления
        /// </summary>
        public NotificationType Type { get; set; }

        /// <summary>
        /// Опциональное сообщение
        /// </summary>
        public string Message { get; set; }
		
		/// <summary>
		/// Дата и время уведомления
		/// </summary>
		public DateTimeOffset TrackDate { get; set; }
	}
}