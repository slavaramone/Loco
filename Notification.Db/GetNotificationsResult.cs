using System.Collections.Generic;

namespace Notification.Db
{
	public class GetNotificationsResult
	{
		public List<Entities.Notification> Notifications { get; set; }

		public int Total { get; set; }
	}
}
