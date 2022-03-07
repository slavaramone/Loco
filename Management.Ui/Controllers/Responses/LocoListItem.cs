using System;

namespace Management.Ui.Controllers
{
    /// <summary>
    /// Элемента ответа списка локомотивов
    /// </summary>
    public class LocoListItem
    {
        /// <summary>
        /// Id локо
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; set; }

		/// <summary>
		/// Признак активации
		/// </summary>
		public bool IsActive { get; set; }
    }
}
