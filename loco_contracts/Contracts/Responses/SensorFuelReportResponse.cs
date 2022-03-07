using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.Responses
{
    /// <summary>
    /// Ответ данных для отчета показаний дут
    /// </summary>
    public class SensorFuelReportResponse
    {
        /// <summary>
        /// Список показаний дут
        /// </summary>
        public List<SensorReportFuelItemContract> FuelItems { get; set; }
    }
}
