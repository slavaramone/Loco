namespace Contracts.Requests
{
    /// <summary>
    /// Контракт сортировки
    /// </summary>
    public class SortFilterContract
    {
		/// <summary>
		/// Сортируемое поле.
		/// </summary>
		public string By { get; set; }

		/// <summary>
		/// Тип (asc, desc)
		/// </summary>
		public string Order { get; set; }
	}
}
