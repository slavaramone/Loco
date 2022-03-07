using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.Responses
{
    /// <summary>
    /// Ответ данных для отчета показаний координат
    /// </summary>
    public class LocoCoordReportResponse
    {
        /// <summary>
        /// Список координат
        /// </summary>
        public List<LocoReportCoordItemContract> CoordItems { get; set; }
    }
}
