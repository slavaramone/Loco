﻿// <auto-generated />
using System;
using Management.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Management.Db.Migrations
{
    [DbContext(typeof(ManagementDbContext))]
    [Migration("20201014141503_ReturnedUrlToLocoVideoStream")]
    partial class ReturnedUrlToLocoVideoStream
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Management.Db.Entities.Camera", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreationDateTimeUtc")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("timezone('utc', now())");

                    b.Property<Guid?>("LocoId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("NucNumber")
                        .HasColumnType("text");

                    b.Property<string>("Number")
                        .HasColumnType("text");

                    b.Property<int>("Position")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("LocoId");

                    b.ToTable("Cameras");
                });

            modelBuilder.Entity("Management.Db.Entities.FuelLevelCalibration", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<double>("CalibratedValue")
                        .HasColumnType("double precision");

                    b.Property<Guid>("FuelLevelSensorId")
                        .HasColumnType("uuid");

                    b.Property<double>("RawValue")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.HasIndex("FuelLevelSensorId");

                    b.ToTable("FuelLevelCalibrations");
                });

            modelBuilder.Entity("Management.Db.Entities.Loco", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreationDateTimeUtc")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("timezone('utc', now())");

                    b.Property<Guid?>("MapItemId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Locos");
                });

            modelBuilder.Entity("Management.Db.Entities.LocoApiKey", b =>
                {
                    b.Property<Guid>("LocoId")
                        .HasColumnType("uuid");

                    b.Property<string>("ApiKey")
                        .HasColumnType("text");

                    b.HasKey("LocoId");

                    b.ToTable("LocoApiKeys");
                });

            modelBuilder.Entity("Management.Db.Entities.LocoVideoStream", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("CameraPosition")
                        .HasColumnType("integer");

                    b.Property<Guid>("LocoId")
                        .HasColumnType("uuid");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("LocoId");

                    b.ToTable("LocoVideoStreams");
                });

            modelBuilder.Entity("Management.Db.Entities.Sensor", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreationDateTimeUtc")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("timezone('utc', now())");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<Guid>("SensorGroupId")
                        .HasColumnType("uuid");

                    b.Property<string>("TrackerId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("SensorGroupId");

                    b.ToTable("Sensors");
                });

            modelBuilder.Entity("Management.Db.Entities.SensorGroup", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreationDateTimeUtc")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("timezone('utc', now())");

                    b.Property<bool>("IsTakeAverageValue")
                        .HasColumnType("boolean");

                    b.Property<Guid>("LocoId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("LocoId");

                    b.ToTable("SensorGroups");
                });

            modelBuilder.Entity("Management.Db.Entities.Shunter", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreationDateTimeUtc")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("timezone('utc', now())");

                    b.Property<Guid?>("MapItemId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Shunters");
                });

            modelBuilder.Entity("Management.Db.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreationDateTimeUtc")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("timezone('utc', now())");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("character varying(256)")
                        .HasMaxLength(256);

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("LastName")
                        .HasColumnType("character varying(256)")
                        .HasMaxLength(256);

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("character varying(512)")
                        .HasMaxLength(512);

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("character varying(256)")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("character varying(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("Login")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Management.Db.Entities.UserToRole", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<int>("UserRole")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset>("CreationDateTimeUtc")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("UserId", "UserRole");

                    b.ToTable("UserToRoles");
                });

            modelBuilder.Entity("Management.Db.Entities.Camera", b =>
                {
                    b.HasOne("Management.Db.Entities.Loco", "Loco")
                        .WithMany("Cameras")
                        .HasForeignKey("LocoId");
                });

            modelBuilder.Entity("Management.Db.Entities.FuelLevelCalibration", b =>
                {
                    b.HasOne("Management.Db.Entities.Sensor", "FuelLevelSensor")
                        .WithMany("FuelLevelCalibrations")
                        .HasForeignKey("FuelLevelSensorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Management.Db.Entities.LocoApiKey", b =>
                {
                    b.HasOne("Management.Db.Entities.Loco", "Loco")
                        .WithMany("LocoApiKeys")
                        .HasForeignKey("LocoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Management.Db.Entities.LocoVideoStream", b =>
                {
                    b.HasOne("Management.Db.Entities.Loco", "Loco")
                        .WithMany("LocoVideoStreams")
                        .HasForeignKey("LocoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Management.Db.Entities.Sensor", b =>
                {
                    b.HasOne("Management.Db.Entities.SensorGroup", "SensorGroup")
                        .WithMany("Sensors")
                        .HasForeignKey("SensorGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Management.Db.Entities.SensorGroup", b =>
                {
                    b.HasOne("Management.Db.Entities.Loco", "Loco")
                        .WithMany("SensorGroups")
                        .HasForeignKey("LocoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Management.Db.Entities.UserToRole", b =>
                {
                    b.HasOne("Management.Db.Entities.User", "User")
                        .WithMany("UserToRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
