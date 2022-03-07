using System;

namespace Management.Ui.Controllers
{
	/// <summary>
	/// Запрос истории локо (координат, топлива, уеведомлений)
	/// </summary>
	public class LocoHistoryRequest
	{
		/// <summary>
		/// Id локо
		/// </summary>
		public Guid LocoId { get; set; }

		/// <summary>
		/// Время UTC начала истории
		/// </summary>
		public DateTimeOffset DateTimeFrom { get; set; }


		/// <summary>
		/// Время UTC окончания истории
		/// </summary>
		public DateTimeOffset DateTimeTo { get; set; }

		/// <summary>
		/// Интервал между данныим
		/// </summary>
		public TimeSpan Interval { get; set; }
	}
}
