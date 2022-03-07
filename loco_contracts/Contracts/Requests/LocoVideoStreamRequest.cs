using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.Requests
{
    /// <summary>
    /// Запрос списка видеопотоков камер локомотива
    /// </summary>
    public class LocoVideoStreamRequest
    {
        /// <summary>
        /// Id локо
        /// </summary>
        public Guid LocoId { get; set; }
    }
}
