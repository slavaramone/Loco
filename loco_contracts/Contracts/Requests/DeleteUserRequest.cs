using System;

namespace Contracts.Requests
{
    /// <summary>
    /// Запрос удаления пользователя
    /// </summary>
    public class DeleteUserRequest
    {
        /// <summary>
        /// Id пользователя
        /// </summary>
        public Guid Id { get; set; }
    }
}
