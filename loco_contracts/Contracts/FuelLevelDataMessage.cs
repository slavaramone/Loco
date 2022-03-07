using System;

namespace Contracts
{
	/// <summary>
	/// Сообщение поступления "сырых" данных от ДУТ из ГЛОСАВ
	/// </summary>
	public interface FuelLevelDataMessage
	{
		/// <summary>
		/// "Сырые" показания ДУТ
		/// </summary>
		double FuelLevel { get; }

		/// <summary>
		/// Внешний Id ДУТ
		/// </summary>
		string TrackerId { get; }

		/// <summary>
		/// Время снятия показаний
		/// </summary>
		DateTimeOffset ReportDateTime { get; }
	}
}
