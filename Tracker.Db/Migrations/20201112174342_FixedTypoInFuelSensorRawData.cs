using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tracker.Db.Migrations
{
    public partial class FixedTypoInFuelSensorRawData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "FuelSensorRawData");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreationDateTime",
                table: "FuelSensorRawData",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationDateTime",
                table: "FuelSensorRawData");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreationDate",
                table: "FuelSensorRawData",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }
    }
}
