using Microsoft.EntityFrameworkCore.Migrations;

namespace Tracker.Db.Migrations
{
    public partial class AddedIndexesToSeveralTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_RawGeoData_TrackDate",
                table: "RawGeoData",
                column: "TrackDate");

            migrationBuilder.CreateIndex(
                name: "IX_MapItems_TrackerId",
                table: "MapItems",
                column: "TrackerId");

            migrationBuilder.CreateIndex(
                name: "IX_FuelLevels_ReportDateTime",
                table: "FuelLevels",
                column: "ReportDateTime");

            migrationBuilder.CreateIndex(
                name: "IX_FuelLevels_TrackerId",
                table: "FuelLevels",
                column: "TrackerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RawGeoData_TrackDate",
                table: "RawGeoData");

            migrationBuilder.DropIndex(
                name: "IX_MapItems_TrackerId",
                table: "MapItems");

            migrationBuilder.DropIndex(
                name: "IX_FuelLevels_ReportDateTime",
                table: "FuelLevels");

            migrationBuilder.DropIndex(
                name: "IX_FuelLevels_TrackerId",
                table: "FuelLevels");
        }
    }
}
