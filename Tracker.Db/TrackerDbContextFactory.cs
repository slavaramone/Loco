using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Tracker.Db
{
    public class TrackerDbContextFactory : IDesignTimeDbContextFactory<TrackerDbContext>
    {
        public TrackerDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TrackerDbContext>();
            optionsBuilder.UseNpgsql("dummy");

            return new TrackerDbContext(optionsBuilder.Options);
        }
    }
}
