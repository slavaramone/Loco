using Contracts.Enums;
using System.Collections.Generic;

namespace Contracts.Requests
{
    /// <summary>
    /// Запрос добавления пользователя
    /// </summary>
    public class AddUserRequest
    {
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
