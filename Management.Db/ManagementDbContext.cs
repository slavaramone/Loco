using Management.Db.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using SharedLib;

namespace Management.Db
{
    public class ManagementDbContext : DbContext
    {
        public DbSet<Camera> Cameras { get; set; }

        public DbSet<Sensor> Sensors { get; set; }

        public DbSet<SensorGroup> SensorGroups { get; set; }

        public DbSet<FuelLevelCalibration> FuelLevelCalibrations { get; set; }

        public DbSet<Loco> Locos { get; set; }

        public DbSet<LocoApiKey> LocoApiKeys { get; set; }

        public DbSet<LocoVideoStream> LocoVideoStreams { get; set; }

        public DbSet<Shunter> Shunters { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<UserToRole> UserToRoles { get; set; }

        public ManagementDbContext(DbContextOptions<ManagementDbContext> options) : base(options)
        {
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.ReplaceService<IMigrationsAssembly, ContextAwareMigrationsAssembly<ManagementDbContext>>();
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Camera>().Property(x => x.CreationDateTimeUtc)
                .HasDefaultValueSql("timezone('utc', now())");

            modelBuilder.Entity<Loco>().Property(x => x.CreationDateTimeUtc)
                .HasDefaultValueSql("timezone('utc', now())");
            modelBuilder.Entity<Loco>().HasMany(x => x.Cameras)
                .WithOne(x => x.Loco)
                .HasForeignKey(x => x.LocoId)
                .IsRequired(false);

            modelBuilder.Entity<LocoApiKey>().HasKey(x => x.LocoId);            
            modelBuilder.Entity<LocoApiKey>()
                .HasOne(x => x.Loco)
                .WithMany(x => x.LocoApiKeys)
                .HasForeignKey(x => x.LocoId);

            modelBuilder.Entity<LocoVideoStream>()
                .HasOne(x => x.Loco)
                .WithMany(x => x.LocoVideoStreams)
                .HasForeignKey(x => x.LocoId);

            modelBuilder.Entity<Sensor>().Property(x => x.CreationDateTimeUtc)
                .HasDefaultValueSql("timezone('utc', now())");
            modelBuilder.Entity<Sensor>()
                .HasOne(x => x.SensorGroup)
                .WithMany(x => x.Sensors)
                .HasForeignKey(x => x.SensorGroupId);

            modelBuilder.Entity<SensorGroup>().Property(x => x.CreationDateTimeUtc)
                .HasDefaultValueSql("timezone('utc', now())");
            modelBuilder.Entity<SensorGroup>()
                .HasOne(x => x.Loco)
                .WithMany(x => x.SensorGroups)
                .HasForeignKey(x => x.LocoId);

            modelBuilder.Entity<FuelLevelCalibration>()
                .HasOne(x => x.FuelLevelSensor)
                .WithMany(x => x.FuelLevelCalibrations)
                .HasForeignKey(x => x.FuelLevelSensorId);

            modelBuilder.Entity<User>().Property(x => x.CreationDateTimeUtc)
                .HasDefaultValueSql("timezone('utc', now())");
            modelBuilder.Entity<User>()
                .HasIndex(p => p.Login)
                .IsUnique();

            modelBuilder.Entity<Shunter>().Property(x => x.CreationDateTimeUtc)
                .HasDefaultValueSql("timezone('utc', now())");

            modelBuilder.Entity<UserToRole>().HasKey(x => new {x.UserId, x.UserRole});
            modelBuilder.Entity<UserToRole>()
                .HasOne(x => x.User)
                .WithMany(x => x.UserToRoles)
                .HasForeignKey(x => x.UserId);
        }
    }
}
