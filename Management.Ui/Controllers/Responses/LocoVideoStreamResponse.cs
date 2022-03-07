namespace Management.Ui.Controllers
{
	/// <summary>
	/// Ответ списка видеопотоков камер локомотива
	/// </summary>
	public class LocoVideoStreamResponse
    {
        /// <summary>
        /// Позиция камеры
        /// </summary>
        public int CameraPosition { get; set; }

        /// <summary>
        /// Url просмотра видеопотока
        /// </summary>
        public string Url { get; set; }

		/// <summary>
		/// Url просмотра видеопотока в HD качестве
		/// </summary>
		public string UrlHd { get; set; }
	}
}
