using Microsoft.EntityFrameworkCore.Migrations;

namespace Tracker.Db.Migrations
{
    public partial class AddedIndexesTOFuelSensorRawData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_FuelSensorRawData_RawValue",
                table: "FuelSensorRawData",
                column: "RawValue");

            migrationBuilder.CreateIndex(
                name: "IX_FuelSensorRawData_ReportDateTime",
                table: "FuelSensorRawData",
                column: "ReportDateTime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FuelSensorRawData_RawValue",
                table: "FuelSensorRawData");

            migrationBuilder.DropIndex(
                name: "IX_FuelSensorRawData_ReportDateTime",
                table: "FuelSensorRawData");
        }
    }
}
