namespace Management.Ui.Controllers
{
    /// <summary>
    /// Ответ ссылки на видеоархив
    /// </summary>
    public class VideoLinkResponse
    {
        /// <summary>
        /// Url для просмотра
        /// </summary>
        public string ViewUrl { get; set; }

        /// <summary>
        /// Url для скачивания
        /// </summary>
        public string DownloadUrl { get; set; }
    }
}
