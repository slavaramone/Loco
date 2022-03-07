using System;

namespace Contracts.Responses
{
    /// <summary>
    /// Ответ удаления пользователя
    /// </summary>
    public class DeleteUserResponse
    {
        /// <summary>
        /// Id пользователя
        /// </summary>
        public Guid Id { get; set; }
    }
}
