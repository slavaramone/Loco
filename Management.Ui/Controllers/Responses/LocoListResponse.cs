using System.Collections.Generic;

namespace Management.Ui.Controllers
{
    /// <summary>
    /// Ответ списка локомотивов
    /// </summary>
    public class LocoListResponse
    {
        /// <summary>
        /// Список инфо о локо
        /// </summary>
        public List<LocoListItem> LocoList { get; set; }
    }
}
