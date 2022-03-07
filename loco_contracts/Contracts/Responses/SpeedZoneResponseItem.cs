using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.Responses
{
	/// <summary>
	/// Элемент ответа области, отвечающей за максимальную скорость
	/// </summary>
	public class SpeedZoneResponseItem
	{
		/// <summary>
		/// Id
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Широта верхней левой точки
		/// </summary>
		public double LatitudeTopLeft { get; set; }

		/// <summary>
		/// Долгота верхней левой точки
		/// </summary>
		public double LongitudeTopLeft { get; set; }

		/// <summary>
		/// Широта верхней правой точки
		/// </summary>
		public double LatitudeTopRight { get; set; }

		/// <summary>
		/// Долгота верхней правой точки
		/// </summary>
		public double LongitudeTopRight { get; set; }

		/// <summary>
		/// Широта нижней правой точки
		/// </summary>
		public double LatitudeBottomRight { get; set; }

		/// <summary>
		/// Долгота нижней правой точки
		/// </summary>
		public double LongitudeBottomRight { get; set; }

		/// <summary>
		/// Широта нижней левой точки
		/// </summary>
		public double LatitudeBottomLeft { get; set; }

		/// <summary>
		/// Долгота нижней левой точки 
		/// </summary>
		public double LongitudeBottomLeft { get; set; }

		/// <summary>
		/// Макс скорость в зоне
		/// </summary>
		public double MaxSpeed { get; set; }
	}
}
