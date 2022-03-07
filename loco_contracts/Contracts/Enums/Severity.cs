using System.ComponentModel;

namespace Contracts.Enums
{
    /// <summary>
    /// Важность уведомления
    /// </summary>
    public enum Severity
    {
        /// <summary>
        /// ЧС
        /// </summary>
        [Description("ЧС")]
        Emergency = 1,

        /// <summary>
        /// Тревога
        /// </summary>
        [Description("Тревога")]
        Alert,

        /// <summary>
        /// Внимание
        /// </summary>
        [Description("Внимание")]
        Warning,

        /// <summary>
        /// Информация
        /// </summary>
        [Description("Информация")]
        Info
    }
}
