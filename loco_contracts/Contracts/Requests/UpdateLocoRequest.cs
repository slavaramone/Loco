using System;

namespace Contracts.Requests
{
	/// <summary>
	/// Запрос обновления названия и активности локо
	/// </summary>
	public class UpdateLocoRequest
	{
		/// <summary>
		/// Id локо
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Новое название
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Признак активации
		/// </summary>
		public bool IsActive { get; set; }
	}
}
