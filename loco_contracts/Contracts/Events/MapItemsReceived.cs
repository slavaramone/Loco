namespace Contracts.Events
{
    /// <summary>
    /// Событие при получении из БД существующих элементов, отображаемых на карте
    /// </summary>
    public interface MapItemsReceived
	{
		/// <summary>
		/// Id gps приемников объектов на карте
		/// </summary>
		string[] MapItemTrackIds { get; }
	}
}
