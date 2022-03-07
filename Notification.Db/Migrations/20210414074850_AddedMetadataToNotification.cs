using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Notification.Db.Migrations
{
    public partial class AddedMetadataToNotification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<JsonDocument>(
                name: "Metadata",
                table: "Notifications",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Metadata",
                table: "Notifications");
        }
    }
}
