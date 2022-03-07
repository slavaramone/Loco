using System;

namespace Contracts.Responses
{
    /// <summary>
    /// Ответ апдейта пользователя
    /// </summary>
    public class UpdateUserResponse
    {
        /// <summary>
        /// Id пользователя
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Логин
        /// </summary>
        public string Login { get; set; }
    }
}
