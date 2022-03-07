using System;

namespace Contracts
{
    /// <summary>
    /// Общий контракт объекта карты
    /// </summary>
    public class MapItemContract : StaticMapItemContract
    {
        /// <summary>
        /// Id gps приемника
        /// </summary>
        public string TrackerId { get; set; }

        /// <summary>
        /// Скорость
        /// </summary>
        public double? Speed { get; set; }

        /// <summary>
        /// Направление
        /// </summary>
        public double? Heading { get; set; }

        /// <summary>
        /// Время фиксации координат трекером
        /// </summary>
        public DateTimeOffset TrackDate { get; set; }
    }
}
