using System.Collections.Generic;

namespace Management.Ui.Controllers
{
	/// <summary>
	/// Ответ списка скоростных зон
	/// </summary>
	public class SpeedZoneResponse
	{
		/// <summary>
		/// Вершины многоугольника по часовой стрелке
		/// </summary>
		public List<Point> Vertexes { get; set; }

		/// <summary>
		/// Макс скорость в зоне
		/// </summary>
		public double MaxSpeed { get; set; }
	}
}
