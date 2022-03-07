using System.Collections.Generic;

namespace Contracts.Responses
{
	/// <summary>
	/// Ответ списка статических объектов
	/// </summary>
	public class StaticObjectListResponse
	{
		public List<StaticMapItemContract> Items { get; set; }
	}
}
