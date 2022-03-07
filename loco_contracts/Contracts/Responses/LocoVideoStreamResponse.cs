using System.Collections.Generic;

namespace Contracts.Responses
{
    /// <summary>
    /// Ответ списка видеопотоков камер локомотива
    /// </summary>
    public class LocoVideoStreamResponse
    {
        /// <summary>
        /// Список описаний видеопотоков
        /// </summary>
        public List<LocoVideoStreamContract> VideoStreams { get; set; }
    }
}
