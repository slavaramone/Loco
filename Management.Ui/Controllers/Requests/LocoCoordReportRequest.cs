using System;
using System.Collections.Generic;

namespace Management.Ui.Controllers
{
    /// <summary>
    /// Запрос данных для отчета показаний координат
    /// </summary>
    public class LocoCoordReportRequest : CommonFilterRequest
    {
        /// <summary>
        /// Id локомотивов
        /// </summary>
        public List<Guid> LocoIds { get; set; } = new List<Guid>();
    }
}
