namespace Management.Ui.Controllers
{
    /// <summary>
    /// Запрос аутентификация пользователя
    /// </summary>
    public class AuthRequest
    {
        /// <summary>
        /// Логин (имя фамилия или фамилия имя)
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; }
    }
}
