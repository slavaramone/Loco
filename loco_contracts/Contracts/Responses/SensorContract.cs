using System;

namespace Contracts.Responses
{
    /// <summary>
    /// Контракт ДУТ
    /// </summary>
    public class SensorContract
    {
        /// <summary>
        /// Id дут
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Внешний Id
        /// </summary>
        public Guid FuelSensorId { get; set; }

        /// <summary>
        /// Id группы датчиков
        /// </summary>
        public Guid SensorGroupId { get; set; }
    }
}
