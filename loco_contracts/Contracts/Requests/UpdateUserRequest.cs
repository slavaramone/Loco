using System;

namespace Contracts.Requests
{
    /// <summary>
    /// Запрос обновления пароля и активации
    /// </summary>
    public class UpdateUserRequest
    {
        /// <summary>
        /// Имя
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Статус активации (если не указан, не меняется)
        /// </summary>
        public bool? IsActive { get; set; }

        /// <summary>
        /// Новый пароль
        /// </summary>
        public string NewPassword { get; set; }
    }
}
