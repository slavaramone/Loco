using System;

namespace Contracts.Responses
{
    public class SensorReportFuelItemContract
    {
        /// <summary>
        /// Внешний Id ДУТ
        /// </summary>
        public Guid FuelSensorId { get; set; }

        /// <summary>
        /// Дата зписи
        /// </summary>
        public DateTimeOffset ReportDateTime { get; set; }

        /// <summary>
        /// Сырое значение датчика топлива
        /// </summary>
        public double RawValue { get; set; }
    }
}
