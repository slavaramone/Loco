using System;

namespace Contracts
{
    /// <summary>
    /// Контракт "сырых" показаний ДУТ
    /// </summary>
    public class FuelLevelContract
    {
        /// <summary>
        /// Id датчика
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Внешний Id датчика
        /// </summary>
        public Guid FuelSensorId { get; set; }

        /// <summary>
        /// "Сырое" значение ДУТ
        /// </summary>
        public double RawValue { get; set; }

        /// <summary>
        /// Дата снятия показаний
        /// </summary>
        public DateTimeOffset ReportDateTime { get; set; }
    }
}
