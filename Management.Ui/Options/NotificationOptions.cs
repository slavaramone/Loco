namespace Management.Ui.Options
{
	/// <summary>
	/// Опции уведомлений
	/// </summary>
	public class NotificationOptions
	{
		public const string Notification = "Notification";

		/// <summary>
		/// Время отображения уведомления
		/// </summary>
		public int LifetimeSeconds { get; set; }
	}
}
