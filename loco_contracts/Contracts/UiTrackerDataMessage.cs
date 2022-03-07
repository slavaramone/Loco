using Contracts.Enums;
using System;

namespace Contracts
{
    /// <summary>
    /// Сообщение с gps данными локомотива
    /// </summary>
    public class UiTrackerDataMessage
    {
        /// <summary>
        /// Id объекта на карте (MapItemId)
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Тип объекта
        /// </summary>
        public MapItemType MapItemType { get; set; }

        /// <summary>
        /// Имя объекта
        /// </summary>
        public string MapItemName { get; set; }

        /// <summary>
        /// Данными gps трекера
        /// </summary>
        public TrackerDataMessage TrackerDataMessage { get; set; }
    }
}
