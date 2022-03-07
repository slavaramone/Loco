namespace Tracker.Db
{
    /// <summary>
    /// Настройки фильтрации координат, отправляемых в UI
    /// </summary>
    public class PeriodOptions
    {
        public const string Period = "Period";

        /// <summary>
        /// Период отправки данных о координатах в UI в секундах
        /// </summary>
        public int TrackerDataRefreshSeconds { get; set; }

        /// <summary>
        /// Максимальная разница с секундах между временем трека координат и текущим временем, для отсеивания поступления устаревших координат в UI
        /// </summary>
        public int ArchiveDataDelaySeconds { get; set; }
        
    }
}
