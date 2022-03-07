using Contracts.Requests;
using Notification.Db.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Notification.Db
{
    public interface INotificationDbRepo
    {
        Task<GetNotificationsResult> GetNotifications(NotificationListRequest filter);

		Task<List<SpeedZone>> GetSpeedZones();

		Task<Guid> AddNotification(Entities.Notification notification);
    }
}
