namespace Contracts.Enums
{
    /// <summary>
    /// Тип данных gps трекера
    /// </summary>
    public enum TrackerDataType
    {
        /// <summary>
        /// Сырые данные напрямую с трекера
        /// </summary>
        Raw = 1,

        /// <summary>
        /// Уточненные данных с базовой станции
        /// </summary>
        Precision = 2
    }
}
