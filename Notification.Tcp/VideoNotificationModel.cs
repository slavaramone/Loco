using Contracts.Enums;
using System;
using System.Collections.Generic;

namespace Notification.Tcp
{
    /// <summary>
    /// Json модель данных видео аналитики, поступающих по вебсокету
    /// </summary>
    public class VideoNotificationModel
    {
        /// <summary>
        /// Тип запроса (realtime-event/history)
        /// </summary>
        public string Query { get; set; }

        /// <summary>
        /// Время получения событий
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// IP адреса камер
        /// </summary>
        public List<string> Cams { get; set; }

        /// <summary>
        /// Типы оповещений
        /// </summary>
        public List<NotificationType> Types { get; set; }
    }
}
