using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.Requests
{
    /// <summary>
    /// Запрос данных для отчета показаний дут
    /// </summary>
    public class SensorFuelReportRequest : CommonFilterContract
    {
        /// <summary>
        /// Внешние id ДУТ
        /// </summary>
        public List<Guid> FuelSensorIds { get; set; } = new List<Guid>();
    }
}
