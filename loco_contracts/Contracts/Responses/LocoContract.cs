using System;
using System.Collections.Generic;

namespace Contracts.Responses
{
    /// <summary>
    /// Контракт локомотива
    /// </summary>
    public class LocoContract
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; set; }

		/// <summary>
		/// Id локо на карте
		/// </summary>
		public Guid MapItemId { get; set; }

		/// <summary>
		/// Установленные камеры
		/// </summary>
		public List<CameraContract> Cameras { get; set; }

        /// <summary>
        /// Установленные группы датчиков
        /// </summary>
        public List<SensorGroupContract> SensorGroups { get; set; }
    }
}
