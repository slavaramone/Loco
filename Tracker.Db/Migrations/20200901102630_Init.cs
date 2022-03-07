using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tracker.Db.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MapItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    TrackerId = table.Column<string>(nullable: false),
                    CreationDate = table.Column<DateTimeOffset>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    IsStatic = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MapItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RawGeoData",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    MapItemId = table.Column<Guid>(nullable: false),
                    CreationDate = table.Column<DateTimeOffset>(nullable: false),
                    TrackDate = table.Column<DateTimeOffset>(nullable: false),
                    Latitude = table.Column<double>(nullable: false),
                    Longitude = table.Column<double>(nullable: false),
                    Altitude = table.Column<double>(nullable: true),
                    Speed = table.Column<double>(nullable: true),
                    Heading = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RawGeoData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RawGeoData_MapItems_MapItemId",
                        column: x => x.MapItemId,
                        principalTable: "MapItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PrecisionGeoData",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TrackerId = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTimeOffset>(nullable: false),
                    TrackDate = table.Column<DateTimeOffset>(nullable: false),
                    Latitude = table.Column<double>(nullable: false),
                    Longitude = table.Column<double>(nullable: false),
                    Altitude = table.Column<double>(nullable: true),
                    Speed = table.Column<double>(nullable: true),
                    Heading = table.Column<double>(nullable: true),
                    RawGeoDataId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrecisionGeoData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrecisionGeoData_RawGeoData_RawGeoDataId",
                        column: x => x.RawGeoDataId,
                        principalTable: "RawGeoData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PrecisionGeoData_CreationDate",
                table: "PrecisionGeoData",
                column: "CreationDate");

            migrationBuilder.CreateIndex(
                name: "IX_PrecisionGeoData_RawGeoDataId",
                table: "PrecisionGeoData",
                column: "RawGeoDataId");

            migrationBuilder.CreateIndex(
                name: "IX_PrecisionGeoData_TrackerId",
                table: "PrecisionGeoData",
                column: "TrackerId");

            migrationBuilder.CreateIndex(
                name: "IX_RawGeoData_CreationDate",
                table: "RawGeoData",
                column: "CreationDate");

            migrationBuilder.CreateIndex(
                name: "IX_RawGeoData_MapItemId",
                table: "RawGeoData",
                column: "MapItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrecisionGeoData");

            migrationBuilder.DropTable(
                name: "RawGeoData");

            migrationBuilder.DropTable(
                name: "MapItems");
        }
    }
}
