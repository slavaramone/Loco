using System.Collections.Generic;

namespace Contracts.Responses
{
	/// <summary>
	/// Ответ области, отвечающей за максимальную скорость
	/// </summary>
	public class SpeedZoneResponse
	{
		public List<SpeedZoneResponseItem> Items { get; set; }
	}
}
