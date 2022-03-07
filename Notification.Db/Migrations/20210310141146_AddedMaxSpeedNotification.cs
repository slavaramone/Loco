using Microsoft.EntityFrameworkCore.Migrations;

namespace Notification.Db.Migrations
{
    public partial class AddedMaxSpeedNotification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "MaxSpeed",
                table: "Notifications",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxSpeed",
                table: "Notifications");
        }
    }
}
