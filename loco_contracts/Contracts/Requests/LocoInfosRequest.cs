using System;
using System.Collections.Generic;

namespace Contracts.Requests
{
    /// <summary>
    /// Запрос информации о локомотивах и установленных на них камерах
    /// </summary>
    public class LocoInfosRequest
    {
        /// <summary>
        /// Фильтр по Id локо
        /// </summary>
        public List<Guid> LocoIds { get; set; } = new List<Guid>();
    }
}
