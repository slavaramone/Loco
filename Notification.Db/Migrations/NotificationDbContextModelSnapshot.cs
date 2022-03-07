﻿// <auto-generated />
using System;
using Contracts.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Notification.Db;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Notification.Db.Migrations
{
    [DbContext(typeof(NotificationDbContext))]
    partial class NotificationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Notification.Db.Entities.Notification", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreationDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("MapItemId")
                        .HasColumnType("uuid");

                    b.Property<string>("Message")
                        .HasColumnType("character varying(256)")
                        .HasMaxLength(256);

                    b.Property<int>("Severity")
                        .HasColumnType("integer");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Notifications");

                    b.HasDiscriminator<int>("Type").HasValue(0);
                });

            modelBuilder.Entity("Notification.Db.Entities.SpeedZone", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<double>("LatitudeBottomLeft")
                        .HasColumnType("double precision");

                    b.Property<double>("LatitudeBottomRight")
                        .HasColumnType("double precision");

                    b.Property<double>("LatitudeTopLeft")
                        .HasColumnType("double precision");

                    b.Property<double>("LatitudeTopRight")
                        .HasColumnType("double precision");

                    b.Property<double>("LongitudeBottomLeft")
                        .HasColumnType("double precision");

                    b.Property<double>("LongitudeBottomRight")
                        .HasColumnType("double precision");

                    b.Property<double>("LongitudeTopLeft")
                        .HasColumnType("double precision");

                    b.Property<double>("LongitudeTopRight")
                        .HasColumnType("double precision");

                    b.Property<double>("MaxSpeed")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.ToTable("SpeedZones");
                });

            modelBuilder.Entity("Notification.Db.Entities.SpeedExceededNotification", b =>
                {
                    b.HasBaseType("Notification.Db.Entities.Notification");

                    b.Property<double>("Altitude")
                        .HasColumnType("double precision");

                    b.Property<double>("Latitude")
                        .HasColumnType("double precision");

                    b.Property<double>("Longitude")
                        .HasColumnType("double precision");

                    b.Property<double>("MaxSpeed")
                        .HasColumnType("double precision");

                    b.Property<double>("Speed")
                        .HasColumnType("double precision");

                    b.ToTable("Notifications");

                    b.HasDiscriminator().HasValue(5);
                });
#pragma warning restore 612, 618
        }
    }
}