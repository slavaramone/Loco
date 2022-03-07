using Contracts.Enums;
using System;
using System.Collections.Generic;

namespace Contracts.Requests
{
    /// <summary>
    /// Запрос получения отфильтрованных уведомлений
    /// </summary>
    public class NotificationListRequest : CommonFilterContract
    {
        /// <summary>
        /// Id локомотивов
        /// </summary>
        public List<Guid> LocoIds { get; set; } = new List<Guid>();

        /// <summary>
        /// Типы уведомлений
        /// </summary>
        public List<NotificationType> NotificationTypes { get; set; } = new List<NotificationType>();

        /// <summary>
        /// Вожность уведомлений
        /// </summary>
        public List<Severity> Severities { get; set; } = new List<Severity>();

        /// <summary>
        /// Сортировка вида field-(desc|asc). Доступные поля: date, severity.
        /// </summary>
        public SortFilterContract[] Sort { get; set; }
    }
}