using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.Responses
{
    /// <summary>
    /// Ответ калибровки данных показаний дут для отчета
    /// </summary>
    public class FuelReportCalibrationResponse
    {
        /// <summary>
        /// Список откалиброванных показаний дут
        /// </summary>
        public List<LocoReportFuelItemContract> CalibratedFuelItems { get; set; }
    }
}
