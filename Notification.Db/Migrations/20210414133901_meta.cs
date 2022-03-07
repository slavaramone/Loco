using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Notification.Db.Migrations
{
    public partial class meta : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Metadata",
                table: "Notifications");

            migrationBuilder.AddColumn<double>(
                name: "Altitude",
                table: "Notifications",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Notifications",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Notifications",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Speed",
                table: "Notifications",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Altitude",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "Speed",
                table: "Notifications");

            migrationBuilder.AddColumn<JsonDocument>(
                name: "Metadata",
                table: "Notifications",
                type: "jsonb",
                nullable: true);
        }
    }
}
