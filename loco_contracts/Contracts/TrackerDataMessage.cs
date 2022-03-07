using Contracts.Enums;
using System;

namespace Contracts
{
    /// <summary>
    /// Сообщение с данными gps трекера
    /// </summary>
    public class TrackerDataMessage
    {
        /// <summary>
        /// Id трекера
        /// </summary>
        public string TrackerId { get; set; }

        /// <summary>
        /// Тип данных трекера (сырые, уточненные)
        /// </summary>
        public TrackerDataType DataType { get; set; }        

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
