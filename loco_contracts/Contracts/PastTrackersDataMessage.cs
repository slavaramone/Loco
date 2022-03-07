using System.Collections.Generic;

namespace Contracts
{
    /// <summary>
    /// Сообщение с архивными данными координат устройств
    /// </summary>
    public class PastTrackersDataMessage
    {
        /// <summary>
        /// Набор архивных данных
        /// </summary>
        public List<TrackerDataMessage> TrackersData { get; set; }
    }
}
