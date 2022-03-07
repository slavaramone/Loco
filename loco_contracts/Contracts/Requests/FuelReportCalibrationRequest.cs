using Contracts.Responses;
using System.Collections.Generic;

namespace Contracts.Requests
{
    /// <summary>
    /// Запрос калибровки "сырых" показаний ДУТ
    /// </summary>
    public class FuelReportCalibrationRequest
    {
        /// <summary>
        /// Список "сырых" показаний ДУТ
        /// </summary>
        public List<SensorReportFuelItemContract> FuelItems { get; set; }
    }
}
