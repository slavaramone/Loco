using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Notification.Db.Migrations
{
    public partial class AddedSpeedZoneEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SpeedZones",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    LatitudeTopLeft = table.Column<double>(nullable: false),
                    LongitudeTopLeft = table.Column<double>(nullable: false),
                    LatitudeTopRight = table.Column<double>(nullable: false),
                    LongitudeTopRight = table.Column<double>(nullable: false),
                    LatitudeBottomRight = table.Column<double>(nullable: false),
                    LongitudeBottomRight = table.Column<double>(nullable: false),
                    LatitudeBottomLeft = table.Column<double>(nullable: false),
                    LongitudeBottomLeft = table.Column<double>(nullable: false),
                    MaxSpeed = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpeedZones", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpeedZones");
        }
    }
}
