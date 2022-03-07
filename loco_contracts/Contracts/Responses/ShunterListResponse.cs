using System.Collections.Generic;

namespace Contracts.Responses
{
    /// <summary>
    /// Ответ списка сцепщиков
    /// </summary>
    public class ShunterListResponse
    {
        /// <summary>
        /// Список сцепщиков
        /// </summary>
        public List<ShunterListItemContract> ShunterList { get; set; }
    }
}
