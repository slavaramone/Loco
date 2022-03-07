using System;

namespace Contracts.Responses
{
    /// <summary>
    /// Контракт элемента списка данных отчета локомотивов
    /// </summary>
    public class LocoReportCoordItemContract
    {
        /// <summary>
        /// Id локомотива
        /// </summary>
        public Guid LocoId { get; set; }

        /// <summary>
        /// Дата зписи
        /// </summary>
        public DateTime TrackDateTime { get; set; }

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

    }
}
