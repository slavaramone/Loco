using System;
using System.Collections.Generic;

namespace Contracts.Responses
{
    /// <summary>
    /// Ответ списка пользователей
    /// </summary>
    public class UserListResponse
    {
        /// <summary>
        /// Список
        /// </summary>
        public List<UserContract> Users { get; set; }
    }
}
