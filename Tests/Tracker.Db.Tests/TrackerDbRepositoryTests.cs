using System;
using Microsoft.EntityFrameworkCore;
using NLog.Extensions.Logging;
using Xunit;

namespace Tracker.Db.Tests
{
	public class TrackerDbRepositoryTests
	{
		private TrackerDbContext _dbContext;

		public TrackerDbRepositoryTests()
		{
			var dbContextOptionsBuild = new DbContextOptionsBuilder<TrackerDbContext>();

			dbContextOptionsBuild.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
			dbContextOptionsBuild.UseNpgsql("Host=92.42.15.93;Port=5433;Database=Tracker-db;Username=postgres;Password=sdtdo7sySn2m8xnY");
			dbContextOptionsBuild.EnableSensitiveDataLogging(true);
			dbContextOptionsBuild.UseLoggerFactory(new NLogLoggerFactory());

			_dbContext = new TrackerDbContext(dbContextOptionsBuild.Options);
		}

		[Fact]
		public void Test1()
		{
			var repository = new TrackerDbRepo(_dbContext);

			var items = repository.GetStaticMapItemsWithLatestGeoData();
		}
	}
}