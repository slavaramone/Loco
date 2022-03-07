namespace Contracts.Responses
{
	/// <summary>
	/// Контракт элемента списка видеопотоков камер локомотива
	/// </summary>
	public class LocoVideoStreamContract
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
