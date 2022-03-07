using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using SharedLib;
using Tracker.Db.Entities;

namespace Tracker.Db
{
    public class TrackerDbContext : DbContext
    {
        public DbSet<FuelLevel> FuelLevels { get; set; }

        public DbSet<FuelSensor> FuelSensors { get; set; }

        public DbSet<FuelSensorRawData> FuelSensorRawData { get; set; }

        public DbSet<MapItem> MapItems { get; set; }

        public DbSet<PrecisionGeoData> PrecisionGeoData { get; set; }

        public DbSet<RawGeoData> RawGeoData { get; set; }

        public TrackerDbContext(DbContextOptions<TrackerDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ReplaceService<IMigrationsAssembly, ContextAwareMigrationsAssembly<TrackerDbContext>>();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MapItem>().HasIndex(x => x.TrackerId);

            modelBuilder.Entity<FuelSensor>().HasIndex(x => x.TrackerId);

            modelBuilder.Entity<FuelSensorRawData>().HasOne(x => x.FuelSensor)
                .WithMany(x => x.FuelSensorRawData)
                .HasForeignKey(x => x.FuelSensorId);
			modelBuilder.Entity<FuelSensorRawData>().HasIndex(x => x.ReportDateTime);
			modelBuilder.Entity<FuelSensorRawData>().HasIndex(x => x.RawValue);


			modelBuilder.Entity<FuelLevel>().HasIndex(x => x.ReportDateTime);
            modelBuilder.Entity<FuelLevel>().HasIndex(x => x.TrackerId);

            modelBuilder.Entity<RawGeoData>().HasIndex(x => x.CreationDate);
            modelBuilder.Entity<RawGeoData>().HasIndex(x => x.TrackDate);
            modelBuilder.Entity<RawGeoData>().HasOne(x => x.MapItem)
                .WithMany(x => x.RawGeoData)
                .HasForeignKey(x => x.MapItemId);

            modelBuilder.Entity<PrecisionGeoData>().HasIndex(x => x.CreationDate);
            modelBuilder.Entity<PrecisionGeoData>().HasIndex(x => x.TrackerId);
            modelBuilder.Entity<PrecisionGeoData>().HasOne(x => x.RawGeoData)
                .WithMany(x => x.PrecisionGeoData)
                .HasForeignKey(x => x.RawGeoDataId);
        }
    }
}
