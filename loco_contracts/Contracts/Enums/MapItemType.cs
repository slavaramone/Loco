using System.ComponentModel;

namespace Contracts.Enums
{
    /// <summary>
    /// Тип объекта на карте
    /// </summary>
    public enum MapItemType
    {
        /// <summary>
        /// Неопределено
        /// </summary>
        Undefined = 0,
        
        /// <summary>
        /// Семафор зеленый цвет
        /// </summary>
        [Description("Семафор")]
        TrafficLight = 1,

        /// <summary>
        /// Башмак
        /// </summary>
        [Description("Башмак")]
        Brake,

        /// <summary>
        /// Стрелка
        /// </summary>
        [Description("Стрелка")]
        Arrow,

        /// <summary>
        /// Сцепщик
        /// </summary>
        [Description("Составитель")]
        Shunter,

        /// <summary>
        /// Информация
        /// </summary>
        [Description("Маневровый тепловоз")]
        ShuntingLocomotive
    }
}
