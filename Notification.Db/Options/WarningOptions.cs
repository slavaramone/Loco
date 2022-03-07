namespace Notification.Db.Options
{
    /// <summary>
    /// Границы выдачи предупреждений для различных событий
    /// </summary>
    public class WarningOptions
    {
        public const string Warning = "Warning";

        /// <summary>
        /// Расстояние в м до башмака
        /// </summary>
        public int BreakMeters { get; set; }

        /// <summary>
        /// Расстояние в м до стрелки
        /// </summary>
        public int ArrowMeters { get; set; }

        /// <summary>
        /// Макс. допустимая скорость
        /// </summary>
        public int SpeedKmPerHour { get; set; }
    }
}
