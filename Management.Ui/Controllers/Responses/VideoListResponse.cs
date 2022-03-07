using System.Collections.Generic;

namespace Management.Ui.Controllers
{
    /// <summary>
    /// Ответ список видеофайлов
    /// </summary>
    public class VideoListResponse
    {
        /// <summary>
        /// Список видео
        /// </summary>
        public List<VideoListItem> VideoList { get; set; }

        /// <summary>
        /// Всего видео без фильтрации
        /// </summary>
        public int Total { get; set; }
    }
}
