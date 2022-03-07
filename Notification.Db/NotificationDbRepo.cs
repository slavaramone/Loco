using Contracts.Requests;
using Microsoft.EntityFrameworkCore;
using Notification.Db.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notification.Db
{
	public class NotificationDbRepo : INotificationDbRepo
    {
        private readonly NotificationDbContext _db;

        public NotificationDbRepo(NotificationDbContext db)
        {
            _db = db;
        }

        public async Task<GetNotificationsResult> GetNotifications(NotificationListRequest filter)
        {
            var query = _db.Notifications.AsQueryable();
            if (filter.LocoIds != null && filter.LocoIds.Any())
            {
                query = query.Where(x => x.MapItemId.HasValue && filter.LocoIds.Contains(x.MapItemId.Value));
            }
            
            if (filter.NotificationTypes != null && filter.NotificationTypes.Any())
            {
                query = query.Where(x => filter.NotificationTypes.Contains(x.Type));
            }

            if (filter.Severities != null && filter.Severities.Any())
            {
                query = query.Where(x => filter.Severities.Contains(x.Severity));
            }

            if (filter.DateTimeFrom.HasValue)
            {
                query = query.Where(x => x.CreationDateTime >= filter.DateTimeFrom.Value);
            }

            if (filter.DateTimeTo.HasValue)
            {
                query = query.Where(x => x.CreationDateTime <= filter.DateTimeTo.Value);
            }

            if (filter.Sort?.Length > 0)
            {
                var firstSort = filter.Sort[0];

                var orderedQuery = OrderBy(query, firstSort);

                foreach (var sort in filter.Sort.Skip(1))
                    orderedQuery = ThenBy(orderedQuery, sort);

                query = orderedQuery;
            }

            int total = query.Count();

            if (filter.Skip.HasValue)
            {
                query = query.Skip(filter.Skip.Value);
            }

            if (filter.Take.HasValue)
            {
                query = query.Take(filter.Take.Value);
            }

            return new GetNotificationsResult
            {
                Notifications = await query.ToListAsync(),
                Total = total
            };
        }

		public async Task<List<SpeedZone>> GetSpeedZones()
		{
			var entities = await _db.SpeedZones.ToListAsync();
			return entities;
		}

        public async Task<Guid> AddNotification(Entities.Notification notification)
        {
            _db.Notifications.Add(notification);
            await _db.SaveChangesAsync();
            return notification.Id;
        }

        private IOrderedQueryable<Entities.Notification> OrderBy(IQueryable<Entities.Notification> query, SortFilterContract sort)
        {
            IOrderedQueryable<Entities.Notification> orderedQuery;

            if (sort.By.Equals("severity", StringComparison.OrdinalIgnoreCase))
                orderedQuery = sort.Order.Equals("desc", StringComparison.OrdinalIgnoreCase)
                    ? query.OrderByDescending(e => e.Severity)
                    : query.OrderBy(e => e.Severity);
            else if (sort.By.Equals("creationDateTime", StringComparison.OrdinalIgnoreCase))
                orderedQuery = sort.Order.Equals("desc", StringComparison.OrdinalIgnoreCase)
                    ? query.OrderByDescending(e => e.CreationDateTime)
                    : query.OrderBy(e => e.CreationDateTime);
            else if (sort.By.Equals("message", StringComparison.OrdinalIgnoreCase))
                orderedQuery = sort.Order.Equals("desc", StringComparison.OrdinalIgnoreCase)
                    ? query.OrderByDescending(e => e.Message)
                    : query.OrderBy(e => e.Message);
            else
                orderedQuery = query.OrderBy(e => e);

            return orderedQuery;
        }

        private IOrderedQueryable<Entities.Notification> ThenBy(IOrderedQueryable<Entities.Notification> query, SortFilterContract sort)
        {
            if (sort.By.Equals("severity", StringComparison.OrdinalIgnoreCase))
                query = sort.Order.Equals("desc", StringComparison.OrdinalIgnoreCase)
                    ? query.ThenByDescending(e => e.Severity)
                    : query.ThenByDescending(e => e.Severity);
            else if (sort.By.Equals("creationDateTime", StringComparison.OrdinalIgnoreCase))
                query = sort.Order.Equals("desc", StringComparison.OrdinalIgnoreCase)
                    ? query.ThenByDescending(e => e.CreationDateTime)
                    : query.ThenBy(e => e.CreationDateTime);
            else if (sort.By.Equals("message", StringComparison.OrdinalIgnoreCase))
                query = sort.Order.Equals("desc", StringComparison.OrdinalIgnoreCase)
                    ? query.ThenByDescending(e => e.Message)
                    : query.ThenBy(e => e.Message);

            return query;
        }
    }
}
