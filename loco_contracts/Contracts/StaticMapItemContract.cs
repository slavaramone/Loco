using Contracts.Enums;
using System;

namespace Contracts
{
    /// <summary>
    /// Контракт статического объекта на карте
    /// </summary>
    public class StaticMapItemContract
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Тип объекта
        /// </summary>
        public MapItemType Type { get; set; }

        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Широта
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// Долгота
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// Высота
        /// </summary>
        public double? Altitude { get; set; }
    }
}
