using System;

namespace Management.Ui.Controllers
{
    /// <summary>
    /// Ответ авторизации пользователя
    /// </summary>
    public class AuthResponse
    {
        /// <summary>
        /// Bearer token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Время когда токен станет недействительным
        /// </summary>
        public DateTime ExpirationDateTime { get; set; }
    }
}
