using Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Security.Cryptography;

namespace Contracts.Responses
{
    /// <summary>
    /// Ответ авторизации пользователя
    /// </summary>
    public class AuthResponse
    {
        /// <summary>
        /// Id пользователя
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Эл.почта
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Телефон
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Роли пользователя
        /// </summary>
        public List<UserRole> UserRoles { get; set; }
    }
}
