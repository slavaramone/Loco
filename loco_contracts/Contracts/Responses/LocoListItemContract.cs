using System;

namespace Contracts.Responses
{
    /// <summary>
    /// Контракт элемента списка локомотивов
    /// </summary>
    public class LocoListItemContract
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
