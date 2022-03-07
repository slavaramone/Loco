using System.Collections.Generic;

namespace Contracts.Responses
{
    /// <summary>
    /// Ответ информации о всех объектах на карте
    /// </summary>
    public class InitMapDataResponse
    {
        /// <summary>
        /// Список описаний объектов на карте
        /// </summary>
        public List<MapItemContract> MapItems { get; set; }

        /// <summary>
        /// "Сырые" значения ДУТ локомотивов
        /// </summary>
        public List<FuelLevelContract> FuelLevels { get; set; }
    }
}
