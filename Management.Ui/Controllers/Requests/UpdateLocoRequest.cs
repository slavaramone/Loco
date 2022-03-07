using System;

namespace Management.Ui.Controllers
{
	/// <summary>
	/// Запрос обновления названия и активности локо
	/// </summary>
	public class UpdateLocoRequest
	{
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
