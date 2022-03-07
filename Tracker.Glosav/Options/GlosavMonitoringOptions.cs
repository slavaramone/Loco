using System;

namespace Tracker.Glosav.Options
{
	/// <summary>
	/// Опции мониторинга данных сервера ГЛОСАВ
	/// </summary>
	public class GlosavMonitoringOptions
	{
		public const string GlosavMonitoring = "GlosavMonitoring";

		/// <summary>
		/// Таймаут запроса данных с сревера
		/// </summary>
		public TimeSpan RequestTimeout { get; set; }

		/// <summary>
		/// Интервал, по которому запрашивается уровень топлива от текущего момента
		/// </summary>
		public TimeSpan FuelTimeIntervalFilter { get; set; }

		/// <summary>
		/// CRON выражение интервала получения координат объектов
		/// </summary>
		public string ReceiveGlosavDevicesCronExpression { get; set; }

		/// <summary>
		/// CRON выражение интервала получения показаний ДУТ
		/// </summary>
		public string ReceiveGlosavFuelCronExpression { get; set; }

		/// <summary>
		/// Базовый URL сервера ГЛОСАВ
		/// </summary>
		public string GlosavApiBaseUrl { get; set; }
	}
}
