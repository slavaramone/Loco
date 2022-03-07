using Contracts.Enums;
using System;

namespace Management.Ui.Controllers
{
    /// <summary>
    /// Элемент списка видео
    /// </summary>
    public class VideoListItem
    {
        /// <summary>
        /// Id локо
        /// </summary>
        public Guid LocoId { get; set; }

        /// <summary>
        /// Url файла
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Время начала записи
        /// </summary>
        public DateTimeOffset StartDateTime { get; set; }

        /// <summary>
        /// Время окончания записи
        /// </summary>
        public DateTimeOffset EndDateTime { get; set; }

        /// <summary>
        /// Позиция камеры на локо
        /// </summary>
        public int CameraPosition { get; set; }
    }
}
