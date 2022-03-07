﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Notification.Db;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Notification.Db.Migrations
{
    [DbContext(typeof(NotificationDbContext))]
    [Migration("20210222103342_AddedSpeedZonesData")]
    partial class AddedSpeedZonesData
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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
#pragma warning restore 612, 618
        }
    }
}
