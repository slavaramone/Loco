using System;

namespace SharedLib.Options
{
    /// <summary>
    /// Опции авторизации
    /// </summary>
    public class AuthOptions
    {
        public const string Auth = "Auth";

        /// <summary>
        /// Ключ используемый при формировании JWT токена
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Время жизни токена
        /// </summary>
        public TimeSpan? Lifetime { get; set; }
    }
}
