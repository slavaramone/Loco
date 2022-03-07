using System;

namespace Management.Ui.Controllers
{
    /// <summary>
    /// Контракт элемента списка данных отчета локомотивов
    /// </summary>
    public class LocoCoordReportResponse
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
        /// Долготоа
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
