using Contracts.Enums;
using System;

namespace Management.Ui.Models
{
	public class NotificationModel
    {
        public Guid LocoId { get; set; }

        public Severity Severity { get; set; }

        public NotificationType Type { get; set; }

		public DateTimeOffset ExpirationDateTime { get; set; }

		public string Message { get; set; }
		
		public object Metadata { get; set; }
    }
}
