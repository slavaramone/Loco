using System;

namespace Contracts
{
    /// <summary>
    /// Сообщение калибровки "сырых" данных от ДУТ
    /// </summary>
    public class FuelLevelDataCalibrationMessage
    {
        /// <summary>
        /// "Сырые" показания ДУТ
        /// </summary>
        public double FuelLevel { get; set; }

        /// <summary>
        /// Внешний Id ДУТ
        /// </summary>
        public Guid FuelSensorId { get; set; }

        /// <summary>
        /// Время снятия показаний
        /// </summary>
        public DateTimeOffset ReportDateTime { get; set; }
    }
}
