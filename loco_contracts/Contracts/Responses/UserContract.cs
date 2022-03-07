using System;

namespace Contracts.Responses
{
    /// <summary>
    /// Контракт пользователя
    /// </summary>
    public class UserContract
    {
        /// <summary>
        /// Id пользователя
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTimeOffset CreationDateTimeUtc { get; set; }

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
        /// Статус активации
        /// </summary>
        public bool IsActive { get; set; }
    }
}
