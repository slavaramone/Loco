using System.ComponentModel;

namespace Contracts.Enums
{
    /// <summary>
    /// Тип уведомления
    /// </summary>
    public enum NotificationType
    {
        /// <summary>
        /// Уведомление с кастомных сообщением
        /// </summary>
        Custom = 0,

        /// <summary>
        /// Препятствие на путях
        /// </summary>
        [Description("Препятствие на путях!")]
        Obstacle,

        /// <summary>
        /// Человек на путях
        /// </summary>
        [Description("Человек на путях!")]
        Person,

        /// <summary>
        /// Направление стрелки
        /// </summary>
        [Description("Направление стрелки!")]
        SwithDirection,

        /// <summary>
        /// Красный сигнал семафора
        /// </summary>
        [Description("Красный сигнал семафора!")]
        SemaphoreRed,

        /// <summary>
        /// Превышена скорость
        /// </summary>
        [Description("Вы превысили предельно допустимую скорость!")]
        Speed,

        /// <summary>
        /// Приближение к башмаку
        /// </summary>
        [Description("Вы приближаетесь к тормозному башмаку!")]
        Brake,

        /// <summary>
        /// Приближение к стрелке
        /// </summary>
        [Description("Вы приближаетесь к стрелке!")]
        Arrow
    }
}
