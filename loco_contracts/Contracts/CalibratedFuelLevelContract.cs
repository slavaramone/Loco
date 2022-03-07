using System;

namespace Contracts
{
    /// <summary>
    /// Контракт откалиброванных показаний ДУТ
    /// </summary>
    public class CalibratedFuelLevelContract : FuelLevelContract
    {
        /// <summary>
        /// Id локо
        /// </summary>
        public Guid LocoId { get; set; }

        /// <summary>
        /// Откалиброванное значение ДУТ
        /// </summary>
        public double CalibratedValue { get; set; }

        /// <summary>
        /// Максимальное значение ДУТ
        /// </summary>
        public double MaxValue { get; set; }
    }
}
