using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.Requests
{
    /// <summary>
    /// Запрос данных для отчета показаний координат
    /// </summary>
    public class LocoCoordReportRequest : CommonFilterContract
    {
        /// <summary>
        /// Id локомотивов
        /// </summary>
        public List<Guid> LocoIds { get; set; } = new List<Guid>();
    }
}
