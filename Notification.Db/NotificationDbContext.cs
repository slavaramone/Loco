using Contracts.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Notification.Db.Entities;
using SharedLib;

namespace Notification.Db
{
	public class NotificationDbContext : DbContext
	{
		public DbSet<Entities.Notification> Notifications { get; set; }

		public DbSet<SpeedZone> SpeedZones { get; set; }

		public NotificationDbContext(DbContextOptions<NotificationDbContext> options) : base(options)
		{
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.ReplaceService<IMigrationsAssembly, ContextAwareMigrationsAssembly<NotificationDbContext>>();
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Entities.Notification>()
				.HasDiscriminator(x => x.Type)
				.HasValue<Entities.Notification>(NotificationType.Custom);
			
			modelBuilder.Entity<Entities.SpeedExceededNotification>()
				.HasBaseType<Entities.Notification>()
				.HasDiscriminator(x => x.Type)
				.HasValue<Entities.SpeedExceededNotification>(NotificationType.Speed);
		}
	}
}