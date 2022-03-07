using Contracts.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace Notification.Db.Entities
{
    [Table("Notifications")]
    public class Notification
    {
        public Guid Id { get; set; }

        public Severity Severity { get; set; }
		
        public NotificationType Type { get; set; }

        public DateTimeOffset CreationDateTime { get; set; }

        public Guid? MapItemId { get; set; }

        [MaxLength(256)]
        public string Message { get; set; }
	}
}
