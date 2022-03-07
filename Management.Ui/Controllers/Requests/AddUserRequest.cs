using Contracts.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Management.Ui.Controllers
{
    /// <summary>
    /// Запрос добавления пользователя
    /// </summary>
    public class AddUserRequest
    {
        /// <summary>
        /// Имя
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string FirstName { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string LastName { get; set; }

        /// <summary>
        /// Логин
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string Password { get; set; }

        /// <summary>
        /// Роли пользователя
        /// </summary>
        [Required]
        public List<UserRole> Roles { get; set; }
    }
}
