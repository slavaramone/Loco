using System;

namespace Contracts.Responses
{
    /// <summary>
    /// Контракт эелемента отчета показаний ДУТ
    /// </summary>
    public class LocoReportFuelItemContract
    {
        /// <summary>
        /// MapItemId локо
        /// </summary>
        public Guid LocoId { get; set; }

        /// <summary>
        /// Дата зписи
        /// </summary>
        public DateTimeOffset ReportDateTime { get; set; }

        /// <summary>
        /// Откалиброванное значение ДУТ
        /// </summary>
        public double CalibratedValue { get; set; }
    }
}