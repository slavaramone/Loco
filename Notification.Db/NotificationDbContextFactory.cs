using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Notification.Db
{
    public class NotificationDbContextFactory : IDesignTimeDbContextFactory<NotificationDbContext>
    {
        public NotificationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<NotificationDbContext>();
            optionsBuilder.UseNpgsql("dummy");

            return new NotificationDbContext(optionsBuilder.Options);
        }
    }
}
