using Contracts.Enums;
using Contracts.Requests;
using SharedLib.Filters;
using System;
using System.Collections.Generic;

namespace Management.Ui.Controllers
{
    /// <summary>
    /// Запрос списка уведомлений по фильтру
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
        /// Сортировка вида field-(desc|asc). Доступные поля: creationdatetime, severity. Пример creationdatetime-desc
        /// </summary>
        [SortValidation(new string[] { "creationdatetime", "severity", "message" }, new string[] { "asc", "desc" })]
        public string[] Sort { get; set; } = new string[0];
    }
}