using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Management.Db
{
    public class ManagementDbContextFactory : IDesignTimeDbContextFactory<ManagementDbContext>
    {
        public ManagementDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ManagementDbContext>();
            optionsBuilder.UseNpgsql("dummy");

            return new ManagementDbContext(optionsBuilder.Options);
        }
    }
}