using System.Collections.Generic;

namespace Contracts.Responses
{
    /// <summary>
    /// Информация о локомотивах и установленных на них камерах
    /// </summary>
    public class LocoInfosResponse
    {
        /// <summary>
        /// Список локомотивов
        /// </summary>
        public List<LocoContract> Locos { get; set; }
    }
}
