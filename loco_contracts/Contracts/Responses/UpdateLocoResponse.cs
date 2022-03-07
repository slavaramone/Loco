using System;

namespace Contracts.Responses
{
	/// <summary>
	/// Ответ обновления названия и активности локо
	/// </summary>
	public class UpdateLocoResponse
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
		/// Новый признак активации
		/// </summary>
		public bool IsActive { get; set; }
	}
}
