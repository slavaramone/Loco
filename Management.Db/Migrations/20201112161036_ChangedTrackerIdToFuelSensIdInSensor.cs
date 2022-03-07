using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Management.Db.Migrations
{
    public partial class ChangedTrackerIdToFuelSensIdInSensor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrackerId",
                table: "Sensors");

            migrationBuilder.AddColumn<Guid>(
                name: "FuelSensorId",
                table: "Sensors",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FuelSensorId",
                table: "Sensors");

            migrationBuilder.AddColumn<string>(
                name: "TrackerId",
                table: "Sensors",
                type: "text",
                nullable: true);
        }
    }
}
