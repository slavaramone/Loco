using Contracts.Enums;

namespace Management.Ui.Controllers
{
	/// <summary>
	/// Ответ списка статических объектов
	/// </summary>
	public class StaticObjectResponse
	{
		/// <summary>
		/// Тип объекта на карте
		/// </summary>
		public MapItemType Type { get; set; }

		/// <summary>
		/// Название
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Широта
		/// </summary>
		public double Latitude { get; set; }

		/// <summary>
		/// Долгота
		/// </summary>
		public double Longitude { get; set; }

		/// <summary>
		/// Высота
		/// </summary>
		public double? Altitude { get; set; }
	}
}
