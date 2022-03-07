using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tracker.Db.Migrations
{
    public partial class AddedFuelDataLevelEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FuelLevels",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TrackerId = table.Column<string>(nullable: true),
                    RawValue = table.Column<double>(nullable: false),
                    CreationDateTime = table.Column<DateTime>(nullable: false),
                    ReportDateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuelLevels", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FuelLevels");
        }
    }
}
