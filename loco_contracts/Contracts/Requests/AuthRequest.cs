namespace Contracts.Requests
{
    /// <summary>
    /// Запрос аутентификация пользователя
    /// </summary>
    public class AuthRequest
    {
        /// <summary>
        /// Эл.почта
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Телефон
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Код доступа
        /// </summary>
        public string Code { get; set; }
    }
}
