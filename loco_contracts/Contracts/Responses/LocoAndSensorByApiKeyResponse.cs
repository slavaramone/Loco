using System;

namespace Contracts.Responses
{
    /// <summary>
    /// Ответ получения Id объекта на карте по API ключю
    /// </summary>
    public class LocoAndSensorByApiKeyResponse
    {
        /// <summary>
        /// Id объекта на карте
        /// </summary>
        public Guid? MapItemId { get; set; }

        /// <summary>
        /// Внешний Id дут
        /// </summary>
        public Guid? FuelSensorId { get; set; }

		/// <summary>
		/// Признак активации
		/// </summary>
		public bool IsActive { get; set; }
	}
}
