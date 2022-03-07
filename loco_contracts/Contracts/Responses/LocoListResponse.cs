using System.Collections.Generic;

namespace Contracts.Responses
{
    /// <summary>
    /// Ответ списка локомотивов
    /// </summary>
    public class LocoListResponse
    {
        /// <summary>
        /// Список инфо о локо
        /// </summary>
        public List<LocoListItemContract> LocoList { get; set; }
    }
}
