using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Notification.Db.Migrations
{
    public partial class AddedMapItemIdToNotification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MapItemId",
                table: "Notifications",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MapItemId",
                table: "Notifications");
        }
    }
}
