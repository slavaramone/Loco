using System;
using System.Collections.Generic;

namespace Contracts.Responses
{
    /// <summary>
    /// Контракт группы датчиков
    /// </summary>
    public class SensorGroupContract
    {
        /// <summary>
        /// Id группы
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Использовать среднее значение по группе да/нет
        /// </summary>
        public bool IsTakeAverageValue { get; set; }

        /// <summary>
        /// Id локо на котором установлены
        /// </summary>
        public Guid LocoId { get; set; }

        /// <summary>
        /// Установленные датчики
        /// </summary>
        public List<SensorContract> Sensors { get; set; }
    }
}
