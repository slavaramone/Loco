using Contracts.Enums;
using System;
using System.Collections.Generic;

namespace Contracts.Responses
{
    /// <summary>
    /// Ответ добавления пользователя
    /// </summary>
    public class AddUserResponse
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

        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Роли пользователя
        /// </summary>
        public List<UserRole> Roles { get; set; }
    }
}
