using System.ComponentModel;

namespace Contracts.Enums
{
    /// <summary>
    /// Роли пользователей
    /// </summary>
    public enum UserRole
    {
        /// <summary>
        /// Супер админ
        /// </summary>
        [Description("Супер админ")]
        SuperAdmin = 1,

        /// <summary>
        /// Диспетчер
        /// </summary>
        [Description("Диспетчер")]
        TrafficController = 2,

        /// <summary>
        /// Машинист
        /// </summary>
        [Description("Машинист")]
        Driver = 3,

        /// <summary>
        /// Сцепщик
        /// </summary>
        [Description("Сцепщик")]
        Shunter = 4
    }
}
