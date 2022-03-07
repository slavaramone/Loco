namespace Management.Ui.Options
{
    /// <summary>
    /// Опции видео архива
    /// </summary>
    public class VideoOptions
    {
        public const string Video = "Video";

        /// <summary>
        /// Формат формирования ссылки на видеофайл вида https://xyz.ru/convertFile?file={0} для просмотра
        /// {0} - название файла
        /// </summary>
        public string ViewLinkUrlFormat { get; set; }

        /// <summary>
        /// Формат формирования ссылки на видеофайл вида https://xyz.ru/nuc{0}/video/{1}/{2}/{3} для скачивания
        /// {0} - номер nuc {1} - номер камеры {2} - день месяца  {3} - название файла
        /// </summary>
        public string DownloadLinkUrlFormat { get; set; }

        /// <summary>
        /// Директория на сервере с архивными видео файлами
        /// </summary>
        public string VideoArchiveDir { get; set; }

        /// <summary>
        /// Regex выражение поиска файлов видеоархива
        /// </summary>
        public string SearchRegex { get; set; }

        /// <summary>
        /// Формат имени файла за исключением даты и разрешения видео
        /// </summary>
        public string FileNoDateAndSizeFormat { get; set; }

        /// <summary>
        /// Расширение видео файла
        /// </summary>
        public string SearchPattern { get; set; }

        /// <summary>
        /// Длительность видео в секундах
        /// </summary>
        public int DurationSeconds { get; set; }
    }
}