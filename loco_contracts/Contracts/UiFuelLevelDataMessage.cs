using Contracts.Enums;
using System;

namespace Contracts
{
    /// <summary>
    /// Сообщение с данными Датчика Уровня Топлива
    /// </summary>
    public class UiFuelLevelDataMessage
    {
        /// <summary>
        /// Id объекта на карте (MapItemId)
        /// </summary>
        public Guid LocoId { get; set; }

        /// <summary>
        /// Значение ДУТ
        /// </summary>
        public double CurrentValue { get; set; }

        /// <summary>
        /// Время фиксации показаний ДУТ
        /// </summary>
        public DateTimeOffset ReportDateTime { get; set; }
    }
}
