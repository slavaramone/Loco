namespace Contracts.Requests
{
    /// <summary>
    /// Запрос списка локомотивов
    /// </summary>
    public class LocoListRequest
    {
		/// <summary>
		/// Возврат толькр активных локо
		/// </summary>
		public bool IsOnlyActive { get; set; }
	}
}
