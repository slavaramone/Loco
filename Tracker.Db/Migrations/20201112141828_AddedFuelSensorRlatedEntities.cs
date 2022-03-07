using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tracker.Db.Migrations
{
    public partial class AddedFuelSensorRlatedEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FuelSensors",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    TrackerId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuelSensors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FuelSensorRawData",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationDate = table.Column<DateTimeOffset>(nullable: false),
                    ReportDateTime = table.Column<DateTimeOffset>(nullable: false),
                    RawValue = table.Column<double>(nullable: false),
                    FuelSensorId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuelSensorRawData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FuelSensorRawData_FuelSensors_FuelSensorId",
                        column: x => x.FuelSensorId,
                        principalTable: "FuelSensors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FuelSensorRawData_FuelSensorId",
                table: "FuelSensorRawData",
                column: "FuelSensorId");

            migrationBuilder.CreateIndex(
                name: "IX_FuelSensors_TrackerId",
                table: "FuelSensors",
                column: "TrackerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FuelSensorRawData");

            migrationBuilder.DropTable(
                name: "FuelSensors");
        }
    }
}
